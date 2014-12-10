using MyCouch.Responses;
using Newtonsoft.Json.Linq;
using System.Net;

namespace CTrade.Client.DataAccess.Responses
{
    public interface IDbDocResponse : IDbResponse
    {
        string Id { get; }
        string Revision { get; }
        JObject Data { get; }
        string[] Conflicts { get; }
        bool HasConflicts { get; }
    }

    public class DbDocResponse : IDbDocResponse
    {
        private readonly DocumentResponse _docResponse;
        private JObject _data;
        internal DbDocResponse(DocumentResponse docResponse)
        {
            _docResponse = docResponse;
            if (!docResponse.IsEmpty)
                _data = JObject.Parse(docResponse.Content);
        }

        internal DbDocResponse(string error,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : this(new DocumentResponse { StatusCode = statusCode, Error = error }) { }

        public JObject Data
        {
            get { return _data; }
        }

        public IHttpHeaderInfo HttpHeaderInfo
        {
            get { return new HttpHeaderInfo(_docResponse); }
        }

        public string Id
        {
            get { return _docResponse.Id; }
        }

        public string Revision
        {
            get { return _docResponse.Rev; }
        }

        public string[] Conflicts
        {
            get { return _docResponse.Conflicts ?? new string[] {}; }
        }

        public bool HasConflicts
        {
            get { return Conflicts.Length > 0; }
        }
    }
}
