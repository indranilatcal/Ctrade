
namespace CTrade.Client.Services
{
    internal static class Index
    {
        internal const string Questions = "questions";
        internal const string Categories = "categories";
        internal const string DesignDoc = "ddoc";
        internal const string DocTypeExpressionFormat = "docType:{0}";
        internal const string MatchSiteExpressionFormat = "site:{0}";
        internal const string PageContents = "pageContents";
        internal const string PageContentSearchExpressionFormat = @"{0} AND site:{1}";
    }

    internal static class DocType
    {
        internal const string Faq = "faq";
        internal const string Page = "page";
        internal const string Category = "category";
        internal const string Promotion = "promotion";
    }
}
