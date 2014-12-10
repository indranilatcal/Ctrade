using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Net;

namespace CTrade.Client.DataAccess.Test.Crud
{
    [TestClass]
    public class UpdateTests : TestBase
    {
        [TestMethod]
        public void TestUpdateNeedsToIncludeIdAndRev()
        {
            var response = Repository.UpdateAsync(null, null, null).Result;
            
            Assert.IsTrue(response.HttpHeaderInfo.HasError);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.HttpHeaderInfo.StatusCode);
            Assert.AreEqual("Id and Revision are mandatory for Update operation", response.HttpHeaderInfo.Error);
        }

        [TestMethod]
        public void TestUpdateShouldFailWithEmptyData()
        {
            var response = Repository.UpdateAsync("igonredId", "ignoredRev", new JObject()).Result;

            Assert.IsTrue(response.HttpHeaderInfo.HasError);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.HttpHeaderInfo.StatusCode);
            Assert.AreEqual("Data cannot be empty", response.HttpHeaderInfo.Error);
        }

        [TestMethod]
        public void TestSimpleUpdate()
        {
            dynamic data = new JObject();
            data.Text = "Hello";
            var createResponse = Repository.CreateAsync(data as JObject).Result;
            var getResponse = Repository.GetAsync(createResponse.Id).Result;
            dynamic dataToUpdate = getResponse.Data;

            dataToUpdate.Text = "Hello2";
            var updateResponse = Repository.UpdateAsync(getResponse.Id, getResponse.Revision, dataToUpdate as JObject).Result;

            Assert.IsFalse(updateResponse.HttpHeaderInfo.HasError);
            dynamic updatedData = Repository.GetAsync(updateResponse.Id, updateResponse.Revision).Result.Data;
            Assert.AreEqual((string)dataToUpdate.Text, (string)updatedData.Text);
        }

        [TestMethod]
        public void TestComplexUpdate()
        {
            dynamic data = new JObject();
            data.Text = "Hello";
            var createResponse = Repository.CreateAsync(data as JObject).Result;
            var getResponse = Repository.GetAsync(createResponse.Id).Result;
            dynamic dataToUpdate = getResponse.Data;

            dataToUpdate.address = new JObject();
            dataToUpdate.address.city = "Boston";
            dataToUpdate.address.zip = "xyz";

            var updateResponse = Repository.UpdateAsync(getResponse.Id, getResponse.Revision, dataToUpdate as JObject).Result;

            Assert.IsFalse(updateResponse.HttpHeaderInfo.HasError);
            dynamic updatedData = Repository.GetAsync(updateResponse.Id, updateResponse.Revision).Result.Data;
            Assert.AreEqual((string)dataToUpdate.address.city, (string)updatedData.address.city);
            Assert.AreEqual((string)dataToUpdate.address.zip, (string)updatedData.address.zip);
            Assert.AreEqual((string)data.Text, (string)updatedData.Text);
        }
    }
}
