using CTrade.Client.Services.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTrade.Client.Services.Test.PageContents
{
    [TestClass]
    public class EditTests : PageContentTestBase
    {

        [TestMethod]
        public void PageIdShouldBeMandatory()
        {
            var request = new PageContent();

            var response = PageContentService.EditAsync(request).Result;

            response.HasError(ErrorMessage.PageIdIsMandatory);
        }

        [TestMethod]
        public void SiteIdShouldBeMandatory()
        {
            var request = new PageContent { SiteId = string.Empty };

            var response = PageContentService.CreateAsync(request).Result;

            response.HasError(ErrorMessage.SiteIdIsMandatory);
        }

        [TestMethod]
        public void TitleShouldBeMandatory()
        {
            var request = new PageContent { SiteId = Constants.Ignored };

            var response = PageContentService.CreateAsync(request).Result;

            response.HasError(ErrorMessage.TitleIsMandatory);
        }

        [TestMethod]
        public void ContentShouldBeMandatory()
        {
            var request = new PageContent { SiteId = Constants.Ignored, Title = Constants.Ignored };

            var response = PageContentService.CreateAsync(request).Result;

            response.HasError(ErrorMessage.ContentIsMandatory);
        }
        
        [TestMethod]
        public void ShouldReportErrorWhenPageDoesNotExist()
        {
            var request = new PageContent { Id = "junk", SiteId = Constants.Ignored, Title = Constants.Ignored,
                Content = Constants.Ignored };

            var response = PageContentService.EditAsync(request).Result;

            response.HasError(ErrorMessage.PageNotFound);
        }

        [TestMethod]
        public void CanEditExistingPage()
        {
            const string siteId = "www.example.com";
            const string pageTitle = "page title";
            const string pageContent = "<ul><li>$100 rebate</li><li>Many more&nbsp;</li></ul>Thanks<br>";
            var request = new PageContent { SiteId = siteId, Title = Constants.Ignored, Content = Constants.Ignored };
            var createResponse = PageContentService.CreateAsync(request).Result;
            createResponse.HasNoError();

            string rev = createResponse.Rev;
            try
            {
                request.Title = pageTitle;
                request.Content = pageContent;
                request.Id = createResponse.Id;
                var editResponse = PageContentService.EditAsync(request).Result;
                editResponse.HasNoError();
                Assert.IsTrue(!string.IsNullOrWhiteSpace(editResponse.Id));
                Assert.IsTrue(!string.IsNullOrWhiteSpace(editResponse.Rev));
                rev = editResponse.Rev;
                var getResponse = PageContentService.GetAsync(siteId, editResponse.Id).Result;
                getResponse.HasNoError();
                var page = getResponse.PageContent;
                Assert.AreEqual(siteId, page.SiteId);
                Assert.AreEqual(pageTitle, page.Title);
                Assert.AreEqual(pageContent, page.Content);
            }
            finally
            {
                Delete(request.Id, rev);
            }
        }
    }
}
