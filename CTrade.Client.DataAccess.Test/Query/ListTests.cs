using CTrade.Client.DataAccess.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCouch.Net;
using MyCouch.Requests;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace CTrade.Client.DataAccess.Test.Query
{
    [TestClass]
    public class ListTests : TestBase
    {
        private Artist[] _artists;
        [TestInitialize]
        public void Setup()
        {
            _artists = new QueryFixture().Init();
        }

        [TestMethod]
        public void ShouldTransformAllViewRowsWhenQueryParametersNotSupplied()
        {
            var queryRequest = QueryRequestFactory.CreateListQuery(
                ClientTestData.Views.ArtistDesignDoc,
                ClientTestData.Views.TransformToDocList,
                ClientTestData.Views.ArtistsNameAsKeyAndDocAsValue);

            var queryResp = Repository.QueryAsync(queryRequest).Result;

            queryResp.HasNoError();
            Assert.IsTrue(queryResp.IsJson);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(queryResp.Etag));
            Assert.AreEqual(HttpStatusCode.OK, queryResp.HttpHeaderInfo.StatusCode);
        }

        [TestMethod]
        public void WhenKeySpecifiedThenMatchingRowIsTransformed()
        {
            const string keyToReturn = "Fake artist 1";
            var queryRequest = QueryRequestFactory.CreateListQuery(
                ClientTestData.Views.ArtistDesignDoc,
                ClientTestData.Views.TransformToDocList,
                ClientTestData.Views.ArtistsNameAsKeyAndDocAsValue)
                .Configure(q => q.Key(keyToReturn));

            var queryResp = Repository.QueryAsync(queryRequest).Result;

            queryResp.HasNoError();
            Assert.IsTrue(queryResp.IsJson);
            var contentArray = queryResp.JsonContent.Cast<dynamic>();
            Assert.AreEqual(1, contentArray.Count());
        }

        [TestMethod]
        public void WhenKeysAreSpecifiedThenMatchingRowsAreTransformed()
        {
            var artists = _artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var queryRequest = QueryRequestFactory.CreateListQuery(
                ClientTestData.Views.ArtistDesignDoc,
                ClientTestData.Views.TransformToDocList,
                ClientTestData.Views.ArtistsNameAsKeyAndDocAsValue)
                .Configure(q => q.Keys(keys));

            var queryResp = Repository.QueryAsync(queryRequest).Result;

            queryResp.HasNoError();
            Assert.IsTrue(queryResp.IsJson);
            var contentArray = queryResp.JsonContent.Cast<dynamic>();
            Assert.AreEqual(3, contentArray.Count());
        }

        [TestMethod]
        public void CanTransformToHtml()
        {
            var queryRequest = QueryRequestFactory.CreateListQuery(
                ClientTestData.Views.ArtistDesignDoc,
                ClientTestData.Views.TransformToHtmlList,
                ClientTestData.Views.ArtistsNameAsKeyAndDocAsValue)
                .Configure(q => q.ContentType(HttpContentTypes.Html));

            var queryResp = Repository.QueryAsync(queryRequest).Result;

            queryResp.HasNoError();
            Assert.IsTrue(queryResp.IsHtml);
            Assert.AreEqual(HttpStatusCode.OK, queryResp.HttpHeaderInfo.StatusCode);
        }

        [TestMethod]
        public void ListTransformsCanUtilizeAdditionalQueryParametersSentWithRequests()
        {
            var artists = _artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var additionalQueryParams = new Dictionary<string, object>();
            additionalQueryParams.Add("foo", "bar");
            var queryRequest = QueryRequestFactory.CreateListQuery(
                ClientTestData.Views.ArtistDesignDoc,
                ClientTestData.Views.TransformToJsonWithAdditonalQueryParamsList,
                ClientTestData.Views.ArtistsNameAsKeyAndDocAsValue)
                .Configure(q => q.Keys(keys)
                    .AdditionalQueryParameters(additionalQueryParams));

            var queryResp = Repository.QueryAsync(queryRequest).Result;

            queryResp.HasNoError();
            Assert.IsTrue(queryResp.IsJson);
            var transformedDocs = queryResp.JsonContent.Cast<dynamic>().ToArray();
            Assert.AreEqual(3, transformedDocs.Count());
            for (var i = 0; i < keys.Length; i++)
            {
                Assert.AreEqual(keys[i], (string)transformedDocs[i].name);
                Assert.AreEqual("\"bar\"", (string)transformedDocs[i].foo);
            }
        }
    }
}
