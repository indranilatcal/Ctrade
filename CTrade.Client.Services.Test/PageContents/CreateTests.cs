using CTrade.Client.Services.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTrade.Client.Services.Test.PageContents
{
    [TestClass]
    public class CreateTests : PageContentTestBase
    {

        [TestMethod]
        public void PageIdCannotBeSpecifiedForCreate()
        {
            var request = new PageContent { Id = "some Value" };

            var response = PageContentService.CreateAsync(request).Result;

            response.HasError(ErrorMessage.PageIdCannotBeSpecifiedForCreate);
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
        public void CanCreateNewPageContent()
        {
            const string siteId = "www.example.com";
            const string pageTitle = "page title";
            const string pageContent = @"<ul><li>$100 rebate</li><li>Many <i>more</i>&nbsp;</li></ul>Visit here:&nbsp;<a href=""http://www.google.com"" target=""_blank"" rel=""nofollow"">http://www.google.com/</a> <br><img alt="" src=""http://i1008.photobucket.com/albums/af201/visuallightbox/Butterfly/9-1.jpg""><br><h1>heading 1</h1>Thanks<br>";
            var request = new PageContent { SiteId = siteId, Title = pageTitle, Content = pageContent };

            var createResponse = PageContentService.CreateAsync(request).Result;
            var getResponse = PageContentService.GetAsync(siteId, createResponse.Id).Result;

            createResponse.HasNoError();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(createResponse.Id));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(createResponse.Rev));
            Assert.AreEqual(DocType.Page, getResponse.PageContent.DocType);

            Delete(createResponse.Id, createResponse.Rev);
        }

        [TestMethod]
        public void CanCreateMultiplePagesForSameSite()
        {
            const string siteId = "www.example.com";
            var req = new PageContent { SiteId = siteId, Title = Constants.Ignored, Content = Constants.Ignored };

            var page1 = PageContentService.CreateAsync(req).Result;
            var page2 = PageContentService.CreateAsync(req).Result;

            page1.HasNoError();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(page1.Id));
            page2.HasNoError();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(page2.Id));
            Assert.AreNotEqual(page1.Id, page2.Id);

            Delete(page1.Id, page1.Rev);
            Delete(page2.Id, page2.Rev);
        }
    }
}
