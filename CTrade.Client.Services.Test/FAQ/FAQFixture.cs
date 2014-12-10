using CTrade.Client.DataAccess.Requests;
using CTrade.Client.Services.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace CTrade.Client.Services.Test.FAQ
{
    internal class FAQFixture
    {
        internal Question[] Init(IFAQService faqService)
        {
            TestRuntime.ResetRepository(Constants.MasterConnectionKey);
            TestRuntime.CreateIndex(Constants.MasterConnectionKey, DesignDocumentBuilder.CreateIndexContent());

            var repository = TestRuntime.CreateRepository(Constants.MasterConnectionKey);

            JObject[] _questions = CreateAllQuestions();
            var bulkRequest = BulkRequestFactory.Create();
            bulkRequest.Include(_questions);
            var docResult = repository.BulkAsync(bulkRequest).Result;
            Assert.IsFalse(docResult.HttpHeaderInfo.HasError);

            var questionResponse = faqService.GetQuestionsAsync().Result;
            Assert.IsTrue(questionResponse.Questions.Any());

            return questionResponse.Questions;
        }

        private JObject[] CreateAllQuestions()
        {
            return new JObject[]
            {
                CreateQuestionWithoutSite(),
                CreateQuestionWithOneSite(),
                CreateQuestionWithTwoSites()
            };
        }

        private JObject CreateQuestionWithoutSite()
        {
            dynamic questionWithoutSite = new JObject();

            questionWithoutSite.docType = DocType.Faq;
            questionWithoutSite.questionText = "q1";
            questionWithoutSite.answerText = "ans1";

            return questionWithoutSite;
        }

        private JObject CreateQuestionWithOneSite()
        {
            dynamic questionWithOneSite = new JObject();

            questionWithOneSite.docType = DocType.Faq;
            questionWithOneSite.questionText = "q2";
            questionWithOneSite.answerText = "ans2";
            questionWithOneSite.sites = new JArray();
            questionWithOneSite.sites.Add("www.example.com");

            return questionWithOneSite;
        }

        private JObject CreateQuestionWithTwoSites()
        {
            dynamic questionWithTwoSites = new JObject();

            questionWithTwoSites.docType = DocType.Faq;
            questionWithTwoSites.questionText = "q3";
            questionWithTwoSites.answerText = "ans3";
            questionWithTwoSites.sites = new JArray();
            questionWithTwoSites.sites.Add("www.example.com");
            questionWithTwoSites.sites.Add("www.myfaq.com");

            return questionWithTwoSites;
        }
    }
}
