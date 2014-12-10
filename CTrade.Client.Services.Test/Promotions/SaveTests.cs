using CTrade.Client.Services.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CTrade.Client.Services.Test.Promotions
{
    [TestClass]
    public class SaveTests : PromotionTestBase
    {
    
        [TestMethod]
        public void SiteIdShouldBeMandatory()
        {
            var request = new Promotion { Id = string.Empty, PromotionText = Constants.Ignored };

            var response = PromotionService.SaveAsync(request).Result;

            response.HasError(ErrorMessage.SiteIdIsMandatory);
        }

        [TestMethod]
        public void PromotionTextShouldBeMandatory()
        {
            var request = new Promotion { Id = Constants.Ignored, PromotionText = null };

            var response = PromotionService.SaveAsync(request).Result;

            response.HasError(ErrorMessage.PromotionTextIsMandatory);
        }

        [TestMethod]
        public void ShouldHaveDatesIfActivateIsNotSpecified()
        {
            var request = new Promotion { Id = Constants.Ignored, PromotionText = Constants.Ignored };

            var response = PromotionService.SaveAsync(request).Result;

            response.HasError(ErrorMessage.DatesMandatoryInAbsenceOfActivate);
        }

        [TestMethod]
        public void EndDateShouldSucceedStartDate()
        {
            var now = DateTime.Now;
            var request = new Promotion { Id = Constants.Ignored, PromotionText = Constants.Ignored, StartDate = now.AddDays(1), EndDate = now };

            var response = PromotionService.SaveAsync(request).Result;

            response.HasError(ErrorMessage.EndDateShouldSucceedStartDate);
        }

        [TestMethod]
        public void EitherActivateOrDatesShouldBeSpecified()
        {
            var request = new Promotion { Id = Constants.Ignored, PromotionText = Constants.Ignored, StartDate = DateTime.Now, Activate = true };

            var response = PromotionService.SaveAsync(request).Result;

            response.HasError(ErrorMessage.EitherActivateOrDatesShouldBeSpecified);
        }

        [TestMethod]
        public void CanSavePromotionWithActivateOption()
        {
            const string siteId = "www.example.com";
            const string promotionText = "<ul><li>$100 rebate</li><li>Many more&nbsp;</li></ul>Thanks<br>";
            var request = new Promotion { Id = siteId, PromotionText = promotionText, Activate = true };

            var response = PromotionService.SaveAsync(request).Result;

            response.HasNoError();
        }

        [TestMethod]
        public void CanSavePromotionWithValidDates()
        {
            const string siteId = "www.example.com";
            const string promotionText = "<ul><li>$100 rebate</li><li>Many more&nbsp;</li></ul>Thanks<br>";
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddDays(1);
            var request = new Promotion { Id = siteId, PromotionText = promotionText, StartDate = startDate, EndDate = endDate };

            var saveResponse = PromotionService.SaveAsync(request).Result;
            var getResponse = PromotionService.GetAsync(siteId).Result;

            saveResponse.HasNoError();
            Assert.AreEqual(promotionText, getResponse.Promotion.PromotionText);
            Assert.IsFalse(getResponse.Promotion.Activate);
            Assert.AreEqual(startDate, getResponse.Promotion.StartDate);
            Assert.AreEqual(endDate, getResponse.Promotion.EndDate);
        }

        [TestMethod]
        public void ShouldOverwritePromotionWhenExists()
        {
            const string siteId = "www.example.com";
            const string promotionTextModified = "someText modified";
            var request = new Promotion { Id = siteId, PromotionText = "someText", Activate = true };
            PromotionService.SaveAsync(request).Wait();
            request.PromotionText = promotionTextModified;

            var saveResponse = PromotionService.SaveAsync(request).Result;
            var getResponse = PromotionService.GetAsync(siteId).Result;

            saveResponse.HasNoError();
            Assert.AreEqual(promotionTextModified, getResponse.Promotion.PromotionText);
        }        
    }
}
