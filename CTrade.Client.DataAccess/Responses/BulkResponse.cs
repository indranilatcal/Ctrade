using System.Linq;


namespace CTrade.Client.DataAccess.Responses
{
    public class BulkResponse : IDbResponse
    {
        private readonly MyCouch.Responses.BulkResponse _bulkResponse;
        internal BulkResponse(MyCouch.Responses.BulkResponse bulkResponse)
        {
            _bulkResponse = bulkResponse;
        }

        public bool IsEmpty { get { return _bulkResponse.IsEmpty; } }
        public Row[] Rows
        {
            get
            {
                return (!_bulkResponse.IsEmpty) ? _bulkResponse.Rows
                    .Select(r => new Row(r)).ToArray() : new Row[] { };
            }
        }

        public class Row
        {
            private readonly MyCouch.Responses.BulkResponse.Row _row;

            internal Row(MyCouch.Responses.BulkResponse.Row row)
            {
                _row = row;
            }

            public string Id { get { return _row.Id; } }
            public string Revision { get { return _row.Rev; } }
            public string Error { get { return _row.Error; } }
            public string Reason { get { return _row.Reason; } }
            public bool Succeeded { get { return _row.Succeeded; } }
        }

        public IHttpHeaderInfo HttpHeaderInfo
        {
            get { return new HttpHeaderInfo(_bulkResponse); }
        }
    }
}
