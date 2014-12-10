using CTrade.Client.DataAccess.Requests;
using CTrade.Client.DataAccess.Responses;
using MyCouch.Cloudant.Requests;
using MyCouch.Requests;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
namespace CTrade.Client.DataAccess
{
    public interface IDbRepository
    {
        Task<BulkResponse> BulkAsync(BulkRequestWrapper bulkRequestWrapper);
        Task<IDbDocHeaderResponse> CreateAsync(JObject data, string id = null);
        Task<IDbDocHeaderResponse> DeleteAsync(string id, string revision);
        Task<IDbDocResponse> GetAsync(string id, string revision = null);
        Task<QueryResponse> QueryAsync(QueryViewRequest queryRequest);
        Task<SearchResponse> SearchAsync(SearchIndexRequest searchIndexRequest);
        Task<IDbDocHeaderResponse> UpdateAsync(string id, string revision, JObject data);
        Task<ListResponse> QueryAsync(QueryListRequest queryListRequest);
    }
}
