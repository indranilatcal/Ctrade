using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CTrade.Client.Services.Test.Categories
{
    [TestClass]
    public class GetTests : CategoriesTestBase
    {
        [TestMethod]
        public void TestGetCategoriesRetrievesAllCategoriesIrrespectiveOfSites()
        {
            var response = CategoryService.GetCategoriesAsync().Result;

            response.HasNoError();
            Assert.IsFalse(response.IsEmpty);
        }

        [TestMethod]
        public void TestGetCategoriesCanLimitResults()
        {
            var response = CategoryService.GetCategoriesAsync(1).Result;

            Assert.AreEqual(1, response.Categories.Length);
            Assert.AreEqual(DocType.Category, response.Categories.First().DocType);
        }

        [TestMethod]
        public void TestGetCategoriesForSiteReturnsMatchingCategoriesWithSiteIncludedInSubCategories()
        {
            var siteId = "www.example.com";
            var response = CategoryService.GetCategoriesForSiteAsync(siteId).Result;

            Assert.IsFalse(response.IsEmpty);
            Assert.IsTrue(response.Categories.All(c => c.SubCategories.All(s => s.Sites.Any(st => st.Id == siteId))));
        }
    }
}
