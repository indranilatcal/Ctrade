using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Net;

namespace CTrade.Client.DataAccess.Test.Crud
{
    [TestClass]
    public class GetTests : TestBase
    {
        [TestMethod]
        public void TestIdIsMandatoryForGet()
        {
            var getResponse = Repository.GetAsync(string.Empty).Result;

            Assert.IsTrue(getResponse.HttpHeaderInfo.HasError);
            Assert.AreEqual(HttpStatusCode.BadRequest, getResponse.HttpHeaderInfo.StatusCode);
            Assert.AreEqual("Id is mandatory for Get Operation", getResponse.HttpHeaderInfo.Error);
        }

        [TestMethod]
        public void TestReadWithNonExistentId()
        {
            var getResponse = Repository.GetAsync("junk").Result;
            Assert.IsTrue(getResponse.HttpHeaderInfo.HasError);
            Assert.AreEqual(HttpStatusCode.NotFound, getResponse.HttpHeaderInfo.StatusCode);
        }

        [TestMethod]
        public void TestGetWithSimpleId()
        {
            dynamic album = new JObject();
            album.title = "My album";
            album.singer = new JObject();
            album.singer.fullName = "Bruce";
            album.songs = new JArray();
            dynamic song1 = new JObject();
            song1.title = "song1";
            album.songs.Add(song1);
            var createResponse = Repository.CreateAsync(album as JObject).Result;

            var getResponse = Repository.GetAsync(createResponse.Id).Result;

            Assert.AreEqual(createResponse.Id, getResponse.Id);
            Assert.AreEqual(createResponse.Revision, getResponse.Revision);
            Assert.IsNotNull(getResponse.Data);
            dynamic readData = getResponse.Data;
            Assert.AreEqual("My album", (string)readData.title);
            Assert.AreEqual("Bruce", (string)readData.singer.fullName);
        }
    }
}
