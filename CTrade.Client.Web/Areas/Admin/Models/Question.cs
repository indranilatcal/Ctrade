using CTrade.Client.Services.Entities;
using CTrade.Client.Services.Requests;
using System.Linq;
using CTrade.Client.Core;
using System.Web.Mvc;

namespace CTrade.Client.Web.Areas.Admin.Models
{
    public class QuestionAssociationViewModel
    {
        private Question _question;

        public QuestionAssociationViewModel()
        {

        }
        public QuestionAssociationViewModel(Question question, string siteId)
        {
            _question = question;
            SiteId = siteId;
            QuestionId = _question.Id;
            IncludedForSite = !string.IsNullOrWhiteSpace(SiteId) && _question.Sites.Contains(SiteId);
        }

        public string QuestionText { get { return _question.QuestionText; } }
        public string AnswerText { get { return _question.AnswerText; } }
        public bool IncludedForSite { get; set; }
        public string SiteId { get; set; }
        public string QuestionId { get; set; }
        internal QuestionUpdateRequest AsUpdateRequest()
        {
            return new QuestionUpdateRequest(QuestionId, SiteId, IncludedForSite ? UpdateType.Add : UpdateType.Delete);
        }
    }

    public class QuestionViewModel
    {
        public QuestionViewModel() { }
        internal QuestionViewModel(Question question)
        {
            question.NotNull();

            Id = question.Id;
            QuestionText = question.QuestionText;
            AnswerText = question.AnswerText;
        }

        internal QuestionViewModel(string errorMessage)
        {
            Error = errorMessage;
        }

        [AllowHtml]
        public string QuestionText { get; set; }
        [AllowHtml]
        public string AnswerText { get; set; }
        public string Id { get; set; }
        public string Error { get; set; }
        public bool HasError { get { return !string.IsNullOrWhiteSpace(Error); } }

        internal Question AsQuestion()
        {
            return new Question { Id = Id, QuestionText = QuestionText, AnswerText = AnswerText };
        }
    }
}