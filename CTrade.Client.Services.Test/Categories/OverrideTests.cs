using CTrade.Client.Services.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrade.Client.Services.Test.Categories
{
    [TestClass]
    public class OverrideTests : CategoriesTestBase
    {
        [TestMethod]
        public void SiteIdShouldNotBeEmpty()
        {
            var response = CategoryService.OverrideAsync(null, null).Result;

            response.HasError(ErrorMessage.SiteIdIsMandatory);
        }

        [TestMethod]
        public void ItemsToOverrideShouldNotBeEmpty()
        {
            var response = CategoryService.OverrideAsync("ignored", new Category[]{}).Result;

            response.HasError(ErrorMessage.ItemsToOverrideShouldNotBeEmpty);
        }

        [TestMethod]
        public void ShouldOverrideSubCategoryLabelsOnlyWhenOverrideTextSpecified()
        {
            const string siteId = "www.example.com";
            const string subCategoryId = "100104";
            const string subCatOverrideLabel = "OverridenSubCatValue";
            var categoriesForSiteResponse = CategoryService.GetCategoriesForSiteAsync(siteId).Result;
            var site = categoriesForSiteResponse.Categories.SelectMany(c => c.SubCategories)
                .First(s => s.Id == subCategoryId).Sites.First(st => st.Id == siteId);
            site.SubCategoryLabelOverride = subCatOverrideLabel;

            var overrideResponse = CategoryService.OverrideAsync(siteId, categoriesForSiteResponse.Categories).Result;
            categoriesForSiteResponse = CategoryService.GetCategoriesForSiteAsync(siteId).Result;
            site = categoriesForSiteResponse.Categories.SelectMany(c => c.SubCategories)
                .First(s => s.Id == subCategoryId).Sites.First(st => st.Id == siteId);

            overrideResponse.HasNoError();
            Assert.AreEqual(subCatOverrideLabel, site.SubCategoryLabelOverride);
            Assert.AreNotEqual(subCatOverrideLabel, site.CategoryLabelOverride);
        }

        [TestMethod]
        public void SubCategoryLabelOverrideCanBeSetToEmpty()
        {
            const string siteId = "www.example.com";
            const string subCategoryId = "100104";
            var categoriesForSiteResponse = CategoryService.GetCategoriesForSiteAsync(siteId).Result;
            var site = categoriesForSiteResponse.Categories.SelectMany(c => c.SubCategories)
                .First(s => s.Id == subCategoryId).Sites.First(st => st.Id == siteId);
            site.SubCategoryLabelOverride = null;

            var overrideResponse = CategoryService.OverrideAsync(siteId, categoriesForSiteResponse.Categories).Result;
            categoriesForSiteResponse = CategoryService.GetCategoriesForSiteAsync(siteId).Result;
            site = categoriesForSiteResponse.Categories.SelectMany(c => c.SubCategories)
                .First(s => s.Id == subCategoryId).Sites.First(st => st.Id == siteId);

            overrideResponse.HasNoError();
            Assert.IsNull(site.SubCategoryLabelOverride);
        }

        [TestMethod]
        public void ShouldOverrideCategoryLabelsOnlyWhenOverrideTextSpecified()
        {
            const string siteId = "www.example.com";
            const string subCategoryId = "100104";
            const string catOverrideLabel = "OverridenCatValue";
            var categoriesForSiteResponse = CategoryService.GetCategoriesForSiteAsync(siteId).Result;
            var site = categoriesForSiteResponse.Categories.SelectMany(c => c.SubCategories)
                .First(s => s.Id == subCategoryId).Sites.First(st => st.Id == siteId);
            site.CategoryLabelOverride = catOverrideLabel;

            var overrideResponse = CategoryService.OverrideAsync(siteId, categoriesForSiteResponse.Categories).Result;
            categoriesForSiteResponse = CategoryService.GetCategoriesForSiteAsync(siteId).Result;
            site = categoriesForSiteResponse.Categories.SelectMany(c => c.SubCategories)
                .First(s => s.Id == subCategoryId).Sites.First(st => st.Id == siteId);
            overrideResponse.HasNoError();
            Assert.AreEqual(catOverrideLabel, site.CategoryLabelOverride);
            Assert.AreNotEqual(catOverrideLabel, site.SubCategoryLabelOverride);
        }

        [TestMethod]
        public void CategoryLabelOverrideCanBeSetToEmpty()
        {
            const string siteId = "www.example.com";
            const string subCategoryId = "100104";
            var categoriesForSiteResponse = CategoryService.GetCategoriesForSiteAsync(siteId).Result;
            var site = categoriesForSiteResponse.Categories.SelectMany(c => c.SubCategories)
                .First(s => s.Id == subCategoryId).Sites.First(st => st.Id == siteId);
            site.CategoryLabelOverride = null;

            var overrideResponse = CategoryService.OverrideAsync(siteId, categoriesForSiteResponse.Categories).Result;
            categoriesForSiteResponse = CategoryService.GetCategoriesForSiteAsync(siteId).Result;
            site = categoriesForSiteResponse.Categories.SelectMany(c => c.SubCategories)
                .First(s => s.Id == subCategoryId).Sites.First(st => st.Id == siteId);

            overrideResponse.HasNoError();
            Assert.IsNull(site.CategoryLabelOverride);
        }

        [TestMethod]
        public void CanSetMultipleOverridesForSite()
        {
            const string siteId = "www.example.com";
            const string subCategoryId1 = "100104";
            const string subCategoryId2 = "100501";
            const string catOverrideLabel1 = "100104CatValue";
            const string subCatOverrideLabel1 = "100104OverridenSubCatValue";
            const string catOverrideLabel2 = "100501CatValue";
            var categoriesForSiteResponse = CategoryService.GetCategoriesForSiteAsync(siteId).Result;
            var site1 = categoriesForSiteResponse.Categories.SelectMany(c => c.SubCategories)
                .First(s => s.Id == subCategoryId1).Sites.First(st => st.Id == siteId);
            site1.CategoryLabelOverride = catOverrideLabel1;
            site1.SubCategoryLabelOverride = subCatOverrideLabel1;
            var site2 = categoriesForSiteResponse.Categories.SelectMany(c => c.SubCategories)
                .First(s => s.Id == subCategoryId2).Sites.First(st => st.Id == siteId);
            site2.CategoryLabelOverride = catOverrideLabel2;
            site2.SubCategoryLabelOverride = null;

            var overrideResponse = CategoryService.OverrideAsync(siteId, categoriesForSiteResponse.Categories).Result;
            categoriesForSiteResponse = CategoryService.GetCategoriesForSiteAsync(siteId).Result;
            site1 = categoriesForSiteResponse.Categories.SelectMany(c => c.SubCategories)
                .First(s => s.Id == subCategoryId1).Sites.First(st => st.Id == siteId);
            site2 = categoriesForSiteResponse.Categories.SelectMany(c => c.SubCategories)
                .First(s => s.Id == subCategoryId2).Sites.First(st => st.Id == siteId);
            
            Assert.AreEqual(catOverrideLabel1, site1.CategoryLabelOverride);
            Assert.AreEqual(subCatOverrideLabel1, site1.SubCategoryLabelOverride);
            Assert.AreEqual(catOverrideLabel2, site2.CategoryLabelOverride);
            Assert.IsNull(site2.SubCategoryLabelOverride);
        }
    }
}
