using MyCouch;
using MyCouch.Requests;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace CTrade.Client.DataAccess.Requests
{
    public static class BulkRequestFactory
    {
        public static BulkRequestWrapper Create()
        {
            return new BulkRequestWrapper(new BulkRequest());
        }
    }

    public class BulkRequestWrapper
    {
        private readonly BulkRequest _bulkRequest;
        internal BulkRequestWrapper(BulkRequest bulkRequest)
        {
            _bulkRequest = bulkRequest;
        }

        internal BulkRequest BulkRequest
        {
            get { return _bulkRequest; }
        }

        public JObject[] GetDocuments()
        {
            return _bulkRequest.GetDocuments().Select(d => JObject.Parse(d)).ToArray();
        }

        public BulkRequestWrapper Include(params JObject[] docs)
        {
            if (docs != null && docs.Any())
                _bulkRequest.Include(docs.Select(d => d.ToString()).ToArray());

            return this;
        }

        public BulkRequestWrapper Delete(string id, string rev)
        {
            _bulkRequest.Delete(id, rev);

            return this;
        }

        public BulkRequestWrapper Delete(params DocumentHeader[] docHeaders)
        {
            _bulkRequest.Delete(docHeaders);

            return this;
        }

        public bool IsEmpty { get { return _bulkRequest.IsEmpty; } }
    }
}
