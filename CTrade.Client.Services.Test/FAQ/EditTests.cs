using CTrade.Client.Services.Entities;
using CTrade.Client.Services.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CTrade.Client.Services.Test.FAQ
{
    [TestClass]
    public class EditTests : FAQTestBase
    {
        [TestMethod]
        public void TestQuestionIdIsMandatory()
        {
            var editResponse = FAQService.EditQuestionAsync(
                new Question { Id = string.Empty, QuestionText = Constants.Ignored, AnswerText = Constants.Ignored }).Result;

            editResponse.HasError(ErrorMessage.QuestionIdIsMandatory);
        }
        
        [TestMethod]
        public void TestQuestionTextIsMandatory()
        {
            var editResponse = FAQService.EditQuestionAsync(
                new Question { Id = Constants.Ignored, QuestionText = string.Empty, AnswerText = Constants.Ignored }).Result;

            editResponse.HasError(ErrorMessage.QuestionTextIsMandatory);
        }

        [TestMethod]
        public void TestAnswerTextIsMandatory()
        {
            var editResponse = FAQService.EditQuestionAsync(
                new Question { Id = Constants.Ignored, QuestionText = Constants.Ignored, AnswerText = null }).Result;

            editResponse.HasError(ErrorMessage.AnswerTextIsMandatory);
        }

        [TestMethod]
        public void TestNonExistentQuestionIdShouldReportError()
        {
            var editResponse = FAQService.EditQuestionAsync(
                new Question { Id = "junk", QuestionText = Constants.Ignored, AnswerText = Constants.Ignored }).Result;

            editResponse.HasError();
        }

        [TestMethod]
        public void TestSuccessfulEdit()
        {
            var createResponse = FAQService.AddQuestionAsync("q", "a").Result;

            var editResponse = FAQService.EditQuestionAsync(
                new Question { Id = createResponse.QuestionId, QuestionText = "qUpdated", AnswerText = "aUpdated" }).Result;
            var getResponse = FAQService.GetQuestionAsync(createResponse.QuestionId).Result;

            editResponse.HasNoError();
            Assert.AreEqual(1, getResponse.Questions.Count(q =>
                q.Id == createResponse.QuestionId &&
                q.QuestionText == "qUpdated" &&
                q.AnswerText == "aUpdated"
                ));
            var question = getResponse.Questions.Single();
            Delete(question.Id, question.Rev);
        }
    }
}
