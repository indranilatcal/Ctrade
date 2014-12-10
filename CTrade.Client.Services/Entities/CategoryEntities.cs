
using Newtonsoft.Json.Linq;
using System;
using CTrade.Client.Core;
using System.Linq;

namespace CTrade.Client.Services.Entities
{
    public class Category : EntityBase
    {
        public string Label { get; set; }
        public SubCategory[] SubCategories { get; set; }
    }

    public class SubCategory
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public Site[] Sites { get; set; }
    }

    public class Site
    {
        public string Id { get; set; }
        public string CategoryLabelOverride { get; set; }
        public string SubCategoryLabelOverride { get; set; }
    }

    internal static class CategoryExtensions
    {

        internal static Category AsCategory(this JObject doc)
        {
            doc.NotNull();

            dynamic dynamicDoc = doc;
            Category category = new Category
            {
                Id = dynamicDoc._id,
                Label = dynamicDoc.label,
                DocType = dynamicDoc.docType
            };

            JToken rev;
            if (doc.TryGetValue("_rev", out rev))
                category.Rev = rev.Value<string>();

            JToken subCategories;
            if (doc.TryGetValue("subCategories", out subCategories))
                category.SubCategories = subCategories.Values<JObject>().Select(s => s.AsSubCategory()).ToArray();

            return category;
        }

        internal static JObject AsJObject(this Category category)
        {
            category.NotNull();
            category.Id.NotNullOrWhiteSpace();

            dynamic doc = new JObject();
            doc._id = category.Id;
            if (!string.IsNullOrWhiteSpace(category.Rev))
                doc._rev = category.Rev;
            doc.docType = DocType.Category;
            doc.label = category.Label;
            if (category.SubCategories != null && category.SubCategories.Any())
                doc.subCategories = new JArray(category.SubCategories.Select(s => s.AsJObject()).ToArray());

            return doc;
        }

        internal static SubCategory AsSubCategory(this JObject doc)
        {
            doc.NotNull();

            dynamic dynamicDoc = doc;
            SubCategory subCategory = new SubCategory
            {
                Id = dynamicDoc._id,
                Label = dynamicDoc.label
            };

            JToken sites;
            if (doc.TryGetValue("sites", out sites))
                subCategory.Sites = sites.Values<JObject>().Select(s => s.AsSite()).ToArray();

            return subCategory;
        }

        internal static JObject AsJObject(this SubCategory subCategory)
        {
            subCategory.NotNull();
            subCategory.Id.NotNullOrWhiteSpace();

            dynamic doc = new JObject();
            doc._id = subCategory.Id;
            doc.label = subCategory.Label;

            if (subCategory.Sites != null && subCategory.Sites.Any())
                doc.sites = new JArray(subCategory.Sites.Select(s => s.AsJObject()).ToArray());

            return doc;
        }

        internal static Site AsSite(this JObject doc)
        {
            doc.NotNull();

            dynamic dynamicDoc = doc;
            Site site = new Site { Id = dynamicDoc._id };

            JToken categoryLabelOverride;
            if (doc.TryGetValue("categoryLabelOverride", out categoryLabelOverride))
                site.CategoryLabelOverride = categoryLabelOverride.Value<string>();

            JToken subCategoryLabelOverride;
            if (doc.TryGetValue("subCategoryLabelOverride", out subCategoryLabelOverride))
                site.SubCategoryLabelOverride = subCategoryLabelOverride.Value<string>();

            return site;
        }

        internal static JObject AsJObject(this Site site)
        {
            site.NotNull();
            site.Id.NotNullOrWhiteSpace();

            dynamic doc = new JObject();
            doc._id = site.Id;

            if (!string.IsNullOrWhiteSpace(site.CategoryLabelOverride))
                doc.categoryLabelOverride = site.CategoryLabelOverride;
            if (!string.IsNullOrWhiteSpace(site.SubCategoryLabelOverride))
                doc.subCategoryLabelOverride = site.SubCategoryLabelOverride;

            return doc;
        }
    }
}
