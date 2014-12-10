using CTrade.Client.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTrade.Client.Web.Areas.Admin.Models
{
    public class ManageCategoriesViewModel
    {
        public string SiteId { get; set; }

        internal ManageCategoriesViewModel(string siteId, Category[] categories)
        {
            SiteId = siteId;
            Categories = GetCategories(categories);
        }

        private IList<CategoryViewModel> GetCategories(Category[] categories)
        {
            if (categories != null && categories.Any())
                return categories.Select(c => new CategoryViewModel(SiteId, c)).ToList();
            else
                return new List<CategoryViewModel>();
        }

        public IList<CategoryViewModel> Categories { get; set; }
    }
}