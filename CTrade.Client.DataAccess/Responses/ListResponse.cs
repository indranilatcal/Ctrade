
using MyCouch.Net;
using MyCouch.Responses;
using Newtonsoft.Json.Linq;
namespace CTrade.Client.DataAccess.Responses
{
    public class ListResponse : IDbResponse
    {
        private readonly ListQueryResponse _listQueryResponse;
        private readonly JToken _jsonContent;

        internal ListResponse(ListQueryResponse listQueryResponse)
        {
            _listQueryResponse = listQueryResponse;
            if (IsJson)
                _jsonContent = JToken.Parse(_listQueryResponse.Content);
        }

        public bool IsEmpty
        {
            get { return _listQueryResponse.IsEmpty; }
        }

        public string Etag
        {
            get { return _listQueryResponse.Etag; }
        }

        public string RawContent
        {
            get { return _listQueryResponse.Content; }
        }

        public JToken JsonContent
        {
            get { return _jsonContent; }
        }

        public IHttpHeaderInfo HttpHeaderInfo
        {
            get { return new HttpHeaderInfo(_listQueryResponse); }
        }

        public bool IsHtml
        {
            get { return !IsEmpty && _listQueryResponse.ContentType.Contains(HttpContentTypes.Html); }
        }

        public bool IsJson
        {
            get { return !IsEmpty && _listQueryResponse.ContentType.Contains(HttpContentTypes.Json); }
        }
    }
}
