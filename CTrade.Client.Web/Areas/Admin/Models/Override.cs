using CTrade.Client.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CTrade.Client.Core;

namespace CTrade.Client.Web.Areas.Admin.Models
{
    public class ManageOverridesViewModel
    {
        public string SiteId { get; set; }
        public IList<CategoryOverrideViewModel> Categories { get; set; }

        internal ManageOverridesViewModel(string siteId, Category[] categories)
        {
            siteId.NotNullOrWhiteSpace();

            SiteId = siteId;
            if (categories != null && categories.Any())
                Categories = categories.Select(c => new CategoryOverrideViewModel(c, siteId)).ToList();
        }

        public ManageOverridesViewModel() { }
    }

    public class CategoryOverrideViewModel
    {
        internal CategoryOverrideViewModel(Category category, string siteId)
        {
            category.NotNull();

            Id = category.Id;
            Label = category.Label;
            OverridenLabel = GetOverridenLabel(category, siteId);
            if (category.SubCategories != null && category.SubCategories.Any())
                SubCategories = category.SubCategories.Select(s => new SubCategoryOverrideViewModel(s, siteId)).ToList();
        }

        public CategoryOverrideViewModel() { }

        private string GetOverridenLabel(Category category, string siteId)
        {
            string overridenLabel = null;
            if (category.SubCategories != null)
            {
                var firstSubCategory = category.SubCategories.FirstOrDefault(s => s.Sites != null &&
                    s.Sites.Any(st => !string.IsNullOrWhiteSpace(st.CategoryLabelOverride) && st.Id == siteId));
                if (firstSubCategory != null)
                    overridenLabel = firstSubCategory.Sites
                        .First(st => !string.IsNullOrWhiteSpace(st.CategoryLabelOverride) && st.Id == siteId).CategoryLabelOverride;
            }

            return overridenLabel;
        }

        public string Id { get; set; }
        public string Label { get; set; }
        public string OverridenLabel { get; set; }
        public IList<SubCategoryOverrideViewModel> SubCategories { get; set; }

        internal Category AsCategory(string siteId)
        {
            Category category = new Category();

            category.Id = Id;
            if (SubCategories != null && SubCategories.Any())
            {
                category.SubCategories = SubCategories.Select(s => s.AsSubCategory(siteId)).ToArray();
                if (!string.IsNullOrWhiteSpace(OverridenLabel))
                    AddOverridenLabel(category, siteId);
            }

            return category;
        }

        private void AddOverridenLabel(Category category, string siteId)
        {
            var site = category.SubCategories.First(s => s.Sites != null && s.Sites.Any(st => st.Id == siteId))
                .Sites.First(st => st.Id == siteId);

            site.CategoryLabelOverride = OverridenLabel;
        }
    }

    public class SubCategoryOverrideViewModel
    {
        internal SubCategoryOverrideViewModel(SubCategory subCategory, string siteId)
        {
            subCategory.NotNull();

            Id = subCategory.Id;
            Label = subCategory.Label;
            OverridenLabel = GetOverridenLabel(subCategory, siteId);
        }

        public SubCategoryOverrideViewModel() { }

        private string GetOverridenLabel(SubCategory subCategory, string siteId)
        {
            string overridenLabel = null;
            if (subCategory.Sites != null)
            {
                var site = subCategory.Sites.FirstOrDefault(st => 
                    !string.IsNullOrWhiteSpace(st.SubCategoryLabelOverride) && st.Id == siteId);
                if (site != null)
                    overridenLabel = site.SubCategoryLabelOverride;
            }

            return overridenLabel;
        }

        public string Id { get; set; }
        public string Label { get; set; }
        public string OverridenLabel { get; set; }

        internal SubCategory AsSubCategory(string siteId)
        {
            SubCategory subCategory = new SubCategory();

            subCategory.Id = Id;
            subCategory.Sites = new Site[] { new Site { Id = siteId, SubCategoryLabelOverride = OverridenLabel } };

            return subCategory;
        }
    }
}