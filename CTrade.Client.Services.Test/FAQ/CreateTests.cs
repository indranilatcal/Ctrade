using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CTrade.Client.Services.Test.FAQ
{
    [TestClass]
    public class CreateTests : FAQTestBase
    {
        [TestMethod]
        public void TestCreateQuestionFailsWithoutQuestionText()
        {        
            var response = FAQService.AddQuestionAsync(string.Empty, "ignored", "www.example.com").Result;

            response.HasError(ErrorMessage.QuestionTextIsMandatory);
        }

        [TestMethod]
        public void TestCreateQuestionFailsWithoutAnswerText()
        {
            var response = FAQService.AddQuestionAsync("ignored", string.Empty, "www.example.com").Result;

            response.HasError(ErrorMessage.AnswerTextIsMandatory);
        }

        [TestMethod]
        public void TestCreateQuestionSucceedsWithoutSites()
        {
            var createResponse = FAQService.AddQuestionAsync("my question", "my answer").Result;

            var getResponse = FAQService.GetQuestionAsync(createResponse.QuestionId).Result;
            createResponse.HasNoError();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(createResponse.QuestionId));
            var question = getResponse.Questions.Single();
            Assert.AreEqual(DocType.Faq, question.DocType);

            Delete(question.Id, question.Rev);
        }

        [TestMethod]
        public void TestCreateQuestionSucceedsWithMultipleSites()
        {
            var createResponse = FAQService.AddQuestionAsync("my question", "my answer", "www.example.com", "www.myfaq.com").Result;

            var getResponse = FAQService.GetQuestionAsync(createResponse.QuestionId).Result;
            createResponse.HasNoError();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(createResponse.QuestionId));
            var question = getResponse.Questions.Single();
            Assert.AreEqual(DocType.Faq, question.DocType);

            Delete(question.Id, question.Rev);
        }
    }
}
