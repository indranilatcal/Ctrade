using CTrade.Client.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CTrade.Client.DataAccess.Test.Initialization
{
    [TestClass]
    public class InitializationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestUsernameCannotBeNull()
        {
            var repository = new DbRepository(null, "ignore", "ignore", "ignore");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestUsernameCannotBeEmpty()
        {
            var repository = new DbRepository(string.Empty, "ignore", "ignore", "ignore");
        }

        [TestMethod]
        public void TestInvalidUsernameReportsAppropriateError()
        {
            var repository = new DbRepository("junk", "apitest", "bysoustryinscrieringrect", "XMPmAIhkhLMJ6A7dWdepcHNh");
            var response = repository.GetAsync("ignored").Result;

            Assert.IsTrue(response.HttpHeaderInfo.HasError);
            Assert.AreEqual(HttpStatusCode.ServiceUnavailable, response.HttpHeaderInfo.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestDbnameCannotBeNull()
        {
            var repository = new DbRepository("ignore", null, "ignore", "ignore");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestDbnameCannotBeEmpty()
        {
            var repository = new DbRepository("ignore", string.Empty, "ignore", "ignore");
        }

        [TestMethod]
        public void TestInvalidDbNameReportsAppropriateError()
        {
            var repository = new DbRepository("countertrade", "junk", "bysoustryinscrieringrect", "XMPmAIhkhLMJ6A7dWdepcHNh");
            var response = repository.GetAsync("ignored").Result;

            Assert.IsTrue(response.HttpHeaderInfo.HasError);
            Assert.AreEqual(HttpStatusCode.Forbidden, response.HttpHeaderInfo.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestKeyCannotBeNull()
        {
            var repository = new DbRepository("ignore", "ignore", null, "ignore");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestKeyCannotBeEmpty()
        {
            var repository = new DbRepository("ignore", "ignore", string.Empty, "ignore");
        }

        [TestMethod]
        public void TestInvalidKeyReportsAppropriateError()
        {
            var repository = new DbRepository("countertrade", "apitest", "junk", "XMPmAIhkhLMJ6A7dWdepcHNh");
            var response = repository.GetAsync("ignored").Result;

            Assert.IsTrue(response.HttpHeaderInfo.HasError);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.HttpHeaderInfo.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPwdCannotBeNull()
        {
            var repository = new DbRepository("ignore", "ignore", "ignore", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPwdCannotBeEmpty()
        {
            var repository = new DbRepository("ignore", "ignore", "ignore", string.Empty);
        }


        [TestMethod]
        public void TestInvalidPwdReportsAppropriateError()
        {
            var repository = new DbRepository("countertrade", "apitest", "bysoustryinscrieringrect", "junk");
            var response = repository.GetAsync("ignored").Result;

            Assert.IsTrue(response.HttpHeaderInfo.HasError);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.HttpHeaderInfo.StatusCode);
        }
    }
}
