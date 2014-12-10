using MyCouch.Responses;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CTrade.Client.DataAccess.Responses
{
    public class QueryResponse : IDbResponse
    {
        private readonly ViewQueryResponse _viewQueryResponse;

        internal QueryResponse(ViewQueryResponse viewQueryResponse)
        {
            _viewQueryResponse = viewQueryResponse;
        }
        public long TotalRows { get { return _viewQueryResponse.TotalRows; } }
        public long RowCount { get { return _viewQueryResponse.RowCount; } }
        public string UpdateSeq { get { return _viewQueryResponse.UpdateSeq; } }
        public long OffSet { get { return _viewQueryResponse.OffSet; } }
        public QueryResponse.Row[] Rows
        {
            get
            {
                return (_viewQueryResponse.RowCount > 0) ? _viewQueryResponse.Rows
                    .Select(r => new Row(r)).ToArray() : new Row[] { };
            }
        }
        public bool IsEmpty { get { return _viewQueryResponse.IsEmpty; } }
        public class Row
        {
            private readonly ViewQueryResponse.Row _row;
            internal Row(ViewQueryResponse.Row r)
            {
                _row = r;
                IncludedDoc = (!string.IsNullOrWhiteSpace(r.IncludedDoc)) ? JObject.Parse(r.IncludedDoc) : null;
                Value = (!string.IsNullOrWhiteSpace(r.Value)) ? JToken.Parse(r.Value) : null;
            }
            public string Id { get { return _row.Id; } }
            public object Key { get { return _row.Key; } }
            public JToken Value { get; set; }
            public JObject IncludedDoc { get; set; }
            public string KeyAsString()
            {
                return _row.KeyAsString();
            }
            public Guid? KeyAsGuid()
            {
                return _row.KeyAsGuid();
            }
            public DateTime? KeyAsDateTime()
            {
                return _row.KeyAsDateTime();
            }
            public int? KeyAsInt()
            {
                return _row.KeyAsInt();
            }
            public long? KeyAsLong()
            {
                return _row.KeyAsLong();
            }
            public double? KeyAsDouble()
            {
                return _row.KeyAsDouble();
            }
        }

        public IHttpHeaderInfo HttpHeaderInfo
        {
            get { return new HttpHeaderInfo(_viewQueryResponse); }
        }
    }
}
