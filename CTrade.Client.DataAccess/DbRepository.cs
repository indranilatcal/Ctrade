using CTrade.Client.DataAccess.Requests;
using CTrade.Client.DataAccess.Responses;
using MyCouch;
using MyCouch.Cloudant;
using MyCouch.Cloudant.Requests;
using MyCouch.Requests;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading.Tasks;
using CTrade.Client.Core;

namespace CTrade.Client.DataAccess
{
    public class DbRepository : IDbRepository
    {

        static class ErrorMessage
        {
            internal const string DeleteRequiresIdAndRev = "Id and Revision are mandatory for Delete operation";
            internal const string UpdateRequiresIdAndRev = "Id and Revision are mandatory for Update operation";
            internal const string DataCannotBeEmpty = "Data cannot be empty";
            internal const string GetRequiresId = "Id is mandatory for Get Operation";
            internal const string InternalServerError = "Internal server error";
        }

        private const string _serverUriFormat = "https://@{0}.cloudant.com/";

        private readonly string _userName;
        private readonly string _dbName;
        private readonly string _key;
        private readonly string _password;

        public DbRepository(string userName, string dbName, string key, string password)
        {
            userName.NotNullOrWhiteSpace();
            dbName.NotNullOrWhiteSpace();
            key.NotNullOrWhiteSpace();
            password.NotNullOrWhiteSpace();

            _userName = userName;
            _dbName = dbName;
            _key = key;
            _password = password;
        }

        public async Task<IDbDocHeaderResponse> CreateAsync(JObject data, string id = null)
        {
            if (!string.IsNullOrWhiteSpace(id))
                (data as dynamic)._id = id;

            using (var client = InitializeClient())
            {
                if (data.IsBlank() || !data.IsEmpty())
                {
                    if (data.IsBlank())
                        data.RemoveAll();

                    var response = await client.Documents.PostAsync(data.ToString());
                    return new DbDocHeaderResponse(response);
                }
                else
                    return new DbDocHeaderResponse(ErrorMessage.DataCannotBeEmpty,
                        HttpStatusCode.BadRequest);
            }
        }

        public async Task<IDbDocResponse> GetAsync(string id, string revision = null)
        {
            if (string.IsNullOrWhiteSpace(id))
                return new DbDocResponse(ErrorMessage.GetRequiresId,
                    HttpStatusCode.BadRequest);

            using (var client = InitializeClient())
            {
                var response = await client.Documents.GetAsync(id, revision);
                return new DbDocResponse(response);
            }
        }

        public async Task<IDbDocHeaderResponse> UpdateAsync(string id, string revision,JObject data)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(revision))
                return new DbDocHeaderResponse(ErrorMessage.UpdateRequiresIdAndRev,
                    HttpStatusCode.BadRequest);

            if (data.IsBlank() || data.IsEmpty())
                return new DbDocHeaderResponse(ErrorMessage.DataCannotBeEmpty,
                    HttpStatusCode.BadRequest);

            using (var client = InitializeClient())
            {
                var response = await client.Documents.PutAsync(id, revision, data.ToString());
                return new DbDocHeaderResponse(response);
            }
        }

        public async Task<IDbDocHeaderResponse> DeleteAsync(string id, string revision)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(revision))
                return new DbDocHeaderResponse(ErrorMessage.DeleteRequiresIdAndRev,
                    HttpStatusCode.BadRequest);

            using (var client = InitializeClient())
            {
                var response = await client.Documents.DeleteAsync(id, revision);
                return new DbDocHeaderResponse(response);
            }
        }

        public async Task<SearchResponse> SearchAsync(SearchIndexRequest searchIndexRequest)
        {
            using (var client = InitializeClient())
            {
                var response = await client.Searches.SearchAsync(searchIndexRequest);
                return new SearchResponse(response);
            }
        }

        public async Task<QueryResponse> QueryAsync(QueryViewRequest queryRequest)
        {
            using (var client = InitializeClient())
            {
                var response = await client.Views.QueryAsync(queryRequest);
                return new QueryResponse(response);
            }
        }

        public async Task<BulkResponse> BulkAsync(BulkRequestWrapper bulkRequestWrapper)
        {
            using (var client = InitializeClient())
            {
                var response = await client.Documents.BulkAsync(bulkRequestWrapper.BulkRequest);
                return new BulkResponse(response);
            }
        }

        public async Task<ListResponse> QueryAsync(QueryListRequest queryListRequest)
        {
            using (var client = InitializeClient())
            {
                var response = await client.Views.QueryAsync(queryListRequest);
                return new ListResponse(response);
            }
        }

        #region Private Helpers
        private MyCouchCloudantClient InitializeClient()
        {
            var uri = new MyCouchUriBuilder(string.Format(_serverUriFormat, _userName))
                .SetDbName(_dbName)
                .SetBasicCredentials(_key, _password).Build();

            return new MyCouchCloudantClient(uri);
        }
        #endregion	
    }
}