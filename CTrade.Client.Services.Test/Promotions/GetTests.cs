using CTrade.Client.DataAccess.Requests;
using CTrade.Client.Services.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;

namespace CTrade.Client.Services.Test.Promotions
{
    [TestClass]
    public class GetTests : PromotionTestBase
    {
        [TestMethod]
        public void SiteIdShouldBeMandatory()
        {
            var response = PromotionService.GetAsync(null).Result;

            response.HasError(ErrorMessage.SiteIdIsMandatory);
        }

        [TestMethod]
        public void ShouldNotReturnErrorWhenPromotionNotFound()
        {
            var response = PromotionService.GetAsync(Constants.Ignored).Result;

            response.HasNoError();
            Assert.IsTrue(response.IsEmpty);
        }

        [TestMethod]
        public void ShouldReturnActivePromotionText()
        {
            const string siteId = "www.example.com";
            const string promotionText = "<ul><li>$100 rebate</li><li>Many more&nbsp;</li></ul>Thanks<br>";
            var request = new Promotion { Id = siteId, PromotionText = promotionText, Activate = true };
            var saveResponse = PromotionService.SaveAsync(request).Result;

            var getResponse = PromotionService.GetAsync(siteId).Result;

            getResponse.HasNoError();
            Assert.AreEqual(DocType.Promotion, getResponse.Promotion.DocType);
            Assert.IsFalse(getResponse.IsEmpty);
            Assert.AreEqual(promotionText, getResponse.Promotion.PromotionText);
        }

        [TestMethod]
        public void ActivePromotionWithoutTextShouldReturnError()
        {
            const string siteId = "mySite";
            dynamic promotion = SavePromotion(siteId, null, true);

            var getResult = PromotionService.GetAsync(siteId).Result;

            getResult.HasError(ErrorMessage.ActivePromotionShouldHaveText);
            Assert.IsTrue(getResult.IsEmpty);
            Delete(siteId, (string)promotion._rev);
        }

        [TestMethod]
        public void PromotionShouldHaveBothDatesWhenActiveIsFalse()
        {
            const string siteId = "mySite";
            dynamic promotion = SavePromotion(siteId, Constants.Ignored, false, DateTime.Now);

            var getResult = PromotionService.GetAsync(siteId).Result;

            getResult.HasError(ErrorMessage.PromotionShouldEitherBeActiveOrHaveDates);
            Assert.IsTrue(getResult.IsEmpty);
            Delete(siteId, (string)promotion._rev);
        }

        [TestMethod]
        public void PromotionShouldNotBeRetrievedWhenCurrentDateFallsOutsideRangeWhenActiveIsFalse()
        {
            const string siteId = "mySite";
            DateTime startDate = DateTime.Now.AddDays(1);
            DateTime endDate = startDate.AddDays(1);
            dynamic promotion = SavePromotion(siteId, Constants.Ignored, false, startDate, endDate);

            var getResult = PromotionService.GetAsync(siteId).Result;

            getResult.HasNoError();
            Assert.IsTrue(getResult.IsEmpty);
            Delete(siteId, (string)promotion._rev);
        }

        [TestMethod]
        public void PromotionShouldBeRetrievedWhenCurrentDateFallsWithinRangeWhenActiveIsFalse()
        {
            const string siteId = "mySite";
            const string promotionText = "valid text";
            DateTime startDate = DateTime.Now.AddDays(-1);
            DateTime endDate = startDate.AddDays(2);
            dynamic promotion = SavePromotion(siteId, promotionText, false, startDate, endDate);

            var getResult = PromotionService.GetAsync(siteId).Result;

            getResult.HasNoError();
            Assert.IsFalse(getResult.IsEmpty);
            Assert.AreEqual(promotionText, getResult.Promotion.PromotionText);
            Delete(siteId, (string)promotion._rev);
        }

        private JObject SavePromotion(string siteId, string promotionText = null,
            bool activate = false, DateTime? startDate = null, DateTime? endDate = null)
        {
            dynamic doc = new JObject();

            doc._id = siteId;
            if (!string.IsNullOrWhiteSpace(promotionText))
                doc.promotionText = promotionText;
            doc.activate = activate;
            if (startDate.HasValue)
                doc.startDate = startDate.Value;
            if (endDate.HasValue)
                doc.endDate = endDate.Value;

            return Persist(doc);
        }

        private JObject Persist(dynamic doc)
        {
            var repository = TestRuntime.CreateRepository(Constants.MasterConnectionKey);
            var bulkRequest = BulkRequestFactory.Create();
            bulkRequest.Include(doc);
            var docResult = repository.BulkAsync(bulkRequest).Result;
            Assert.IsFalse(docResult.HttpHeaderInfo.HasError);
            var getResult = repository.GetAsync((string)doc._id).Result;
            Assert.IsFalse(getResult.HttpHeaderInfo.HasError);
            return getResult.Data;
        }
    }
}
