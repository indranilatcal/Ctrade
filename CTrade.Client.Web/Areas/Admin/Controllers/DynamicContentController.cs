using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CTrade.Client.Core;
using CTrade.Client.Web.Areas.Admin.Models;
using CTrade.Client.Services;
using CTrade.Client.Web.Services;
using CTrade.Client.Services.Responses;

namespace CTrade.Client.Web.Areas.Admin.Controllers
{
    public class DynamicContentController : Controller
    {
        private const string _siteId = "SiteId";
        private const string _editUrlFormat = "{0}?pageId={1}";
        private readonly ILogger _logger;
        private readonly IPageContentService _pageContentService;

        private string SiteId
        {
            get
            {
                var siteIdCookie = Request.Cookies[_siteId];
                return siteIdCookie != null ? siteIdCookie.Value : null;
            }
        }

        public DynamicContentController(IPageContentServiceFactory pageContentServiceFactory, ILogger logger)
        {
            logger.NotNull();
            pageContentServiceFactory.NotNull();

            _logger = logger;
            _pageContentService = pageContentServiceFactory.Create();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string pageId)
        {
            ViewBag.SiteId = SiteId;
            ViewBag.PageId = pageId;

            return View();
        }

        public ActionResult Preview(string pageId)
        {
            ViewBag.SiteId = SiteId;
            ViewBag.PageId = pageId;

            return View();
        }

        public ActionResult EndUserView(string pageId)
        {
            ViewBag.SiteId = SiteId;
            ViewBag.PageId = pageId;

            return View();
        }

        [ChildActionOnly]
        public ActionResult PageContent(string pageId)
        {
            ViewBag.SiteId = SiteId;
            ViewBag.PageId = pageId;

            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(PageContentEditViewModel pageContent)
        {
            pageContent.SiteId.NotNullOrWhiteSpace();
            pageContent.Title.NotNullOrWhiteSpace();
            pageContent.Content.NotNullOrWhiteSpace();

            PageContentViewModel viewModel = null;
            IPageContentHeaderResponse editResponse = null;

            if (string.IsNullOrWhiteSpace(pageContent.PageId))
                editResponse = await _pageContentService.CreateAsync(pageContent.AsPageContent());
            else
                editResponse = await _pageContentService.EditAsync(pageContent.AsPageContent());
            viewModel = await ProcessEditAsync(pageContent.SiteId, editResponse);

            return Json(viewModel);
        }

        public async Task<ActionResult> Get(string siteId, string pageId)
        {
            PageContentViewModel viewModel = null;
            if (!string.IsNullOrWhiteSpace(siteId) && !string.IsNullOrWhiteSpace(pageId))
                viewModel = await GetAsync(siteId, pageId);

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetAll(string siteId)
        {
            siteId.NotNullOrWhiteSpace();

            IEnumerable<PageContentViewModel> pageContents = null;
            var response = await _pageContentService.GetForSiteAsync(siteId);
            if (!response.HasError)
            {
                if (!response.IsEmpty)
                {
                    var contents = new List<PageContentViewModel>();
                    foreach (var pageContent in response.PageContents.Select(p => new PageContentViewModel(p)))
                        contents.Add(AddUrls(pageContent));

                    pageContents = contents; 
                }
            }
            else
            {
                _logger.Error(response.Error);
                throw new Exception(ErrorMessage.PageContentRetrieval);
            }

            return Json(pageContents ?? Enumerable.Empty<PageContentViewModel>(), JsonRequestBehavior.AllowGet);
        }

        #region Private Helpers
        private PageContentViewModel AddUrls(PageContentViewModel pageContent)
        {
            pageContent.NotNull();

            pageContent.EditUrl = string.Format(_editUrlFormat, Url.Action("Edit"), pageContent.Id);
            pageContent.PreviewUrl = string.Format(_editUrlFormat, Url.Action("Preview"), pageContent.Id);
            return pageContent;
        }
        private async Task<PageContentViewModel> ProcessEditAsync(string siteId, IPageContentHeaderResponse headerResponse)
        {
            PageContentViewModel viewModel = null;

            if (!headerResponse.HasError)
                viewModel = await GetAsync(siteId, headerResponse.Id);
            else
                viewModel = new PageContentViewModel(headerResponse.Error);

            return viewModel;
        }

        private async Task<PageContentViewModel> GetAsync(string siteId, string pageId)
        {
            PageContentViewModel viewModel = null;
            var response = await _pageContentService.GetAsync(siteId, pageId);
            if (!response.HasError)
            {
                if (!response.IsEmpty)
                    viewModel = new PageContentViewModel(response.PageContent);
                else
                    viewModel = new PageContentViewModel();
            }
            else
                viewModel = new PageContentViewModel(response.Error);
            return viewModel;
        }
        #endregion
    }
}