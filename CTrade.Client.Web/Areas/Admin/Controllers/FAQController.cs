using CTrade.Client.Core;
using CTrade.Client.Services;
using CTrade.Client.Web.Areas.Admin.Models;
using CTrade.Client.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CTrade.Client.Web.Areas.Admin.Controllers
{
    public class FAQController : Controller
    {
        private readonly ILogger _logger;
        private readonly IFAQService _faqService;

        public FAQController(IFAQServiceFactory faqServiceFactory, ILogger logger)
        {
            logger.NotNull();
            faqServiceFactory.NotNull();

            _logger = logger;
            _faqService = faqServiceFactory.Create();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult EditQuestion(string id)
        {
            return View((Object)id);
        }

        public async Task<ActionResult> Questions(string siteId)
        {
            siteId.NotNullOrWhiteSpace();
            ManageQuestionsForSiteViewModel manageQuestionsforSite = new ManageQuestionsForSiteViewModel();
            IList<QuestionAssociationViewModel> questions = null;

            var response = await _faqService.GetQuestionsAsync();
            if (!response.HasError)
                questions = response.Questions.Select(q => new QuestionAssociationViewModel(q, siteId)).ToList();
            else
            {
                _logger.Error(response.Error);
                throw new Exception(ErrorMessage.QuestionRetrieval);
            }

            return PartialView(new ManageQuestionsForSiteViewModel { SiteId = siteId,
                Questions = questions ?? new List<QuestionAssociationViewModel>()});
        }

        public async Task<ActionResult> ListQuestions(string siteId)
        {
            IEnumerable<QuestionViewModel> questions = null;

            var response = await _faqService.GetQuestionsForSiteAsync(siteId);
            if (!response.HasError)
                questions = response.Questions.Select(q => new QuestionViewModel(q));
            else
            {
                _logger.Error(response.Error);
                throw new Exception(ErrorMessage.QuestionRetrieval);
            }

            return Json(questions ?? new List<QuestionViewModel>(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Create(string questionText, string answerText, string siteId)
        {
            var createResponse = await _faqService.AddQuestionAsync(questionText, answerText, siteId);
            if (!createResponse.HasError)
                ViewBag.SuccessMessage = string.Format("Question created with an id {0}", createResponse.QuestionId);
            else
            {
                _logger.Error(createResponse.Error);
                throw new Exception(ErrorMessage.ErrorCreatingAQuestion);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Update(IList<QuestionAssociationViewModel> questions, string SiteId)
        {
            if (questions != null && questions.Any())
            {
                var updateResponse = await _faqService.UpdateQuestionsAsync(
                        questions.Select(q => q.AsUpdateRequest()).ToArray());

                if (updateResponse.HasErrors)
                {
                    foreach (var error in updateResponse.Errors)
                        _logger.Error(error);
                    throw new Exception(ErrorMessage.QuestionUpdateForSiteError);
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> GetQuestion(string questionId)
        {
            questionId.NotNullOrWhiteSpace();
            _logger.Info(string.Format("questionId: {0}", questionId));

            QuestionViewModel viewModel = await GetAsync(questionId);
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateQuestion(QuestionViewModel questionViewModel)
        {
            questionViewModel.NotNull();
            questionViewModel.Id.NotNullOrWhiteSpace();
            questionViewModel.QuestionText.NotNullOrWhiteSpace();
            questionViewModel.AnswerText.NotNullOrWhiteSpace();

            var response = await _faqService.EditQuestionAsync(questionViewModel.AsQuestion());
            QuestionViewModel viewModel = null;
            if (!response.HasError)
                viewModel = await GetAsync(questionViewModel.Id);
            else
                viewModel = new QuestionViewModel(response.Error);

            return Json(viewModel);
        }

        [NonAction]
        private async Task<QuestionViewModel> GetAsync(string questionId)
        {
            QuestionViewModel viewModel = null;
            var response = await _faqService.GetQuestionAsync(questionId);
            if (!response.HasError)
                viewModel = new QuestionViewModel(response.Questions.Single());
            else
                viewModel = new QuestionViewModel(response.Error);

            return viewModel;
        }
    }
}