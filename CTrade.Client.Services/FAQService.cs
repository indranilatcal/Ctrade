
using CTrade.Client.DataAccess;
using CTrade.Client.DataAccess.Requests;
using CTrade.Client.Services.Entities;
using CTrade.Client.Services.Requests;
using CTrade.Client.Services.Responses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTrade.Client.Core;
using System.Net;

namespace CTrade.Client.Services
{
    public interface IFAQService
    {
        Task<IHeaderResponse> AddQuestionAsync(string questionText, string answerText, params string[] sites);
        Task<IQuestionResponse> GetQuestionsAsync(int maxNnumberOfQuestions = 0);
        Task<IQuestionResponse> GetQuestionsForSiteAsync(string siteId);
        Task<IQuestionUpdateResponse> UpdateQuestionsAsync(QuestionUpdateRequest[] requests);
        Task<IQuestionEditResponse> EditQuestionAsync(Question question);
        Task<IQuestionResponse> GetQuestionAsync(string questionId);
    }

    public class FAQService : ServiceBase, IFAQService
    {
        public FAQService(IDbRepository dbRepository, ILogger logger) : base(dbRepository, logger) { }

        public async Task<IHeaderResponse> AddQuestionAsync(string questionText, string answerText, params string[] sites)
        {
            IHeaderResponse addResponse = null;
            try
            {
                if(string.IsNullOrWhiteSpace(questionText))
                    return new HeaderResponse(ErrorMessage.QuestionTextIsMandatory);
                if (string.IsNullOrWhiteSpace(answerText))
                    return new HeaderResponse(ErrorMessage.AnswerTextIsMandatory);

                dynamic question = new JObject();
                question.questionText = questionText;
                question.answerText = answerText;
                question.docType = DocType.Faq;

                if (sites != null && sites.Any())
                    question.sites = new JArray(sites);

                var response = await Repository.CreateAsync(question as JObject);
                addResponse = new HeaderResponse(response);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return addResponse;
        }

        public async Task<IQuestionResponse> GetQuestionsAsync(int maxNnumberOfQuestions = 0)
        {
            IQuestionResponse questionResponse = null;
            try
            {
                var searchRequest = SearchRequestFactory.Create(Index.DesignDoc, Index.Questions);

                if(maxNnumberOfQuestions > 0)
                    searchRequest.Configure(q => q.Limit(maxNnumberOfQuestions));

                searchRequest.Configure(q => q.IncludeDocs(true)
                    .Expression(string.Format(Index.DocTypeExpressionFormat, DocType.Faq))
                    );

                var response = await Repository.SearchAsync(searchRequest);
                questionResponse = new QuestionResponse(response);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return questionResponse;
        }

        public async Task<IQuestionResponse> GetQuestionsForSiteAsync(string siteId)
        {
            if (string.IsNullOrWhiteSpace(siteId))
                return new QuestionResponse(ErrorMessage.SiteIdIsMandatory);
            IQuestionResponse questionResponse = null;
            try
            {
                var searchRequest = SearchRequestFactory.Create(Index.DesignDoc, Index.Questions);
                searchRequest.Configure(q => q.IncludeDocs(true)
                    .Expression(string.Format(Index.MatchSiteExpressionFormat, siteId))
                    );

                var response = await Repository.SearchAsync(searchRequest);
                questionResponse = new QuestionResponse(response);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return questionResponse;
        }

        public async Task<IQuestionResponse> GetQuestionAsync(string questionId)
        {
            if (string.IsNullOrWhiteSpace(questionId))
                return new QuestionResponse(ErrorMessage.QuestionIdIsMandatory);
            IQuestionResponse questionResponse = null;
            try
            {
                var getResponse = await Repository.GetAsync(questionId);
                if(getResponse.HttpHeaderInfo.HasError && getResponse.HttpHeaderInfo.StatusCode == HttpStatusCode.NotFound)
                    questionResponse = new QuestionResponse(string.Format(ErrorMessage.QuestionIdNotFoundFormat, questionId));
                else
                    questionResponse = new QuestionResponse(getResponse);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return questionResponse;
        }

        public async Task<IQuestionUpdateResponse> UpdateQuestionsAsync(QuestionUpdateRequest[] requests)
        {
            requests.HasItems();
            IQuestionUpdateResponse questionsUpdateResponse = new QuestionUpdateResponse();
            try
            {
                var allQuestionsResponse = await GetQuestionsAsync();
                if (allQuestionsResponse.Questions != null && allQuestionsResponse.Questions.Any())
                {
                    var questionsToUpdate = requests.Join(allQuestionsResponse.Questions,
                        r => r.QuestionId, q => q.Id, (r, q) => q);
                    if(questionsToUpdate.Any())
                    {
                        List<JObject> docs = new List<JObject>();

                        foreach(var request in requests)
                        {
                            var question = questionsToUpdate.SingleOrDefault(q => q.Id == request.QuestionId);
                            if (question != null)
                                ProcessUpdateRequest(docs, request, question);
                            else
                                questionsUpdateResponse.AddErrors(
                                    string.Format(ErrorMessage.QuestionIdNotFoundFormat, request.QuestionId));
                        }

                        if (docs.Any())
                        {
                            var bulkResponse = await PersistQuestions(docs);
                            questionsUpdateResponse.ProcessResponse(bulkResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return questionsUpdateResponse;
        }

        public async Task<IQuestionEditResponse> EditQuestionAsync(Question question)
        {
            question.NotNull();
            IQuestionEditResponse editQuestionResponse = null;

            if (string.IsNullOrWhiteSpace(question.Id))
                return new QuestionEditResponse(ErrorMessage.QuestionIdIsMandatory);
            if (string.IsNullOrWhiteSpace(question.QuestionText))
                return new QuestionEditResponse(ErrorMessage.QuestionTextIsMandatory);
            if (string.IsNullOrWhiteSpace(question.AnswerText))
                return new QuestionEditResponse(ErrorMessage.AnswerTextIsMandatory);

            try
            {
                var getResponse = await Repository.GetAsync(question.Id);
                if (getResponse.HttpHeaderInfo.HasError)
                    editQuestionResponse = new QuestionEditResponse(getResponse.HttpHeaderInfo.Error);
                else
                {
                    var questionToEdit = (getResponse.Data as JObject).AsQuestion();
                    questionToEdit.QuestionText = question.QuestionText;
                    questionToEdit.AnswerText = question.AnswerText;
                    var editResponse = await Repository.UpdateAsync(getResponse.Id, getResponse.Revision,
                        questionToEdit.AsJObject());
                    editQuestionResponse = new QuestionEditResponse(editResponse);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return editQuestionResponse;
        }

        #region Private members

        private async Task<DataAccess.Responses.BulkResponse> PersistQuestions(List<JObject> docs)
        {
            var bulkRequest = BulkRequestFactory.Create();
            bulkRequest.Include(docs.ToArray());
            var bulkResponse = await Repository.BulkAsync(bulkRequest);
            return bulkResponse;
        }

        private void ProcessUpdateRequest(List<JObject> docs, QuestionUpdateRequest request, Question question)
        {
            var sitesforQuestion = question.Sites.ToList();
            UpdateSitesForQuestion(request, sitesforQuestion);
            question.Sites = sitesforQuestion.ToArray();
            docs.Add(question.AsJObject());
        }

        private void UpdateSitesForQuestion(QuestionUpdateRequest request, List<string> sitesForQuestion)
        {
            if (request.UpdateType == UpdateType.Add)
            {
                if (!sitesForQuestion.Contains(request.SiteId))
                    sitesForQuestion.Add(request.SiteId);
            }
            else
            {
                if (sitesForQuestion.Contains(request.SiteId))
                    sitesForQuestion.Remove(request.SiteId);
            }
        }
        #endregion
    }
}
