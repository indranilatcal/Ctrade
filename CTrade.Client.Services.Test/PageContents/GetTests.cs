using CTrade.Client.Services.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTrade.Client.Services.Test.PageContents
{
    [TestClass]
    public class GetTests : PageContentTestBase
    {
        [TestMethod]
        public void SiteIdShouldBeMandatory()
        {
            var response = PageContentService.GetAsync(null, null).Result;

            response.HasError(ErrorMessage.SiteIdIsMandatory);
        }

        [TestMethod]
        public void PageIdShouldBeMandatory()
        {
            var response = PageContentService.GetAsync(Constants.Ignored, null).Result;

            response.HasError(ErrorMessage.PageIdIsMandatory);
        }

        [TestMethod]
        public void ExistingPageShouldBeRetrieved()
        {
            const string siteId = "www.example.com";
            const string pageTitle = "page title";
            const string pageContent = "<ul><li>$100 rebate</li><li>Many more&nbsp;</li></ul>Thanks<br>";
            var request = new PageContent { SiteId = siteId, Title = pageTitle, Content = pageContent };
            var createResponse = PageContentService.CreateAsync(request).Result;
            createResponse.HasNoError();

            try
            {
                var getResponse = PageContentService.GetAsync(siteId, createResponse.Id).Result;

                getResponse.HasNoError();
                Assert.IsFalse(getResponse.IsEmpty);
                var page = getResponse.PageContent;
                Assert.AreEqual(siteId, page.SiteId);
                Assert.AreEqual(pageTitle, page.Title);
                Assert.AreEqual(pageContent, page.Content);
            }
            finally
            {
                Delete(createResponse.Id, createResponse.Rev);
            }
        }
    }
}
