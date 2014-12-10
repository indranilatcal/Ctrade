using CTrade.Client.Services.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CTrade.Client.Services.Test.PageContents
{
    [TestClass]
    public class GetForSiteTests : PageContentTestBase
    {
        [TestMethod]
        public void SiteIdShouldBeMandatory()
        {
            var response = PageContentService.GetForSiteAsync(null).Result;

            response.HasError(ErrorMessage.SiteIdIsMandatory);
        }

        [TestMethod]
        public void PagesForSiteShouldBeRetrieved()
        {
            const string siteId1 = "www.example.com";
            const string siteId2 = "www.faq.com";
            CreatePage(siteId1, Constants.Ignored, Constants.Ignored);
            CreatePage(siteId1, Constants.Ignored, Constants.Ignored);
            CreatePage(siteId2, Constants.Ignored, Constants.Ignored);

            var response = PageContentService.GetForSiteAsync(siteId1).Result;

            response.HasNoError();
            Assert.IsFalse(response.IsEmpty);
            Assert.AreEqual(2, response.PageContents.Length);
            Assert.IsTrue(response.PageContents.All(p => p.SiteId == siteId1));
        }

        [TestMethod]
        public void NonExistentPagesShouldNotCauseError()
        {
            const string siteId = "junk";

            var response = PageContentService.GetForSiteAsync(siteId).Result;

            response.HasNoError();
        }

        [TestCleanup]
        public void Clear()
        {
            TestRuntime.ResetRepository(Constants.MasterConnectionKey);
            TestRuntime.CreateIndex(Constants.MasterConnectionKey, DesignDocumentBuilder.CreateIndexContent());
        }

        private void CreatePage(string siteId, string title, string content)
        {
            var response = PageContentService.CreateAsync(
                new PageContent { SiteId = siteId, Title = title, Content = content }).Result;
            response.HasNoError();
        }
    }
}
