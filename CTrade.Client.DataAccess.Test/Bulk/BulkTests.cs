using CTrade.Client.DataAccess.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCouch;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CTrade.Client.DataAccess.Test.Bulk
{
    [TestClass]
    public class BulkTests : TestBase
    {
        [TestInitialize]
        public void Setup()
        {
            TestRuntime.EnsureCleanup();
        }

        [TestMethod]
        public void TestBulkCreates()
        {
            JObject data1 = CreateDataWithId(Guid.NewGuid().ToString(), "testfirst1", "testlast1");
            JObject data2 = CreateDataWithoutId("testfirst2", "testlast2");

            var bulkRequest = BulkRequestFactory.Create();
            bulkRequest.Include(data1, data2);

            var bulkResp = Repository.BulkAsync(bulkRequest).Result;

            Assert.IsFalse(bulkResp.IsEmpty);
            Assert.AreEqual(2, bulkResp.Rows.Length);
            Assert.IsTrue(bulkResp.Rows.All(r => !string.IsNullOrWhiteSpace(r.Revision) && r.Succeeded));
        }

        [TestMethod]
        public void TestBulkUpdates()
        {
            JObject data1 = CreateDataWithId(Guid.NewGuid().ToString(), "testfirst1", "testlast1");
            JObject data2 = CreateDataWithoutId("testfirst2", "testlast2");
            var bulkCreateRequest = BulkRequestFactory.Create();
            bulkCreateRequest.Include(data1, data2);
            var createdRows = Repository.BulkAsync(bulkCreateRequest).Result.Rows;
            dynamic dataToUpdate1 = CreateDataWithoutId("testfirst1Updated", "testlast1Updated");
            dataToUpdate1.id = createdRows.First().Id;
            dataToUpdate1.rev = createdRows.First().Revision;
            dynamic dataToUpdate2 = CreateDataWithoutId("testfirst2Updated", "testlast2Updated");
            dataToUpdate2.id = createdRows.Last().Id;
            dataToUpdate2.rev = createdRows.Last().Revision;

            var bulkUpdateRequest = BulkRequestFactory.Create();
            bulkUpdateRequest.Include(dataToUpdate1, dataToUpdate2);
            var updateResp = Repository.BulkAsync(bulkUpdateRequest).Result;
            Assert.IsFalse(updateResp.IsEmpty);
            Assert.AreEqual(2, updateResp.Rows.Length);
            Assert.IsTrue(updateResp.Rows.All(r => !string.IsNullOrWhiteSpace(r.Revision) && r.Succeeded));
            var updatedRow1 = Repository.GetAsync(updateResp.Rows.First().Id, updateResp.Rows.First().Revision).Result;
            var updatedRow2 = Repository.GetAsync(updateResp.Rows.Last().Id, updateResp.Rows.Last().Revision).Result;
            Assert.AreEqual("testfirst1Updated", ((string)((dynamic)updatedRow1.Data).firstName));
            Assert.AreEqual("testfirst2Updated", ((string)((dynamic)updatedRow2.Data).firstName));
        }

        [TestMethod]
        public void TestBulkDeletes()
        {
            JObject data1 = CreateDataWithId(Guid.NewGuid().ToString(), "testfirst1", "testlast1");
            JObject data2 = CreateDataWithoutId("testfirst2", "testlast2");
            var bulkCreateRequest = BulkRequestFactory.Create();
            bulkCreateRequest.Include(data1, data2);
            var createdRows = Repository.BulkAsync(bulkCreateRequest).Result.Rows;
            var bulkDeleteRequest = BulkRequestFactory.Create();
            bulkDeleteRequest.Delete(
                new DocumentHeader(createdRows.First().Id, createdRows.First().Revision),
                new DocumentHeader(createdRows.Last().Id, createdRows.Last().Revision)
                );

            var deleteResp = Repository.BulkAsync(bulkDeleteRequest).Result;

            Assert.IsFalse(deleteResp.HttpHeaderInfo.HasError);
        }

        #region Private Helpers
        private JObject CreateDataWithId(string id, string firstName, string lastName)
        {
            dynamic obj = CreateDataWithoutId(firstName, lastName);
            obj._id = id;

            return obj;
        }

        private JObject CreateDataWithoutId(string firstName, string lastName)
        {
            dynamic obj = new JObject();
            obj.firstName = firstName;
            obj.lastName = lastName;

            return obj;
        }
        #endregion
    }
}
