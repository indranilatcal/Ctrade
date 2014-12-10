using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Net;

namespace CTrade.Client.DataAccess.Test.Crud
{
    [TestClass]
    public class DeleteTests : TestBase
    {
        [TestMethod]
        public void TestDeleteNeedsToIncludeIdAndRev()
        {
            var response = Repository.DeleteAsync(null, null).Result;
                
            Assert.IsTrue(response.HttpHeaderInfo.HasError);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.HttpHeaderInfo.StatusCode);
            Assert.AreEqual("Id and Revision are mandatory for Delete operation", response.HttpHeaderInfo.Error);
        }

        [TestMethod]
        public void TestDeleteWithExistingIdAndRev()
        {
            dynamic data = new JObject();
            data.IsEmpty = true;
            var response = Repository.CreateAsync(data as JObject).Result;

            response = Repository.DeleteAsync(response.Id, response.Revision).Result;

            Assert.IsFalse(response.HttpHeaderInfo.HasError);
        }
    }
}
