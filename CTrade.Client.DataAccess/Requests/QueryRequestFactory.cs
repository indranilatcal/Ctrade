using MyCouch;
using MyCouch.Requests;
using CTrade.Client.Core;

namespace CTrade.Client.DataAccess.Requests
{
    public static class QueryRequestFactory
    {
        public static QueryViewRequest CreatePrimaryIndexQuery()
        {
            return new QueryViewRequest(SystemViewIdentity.AllDocs);
        }

        public static QueryViewRequest CreateIndexQuery(string designDocument, string viewName)
        {
            designDocument.NotNullOrWhiteSpace();
            viewName.NotNullOrWhiteSpace();

            return new QueryViewRequest(designDocument, viewName);
        }

        public static QueryListRequest CreateListQuery(string designDocument, string functionName, string viewName)
        {
            designDocument.NotNullOrWhiteSpace();
            functionName.NotNullOrWhiteSpace();
            viewName.NotNullOrWhiteSpace();

            return new QueryListRequest(designDocument, functionName, viewName);
        }
    }
}
