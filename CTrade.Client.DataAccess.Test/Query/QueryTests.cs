using CTrade.Client.DataAccess.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CTrade.Client.DataAccess.Test.Query
{
    [TestClass]
    public class QueryTests : TestBase
    {
        private Artist[] _artists;
        [TestInitialize]
        public void Setup()
        {
            _artists = new QueryFixture().Init();
        }

        [TestMethod]
        public void CanReduceWithStandardSumWithoutKeysSpecified()
        {
            var expectedSum = _artists.Sum(a => a.Albums.Count());
            var queryRequest = QueryRequestFactory.CreateIndexQuery(ClientTestData.Views.ArtistDesignDoc,
                ClientTestData.Views.ArtistsTotalNumOfAlbumsView);
            queryRequest.Configure(q => q.Reduce(true));

            var queryResp = Repository.QueryAsync(queryRequest).Result;

            queryResp.HasNoError();
            Assert.AreEqual(1, queryResp.RowCount);
            Assert.AreEqual(expectedSum, queryResp.Rows[0].Value);
        }

        [TestMethod]
        public void IndexWithoutValueWithIncludeDocsShouldReturnDocsButNoValues()
        {
            var queryRequest = QueryRequestFactory.CreateIndexQuery(ClientTestData.Views.ArtistDesignDoc,
                ClientTestData.Views.ArtistsNameNoValueView).Configure(cfg => cfg.IncludeDocs(true));

            var queryResp = Repository.QueryAsync(queryRequest).Result;

            queryResp.HasNoError();
            Assert.AreEqual(_artists.Length, queryResp.RowCount);
            Assert.IsTrue(queryResp.Rows.All(r => r.Value == null));
            for (var i = 0; i < queryResp.RowCount; i++)
                AssertEqual(queryResp.Rows[i].IncludedDoc, _artists[i]);
        }

        [TestMethod]
        public void Skipping2Of10ShouldReturn8Rows()
        {
            var artists = _artists.Skip(2).ToArray();
            var queryRequest = QueryRequestFactory.CreateIndexQuery(ClientTestData.Views.ArtistDesignDoc,
                ClientTestData.Views.ArtistsNameAsKeyAndDocAsValue).Configure(q => q.Skip(2));

            var queryResp = Repository.QueryAsync(queryRequest).Result;

            queryResp.HasNoError();
            Assert.IsFalse(queryResp.IsEmpty);
            Assert.AreEqual(8, queryResp.RowCount);
            var rows = queryResp.Rows.OrderBy(r => r.Id).ToArray();
            for (var i = 0; i < queryResp.RowCount; i++)
                AssertEqual(rows[i].Value, artists[i]);
        }

        [TestMethod]
        public void WhenLimitTo2ShouldReturn2Rows()
        {
            var artists = _artists.Take(2).ToArray();
            var queryRequest = QueryRequestFactory.CreateIndexQuery(ClientTestData.Views.ArtistDesignDoc,
                ClientTestData.Views.ArtistsNameAsKeyAndDocAsValue).Configure(q => q.Limit(2));

            var queryResp = Repository.QueryAsync(queryRequest).Result;

            queryResp.HasNoError();
            Assert.IsFalse(queryResp.IsEmpty);
            Assert.AreEqual(2, queryResp.RowCount);
            var rows = queryResp.Rows.OrderBy(r => r.Id).ToArray();
            for (var i = 0; i < queryResp.RowCount; i++)
                AssertEqual(rows[i].Value, artists[i]);
        }

        [TestMethod]
        public void WhenKeySpecifiedThenMatchingRowIsReturned()
        {
            var artist = _artists[2];
            var queryRequest = QueryRequestFactory.CreateIndexQuery(ClientTestData.Views.ArtistDesignDoc,
                ClientTestData.Views.ArtistsNameAsKeyAndDocAsValue).Configure(q => q.Key(artist.Name));

            var queryResp = Repository.QueryAsync(queryRequest).Result;

            queryResp.HasNoError();
            Assert.IsFalse(queryResp.IsEmpty);
            Assert.AreEqual(1, queryResp.RowCount);
            for (var i = 0; i < queryResp.RowCount; i++)
                AssertEqual(queryResp.Rows[i].Value, artist);
        }

        [TestMethod]
        public void WhenKeysSpecifiedThenMatchingRowsAreReturned()
        {
            var artists = _artists.Skip(2).Take(3).ToArray();
            var keys = artists.Select(a => a.Name).ToArray();
            var queryRequest = QueryRequestFactory.CreateIndexQuery(ClientTestData.Views.ArtistDesignDoc,
                ClientTestData.Views.ArtistsNameAsKeyAndDocAsValue).Configure(q => q.Keys(keys));

            var queryResp = Repository.QueryAsync(queryRequest).Result;

            queryResp.HasNoError();
            Assert.IsFalse(queryResp.IsEmpty);
            var rows = queryResp.Rows.OrderBy(r => r.Id).ToArray();
            for (var i = 0; i < queryResp.RowCount; i++)
                AssertEqual(rows[i].Value, artists[i]);
        }

        [TestMethod]
        public void WhenStartAndEndKeysSpecifiedWithNonInclusiveOptionThenMatchingRowsAreReturned()
        {
            var artists = _artists.Skip(2).Take(5).OrderBy(a => a.Id).ToArray();
            var queryRequest = QueryRequestFactory.CreateIndexQuery(ClientTestData.Views.ArtistDesignDoc,
                ClientTestData.Views.ArtistsNameAsKeyAndDocAsValue).Configure(q => q
                    .StartKey(artists.First().Name)
                    .EndKey(artists.Last().Name)
                    .InclusiveEnd(false)
                    );

            var queryResp = Repository.QueryAsync(queryRequest).Result;

            queryResp.HasNoError();
            Assert.IsFalse(queryResp.IsEmpty);
            artists = artists.Take(artists.Length - 1).ToArray();
            var rows = queryResp.Rows.OrderBy(r => r.Id).ToArray();
            for (var i = 0; i < queryResp.RowCount; i++)
                AssertEqual(rows[i].Value, artists[i]);
        }

        static void AssertEqual(dynamic doc, Artist artist)
        {
            Assert.AreEqual((string)doc._id, artist.Id);
            Assert.AreEqual((string)doc._rev, artist.Rev);
            Assert.AreEqual((string)doc.name, artist.Name);
            for (var i = 0; i < artist.Albums.Length; i++)
                Assert.AreEqual(artist.Albums[i].Name, (string)doc.albums[i].name);
        }
    }
}
