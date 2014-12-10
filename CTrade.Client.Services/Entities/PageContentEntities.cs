
using Newtonsoft.Json.Linq;
using CTrade.Client.Core;

namespace CTrade.Client.Services.Entities
{
    public class PageContent : EntityBase
    {
        public string SiteId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    internal static class PageContentExtensions
    {
        internal static JObject AsJObject(this PageContent pageContent)
        {
            pageContent.NotNull();

            dynamic doc = new JObject();
            if(!string.IsNullOrWhiteSpace(pageContent.Id))
                doc._id = pageContent.Id;
            if (!string.IsNullOrWhiteSpace(pageContent.Rev))
                doc._rev = pageContent.Rev;
            doc.siteId = pageContent.SiteId;
            doc.title = pageContent.Title;
            doc.content = pageContent.Content;
            doc.docType = DocType.Page;

            return doc;
        }

        internal static PageContent AsPageContent(this JObject doc)
        {
            doc.NotNull();

            dynamic dynamicDoc = doc;
            PageContent pageContent = new PageContent
                {
                    Id = dynamicDoc._id,
                    SiteId = dynamicDoc.siteId,
                    Title = dynamicDoc.title,
                    Content = dynamicDoc.content,
                    DocType = dynamicDoc.docType
                };

            JToken rev;
            if (doc.TryGetValue("_rev", out rev))
                pageContent.Rev = rev.Value<string>();

            return pageContent;
        }
    }
}
