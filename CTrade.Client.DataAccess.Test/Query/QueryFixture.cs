using CTrade.Client.DataAccess.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace CTrade.Client.DataAccess.Test.Query
{
    internal class QueryFixture
    {
        private Artist[] _artists;
        internal Artist[] Init()
        {
            if (_artists != null && _artists.Any())
                return _artists;
            TestRuntime.EnsureCleanup();
            _artists = ClientTestData.Artists.CreateArtists(10);

            
            var repository = TestRuntime.CreateRepository();
            var bulkRequest = BulkRequestFactory.Create();
            bulkRequest.Include(_artists.Select(a => a.AsJObject()).ToArray());
            var bulkResponse = repository.BulkAsync(bulkRequest).Result;
            Assert.IsFalse(bulkResponse.HttpHeaderInfo.HasError);
            foreach (var row in bulkResponse.Rows)
            {
                var artist = _artists.Single(i => i.Id == row.Id);
                artist.Rev = row.Revision;
            }
            var indexResponse = repository.CreateAsync(JObject.Parse(ClientTestData.Views.ArtistsViews)).Result;
            Assert.IsFalse(indexResponse.HttpHeaderInfo.HasError);

            return _artists;
        }
    }
}
