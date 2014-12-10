using CTrade.Client.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTrade.Client.Web.Areas.Admin.Models
{
    public class CategoryViewModel
    {

        internal CategoryViewModel(string siteId, Category c)
        {
            Label = c.Label;
            Id = c.Id;
            SubCategories = GetSubCategories(c.SubCategories, siteId);
            SetLabelOverride(siteId);
        }

        private void SetLabelOverride(string siteId)
        {
            var subCategoryWithSite = SubCategories.FirstOrDefault(sc => sc.Included);
            if (subCategoryWithSite != null)
            {
                var site = subCategoryWithSite.SubCategory.Sites.First(st => st.Id == siteId);
                if (!string.IsNullOrWhiteSpace(site.CategoryLabelOverride))
                    Label = site.CategoryLabelOverride;
            }
        }

        private IList<SubCategoryViewModel> GetSubCategories(SubCategory[] subCategories, string siteId)
        {
            if (subCategories != null && subCategories.Any())
                return subCategories.Select(s => new SubCategoryViewModel(siteId, s)).ToList();
            else
                return new List<SubCategoryViewModel>();
        }
        public string Id { get; set; }
        public string Label { get; set; }
        public IList<SubCategoryViewModel> SubCategories { get; set; }
    }

    public class SubCategoryViewModel
    {
        private readonly SubCategory _subCategory;
        public SubCategoryViewModel(string siteId, SubCategory s)
        {
            _subCategory = s;
            Label = s.Label;
            Included = s.Sites != null && s.Sites.Any(st => st.Id == siteId);
            SetLabelOverride(siteId);
        }

        private void SetLabelOverride(string siteId)
        {
            if (Included)
            {
                var site = _subCategory.Sites.First(st => st.Id == siteId);
                if (!string.IsNullOrWhiteSpace(site.SubCategoryLabelOverride))
                    Label = site.SubCategoryLabelOverride;
            }
        }
        public string Id { get { return _subCategory.Id; } }
        public string Label { get; set; }
        public bool Included { get; set; }
        internal SubCategory SubCategory { get { return _subCategory; } }
    }
}