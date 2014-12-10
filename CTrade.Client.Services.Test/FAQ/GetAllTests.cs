using CTrade.Client.Services.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace CTrade.Client.Services.Test.FAQ
{
    [TestClass]
    public class GetAllTests : FAQTestBase
    {
        [TestInitialize]
        public void Setup()
        {
            new FAQFixture().Init(FAQService);
        }

        [TestMethod]
        public void TestGetQuestionsRetrievesAllQuestionsIrrespectiveOfSites()
        {
            var response = FAQService.GetQuestionsAsync().Result;

            Assert.IsFalse(response.HasError);
            Assert.IsFalse(response.IsEmpty);
            Assert.AreEqual(3, response.Questions.Length);
            Assert.IsTrue(response.Questions.Any(q =>
                q.QuestionText == "q1" && q.AnswerText == "ans1" && q.Sites.Length == 0));
            Assert.IsTrue(response.Questions.Any(q =>
                q.QuestionText == "q2" && q.AnswerText == "ans2" && q.Sites.Length == 1
                && q.Sites.Any(s => s == "www.example.com"))
                );
            Assert.IsTrue(response.Questions.Any(q =>
                q.QuestionText == "q3" && q.AnswerText == "ans3" && q.Sites.Length == 2
                && q.Sites.Any(s => s == "www.example.com") && q.Sites.Any(s => s == "www.myfaq.com"))
                );
        }

        [TestMethod]
        public void TestGetQuestionsCanLimitResults()
        {
            var response = FAQService.GetQuestionsAsync(1).Result;

            Assert.AreEqual(1, response.Questions.Length);
        }
    }
}
