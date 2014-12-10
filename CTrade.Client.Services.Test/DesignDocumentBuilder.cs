using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrade.Client.Services.Test
{
    static class DesignDocumentBuilder
    {
        private const string _designDocTemplate
            = "{{" +
                    "\"_id\": \"_design/ddoc\"," +
                    "\"language\": \"javascript\"," +
                    "\"views\": {{}}," +
                    "\"indexes\": {{" +
                        "{0}," +
                        "{1}," +
                        "{2}," +
                        "{3}" +
                    "}}" +
                "}}";

        internal static string CreateIndexContent()
        {
            return string.Format(_designDocTemplate,
                CreateFaqIndex(),
                CreateCategoryIndex(),
                CreatePageContentIndex(),
                CreatePromotionIndex()
                );
        }

        private static string CreateFaqIndex()
        {
            return "\"questions\": {" +
                            "\"index\": \"function(doc){\\n" +
                            "  index('default', doc._id);\\n" +
                            CreateDocTypeFragment() +
                            "  if(!doc.sites || doc.sites.length === 0){\\n return;\\n }\\n for (var i=0; i<doc.sites.length; i++) {\\n index('site', doc.sites[i], {'index': \\\"not_analyzed\\\"});\\n }\\n }\"" +
                        "}";
        }

        private static string CreateCategoryIndex()
        {
            return "\"categories\": {" +
                            "\"index\": \"function(doc){\\n" +
                            "index('default', doc._id);\\n" +
                            CreateDocTypeFragment() +
                            "if(!doc.subCategories || doc.subCategories.length === 0)\\n" +
                                "return;\\n" +
                            "var subCategory;\\n" +
                            "for (var i=0; i<doc.subCategories.length; i++) {\\n" +
                                "index('subCategory', doc.subCategories[i]._id, {'index': \\\"not_analyzed\\\"});\\n" +
                                "if(!doc.subCategories[i].sites || doc.subCategories[i].sites.length === 0)\\n" +
                                    "return;\\n" +
                                "for(var j=0; j<doc.subCategories[i].sites.length; j++){\\n" +
                                    "index('site', doc.subCategories[i].sites[j]._id, {'index': \\\"not_analyzed\\\"});\\n" +
                                "}" +
                            "}" +
                        "}\"" +
                    "}";
        }

        private static string CreatePageContentIndex()
        {
            return "\"pageContents\": {" +
                            "\"index\": \"function(doc){\\n" +
                            "  index('default', doc._id);\\n" +
                            CreateDocTypeFragment() +
                            "  if (doc.siteId){\\n    index('site', doc.siteId, {\\\"store\\\": \\\"yes\\\"});\\n  }\\n" +
                            "  if (doc.title){\\n    index('title', doc.title, {\\\"store\\\": \\\"yes\\\"});\\n  }\\n" +
                            "}\"" +
                        "}";
        }

        private static string CreatePromotionIndex()
        {
            return "\"promotions\": {" +
                            "\"index\": \"function(doc){\\n" +
                            "  index('default', doc._id);\\n" +
                            CreateDocTypeFragment() +
                            "  if (doc.activate){\\n    index('activate', doc.activate, {\\\"store\\\": \\\"yes\\\"});\\n  }\\n" +
                            "  if (doc.startDate){\\n    index('startDate', doc.startDate, {\\\"store\\\": \\\"yes\\\"});\\n  }\\n" +
                            "  if (doc.endDate){\\n    index('endDate', doc.endDate, {\\\"store\\\": \\\"yes\\\"});\\n  }\\n" +
                            "}\"" +
                        "}";
        }

        private static string CreateDocTypeFragment()
        {
            return "  if (doc.docType){\\n    index('docType', doc.docType, {\\\"store\\\": \\\"yes\\\"});\\n  }\\n";
        }
    }
}
