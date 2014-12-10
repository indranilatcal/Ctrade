using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrade.Client.Services.Test.Categories
{
    [TestClass]
    public class UpdateTests : CategoriesTestBase
    {
        [TestMethod]
        public void SiteIdShouldNotBeEmpty()
        {
            var response = CategoryService.UpdateSubCategoriesAsync(null, null).Result;

            response.HasError(ErrorMessage.SiteIdIsMandatory);
        }

        [TestMethod]
        public void SitesShouldBeRemovedFromSubCategoriesWhenNotSupplied()
        {
            const string siteId = "www.myfaq.com";
            var updateResponse = CategoryService.UpdateSubCategoriesAsync(siteId, new string[] { }).Result;
            var getResponse = CategoryService.GetCategoriesAsync().Result;

            updateResponse.HasNoError();
            Assert.IsTrue(getResponse.Categories.All(c => c.SubCategories.All(s => s.Sites == null || !s.Sites.Any(st => st.Id == siteId))));
        }

        [TestMethod]
        public void SitesShouldBeAddedToSubCategoriesWhenSupplied()
        {
            const string siteId = "www.example.com";
            const string subCategoryId = "100102";
            var updateResponse = CategoryService.UpdateSubCategoriesAsync(siteId, new string[] { subCategoryId }).Result;
            var getResponse = CategoryService.GetCategoriesAsync().Result;

            updateResponse.HasNoError();
            var subCategory = getResponse.Categories.SelectMany(c => c.SubCategories).First(s => s.Id == subCategoryId);
            Assert.IsTrue(subCategory.Sites.SingleOrDefault(st => st.Id == siteId) != null);
        }
    }
}
