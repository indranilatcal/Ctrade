using MyCouch.Cloudant.Responses;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CTrade.Client.DataAccess.Responses
{
    public class SearchResponse : IDbResponse
    {
        private readonly SearchIndexResponse _searchIndexResponse;
        internal SearchResponse(SearchIndexResponse searchIndexResponse)
        {
            _searchIndexResponse = searchIndexResponse;
        }

        internal SearchResponse(string error,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : this(new SearchIndexResponse { StatusCode = statusCode, Error = error }) { }

        public string Bookmark { get { return _searchIndexResponse.Bookmark; } }
        public bool IsEmpty { get { return _searchIndexResponse.IsEmpty; } }
        public long TotalRows { get { return _searchIndexResponse.TotalRows; } }
        public long RowCount { get { return _searchIndexResponse.RowCount; } }
        public SearchResponse.Row[] Rows
        {
            get
            {
                return (_searchIndexResponse.RowCount > 0) ? _searchIndexResponse.Rows
                    .Select(r => new Row(r)).ToArray() : new SearchResponse.Row[] { };
            }
        }
        public bool IsGroupsEmpty { get { return _searchIndexResponse.IsGroupsEmpty; } }
        public long GroupCount { get { return _searchIndexResponse.GroupCount; } }
        public SearchResponse.Group[] Groups
        {
            get
            {
                return (_searchIndexResponse.GroupCount > 0) ? _searchIndexResponse.Groups
                    .Select(g => new Group(g)).ToArray() : new SearchResponse.Group[] { };
            }
        }

        public JObject Counts
        {
            get
            {
                return (!string.IsNullOrWhiteSpace(_searchIndexResponse.Counts)) ?
                    JObject.Parse(_searchIndexResponse.Counts) : null;
            }
        }
        public JObject Ranges
        {
            get
            {
                return (!string.IsNullOrWhiteSpace(_searchIndexResponse.Ranges)) ?
                    JObject.Parse(_searchIndexResponse.Ranges) : null;
            }
        }

        public class Row
        {
            private readonly SearchIndexResponse.Row _row;

            internal Row(SearchIndexResponse.Row r)
            {
                _row = r;
                IncludedDoc = (!string.IsNullOrWhiteSpace(r.IncludedDoc)) ? JObject.Parse(r.IncludedDoc) : null;
            }
            public string Id { get { return _row.Id; } }
            public object[] Order { get { return _row.Order; } }
            public Dictionary<string, object> Fields { get { return _row.Fields; } }
            public JObject IncludedDoc { get; set; }
            public double[] OrderAsDoubles { get { return _row.GetOrderAsDoubles(); } }
        }

        public class Group
        {
            private readonly SearchIndexResponse.Group _group;
            internal Group(SearchIndexResponse.Group g)
            {
                _group = g;
            }
            public object By { get { return _group.By; } }
            public bool IsEmpty { get { return _group.IsEmpty; } }
            public long TotalRows { get { return _group.TotalRows; } }
            public long RowCount { get { return _group.RowCount; } }
            public Row[] Rows
            {
                get
                {
                    return (_group.RowCount > 0) ? _group.Rows
                        .Select(r => new Row(r)).ToArray() : new SearchResponse.Row[] { };
                }
            }
        }

        public IHttpHeaderInfo HttpHeaderInfo
        {
            get { return new HttpHeaderInfo(_searchIndexResponse); }
        }
    }
}
