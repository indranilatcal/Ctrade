using CTrade.Client.Services.Entities;
using CTrade.Client.Services.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace CTrade.Client.Services.Test.FAQ
{
    [TestClass]
    public class UpdateTests : FAQTestBase
    {
        private Question[] _questions;

        [TestInitialize]
        public void Setup()
        {
            _questions = new FAQFixture().Init(FAQService);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestQuestionIdIsMandatory()
        {
            new QuestionUpdateRequest(string.Empty, Constants.Ignored, UpdateType.Add);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSiteIdIsMandatory()
        {
            new QuestionUpdateRequest(Constants.Ignored, null, UpdateType.Add);
        }

        [TestMethod]
        public void TestUpdateQuestionsShouldAddOrDeleteSitesToQuestionBasedOnUpdateType()
        {
            const string siteId1 = "www.example.com";
            const string siteId2 = "www.myfaq.com";
            var qToAdd = _questions.Where(q => !q.Sites.Any(s => s == siteId1)).First();
            var qToRemove = _questions.Where(q => q.Sites.Any(s => s == siteId2)).First();

            var addRequest = new QuestionUpdateRequest(qToAdd.Id, siteId1, UpdateType.Add);
            var removeRequest = new QuestionUpdateRequest(qToRemove.Id, siteId2, UpdateType.Delete);
            var updateResponse = FAQService.UpdateQuestionsAsync(new QuestionUpdateRequest[] { addRequest, removeRequest }).Result;
            var getResponse = FAQService.GetQuestionsAsync().Result;

            Assert.IsFalse(updateResponse.HasErrors);
            Assert.AreEqual(3, getResponse.Questions.Count(q => q.Sites.Contains(siteId1)));
            Assert.IsFalse(getResponse.Questions.Any(q => q.Sites.Contains(siteId2)));
        }

        [TestMethod]
        public void TestErrorInOneRequestShouldNotStopRestToBeProcessed()
        {
            const string siteId1 = "www.example.com";
            const string siteId2 = "www.myfaq.com";
            var qToRemove = _questions.Where(q => q.Sites.Any(s => s == siteId2)).First();

            var addRequest = new QuestionUpdateRequest("junk", siteId1, UpdateType.Add);
            var removeRequest = new QuestionUpdateRequest(qToRemove.Id, siteId2, UpdateType.Delete);
            var updateResponse = FAQService.UpdateQuestionsAsync(new QuestionUpdateRequest[] { addRequest, removeRequest }).Result;
            var getResponse = FAQService.GetQuestionsAsync().Result;

            Assert.IsTrue(updateResponse.HasErrors);
            Assert.AreEqual(1, updateResponse.Errors.Length);
            Assert.AreEqual(string.Format(ErrorMessage.QuestionIdNotFoundFormat, "junk"), updateResponse.Errors.First());
            Assert.IsFalse(getResponse.Questions.Any(q => q.Sites.Contains(siteId2)));
        }
    }
}
