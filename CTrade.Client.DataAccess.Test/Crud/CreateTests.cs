using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace CTrade.Client.DataAccess.Test.Crud
{
    [TestClass]
    public class CreateTests : TestBase
    {
        [TestMethod]
        public void TestCreateWithoutIdShouldGenerateId()
        {
            dynamic data = new JObject();
            data.Text = "Hello";

            var response = Repository.CreateAsync(data as JObject).Result;

            Assert.IsFalse(string.IsNullOrWhiteSpace(response.Id));
        }

        [TestMethod]
        public void TestCreateWithValidIdShouldUseIt()
        {
            dynamic data = new JObject();
            string id = Guid.NewGuid().ToString();

            var response = Repository.CreateAsync(data as JObject, id).Result;

            Assert.AreEqual(id, response.Id);
        }

        [TestMethod]
        public void TestCreateShouldFailWithEmptyData()
        {
            var response = Repository.CreateAsync(new JObject()).Result;

            Assert.IsTrue(response.HttpHeaderInfo.HasError);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.HttpHeaderInfo.StatusCode);
            Assert.AreEqual("Data cannot be empty", response.HttpHeaderInfo.Error);
        }

        [TestMethod]
        public void TestCreateWithBlankData()
        {
            dynamic data = new JObject();
            data.IsEmpty = true;

            var response = Repository.CreateAsync(data as JObject).Result;

            Assert.IsFalse(string.IsNullOrWhiteSpace(response.Id));
            Assert.IsFalse(string.IsNullOrWhiteSpace(response.Revision));
        }

        [TestMethod]
        public void TestCreateFailsWithAlreadyExistingIdIsSpecified()
        {
            dynamic data = new JObject();
            data.IsEmpty = true;
            var response = Repository.CreateAsync(data as JObject).Result;

            response = Repository.CreateAsync(data as JObject, response.Id).Result;

            Assert.IsTrue(string.IsNullOrWhiteSpace(response.Id));
            Assert.IsTrue(string.IsNullOrWhiteSpace(response.Revision));
            Assert.IsTrue(response.HttpHeaderInfo.HasError);
            Assert.AreEqual(HttpStatusCode.Conflict, response.HttpHeaderInfo.StatusCode);
        }

        [TestMethod]
        public void TestCreateWithDataWithScalarAndArrayProperties()
        {
            dynamic complexData = new JObject();
            //Scalar attributes
            complexData.firstName = "John";
            complexData.lastName = "Dikes";
            //Nested Object
            complexData.address = new JObject();
            complexData.address.zip = 201304;
            complexData.address.city = "Boston";
            //Array
            complexData.accounts = new JArray();

            dynamic acct1 = new JObject();
            acct1.accountType = "checking";
            acct1.accountNo = "123";
            dynamic acct2 = new JObject();
            acct1.accountType = "savings";
            acct1.accountNo = "xyz";

            complexData.accounts.Add(acct1);
            complexData.accounts.Add(acct2);

            var response = Repository.CreateAsync(complexData as JObject).Result;

            Assert.IsFalse(string.IsNullOrWhiteSpace(response.Id));
            Assert.IsFalse(string.IsNullOrWhiteSpace(response.Revision));
        }
    }
}
