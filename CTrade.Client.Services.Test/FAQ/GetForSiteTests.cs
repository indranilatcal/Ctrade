using CTrade.Client.Services.Entities;
using CTrade.Client.Services.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CTrade.Client.Services.Test.FAQ
{
    [TestClass]
    public class GetForSiteTests : FAQTestBase
    {
        [TestInitialize]
        public void Setup()
        {
            new FAQFixture().Init(FAQService);
        }

        [TestMethod]
        public void TestSiteIdIsMandatory()
        {
            var getResponse = FAQService.GetQuestionsForSiteAsync(string.Empty).Result;

            getResponse.HasError(ErrorMessage.SiteIdIsMandatory);
        }

        [TestMethod]
        public void TestNonExistingSiteShouldReturnNoResult()
        {
            var getResponse = FAQService.GetQuestionsForSiteAsync("junk").Result;

            getResponse.HasNoError();
            Assert.AreEqual(0, getResponse.Questions.Length);
        }

        [TestMethod]
        public void TestGetTestsForSiteShouldRetrieveQuestionsForSite()
        {
            const string siteId = "www.example.com";

            var getResponse = FAQService.GetQuestionsForSiteAsync(siteId).Result;

            getResponse.HasNoError();
            Assert.AreEqual(2, getResponse.Questions.Count(q => q.Sites.Contains(siteId)));
            Assert.IsTrue(getResponse.Questions.All(q => q.Sites.Contains(siteId)));
        }
    }
}
