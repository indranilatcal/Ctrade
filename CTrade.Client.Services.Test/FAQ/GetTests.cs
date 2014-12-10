using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CTrade.Client.Services.Test.FAQ
{
    [TestClass]
    public class GetTests : FAQTestBase
    {
        [TestMethod]
        public void TestQuestionIdIsMandatory()
        {
            var response = FAQService.GetQuestionAsync(string.Empty).Result;

            response.HasError(ErrorMessage.QuestionIdIsMandatory);
        }

        [TestMethod]
        public void TestQuestionNotFoundShouldBeReportedAsError()
        {
            const string nonExistentQuestionId = "junk";
            var response = FAQService.GetQuestionAsync(nonExistentQuestionId).Result;

            response.HasError(string.Format(ErrorMessage.QuestionIdNotFoundFormat, nonExistentQuestionId));
        }

        [TestMethod]
        public void TestExistingQuestionDetailsShouldBeRetrieved()
        {
            const string questionText = "q";
            const string answerText = "a";
            var createResponse = FAQService.AddQuestionAsync(questionText, answerText).Result;

            var getResponse = FAQService.GetQuestionAsync(createResponse.QuestionId).Result;

            getResponse.HasNoError();
            Assert.AreEqual(1, getResponse.Questions.Length);
            var question = getResponse.Questions.Single();
            Assert.AreEqual(questionText, question.QuestionText);
            Assert.AreEqual(answerText, question.AnswerText);

            Delete(question.Id, question.Rev);
        }
    }
}
