using CTrade.Client.Services.Requests;
using CTrade.Client.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CTrade.Client.Core;
using CTrade.Client.Services;
using CTrade.Client.Web.Services;

namespace CTrade.Client.Web.Areas.Admin.Controllers
{
    public class PromotionController : Controller
    {
        private readonly ILogger _logger;
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionServiceFactory promotionServiceFactory, ILogger logger)
        {
            logger.NotNull();
            promotionServiceFactory.NotNull();

            _logger = logger;
            _promotionService = promotionServiceFactory.Create();
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Update(PromotionRequestViewModel request)
        {
            request.SiteId.NotNullOrWhiteSpace();
            request.PromotionText.NotNullOrWhiteSpace();
            _logger.Info(string.Format("siteId: {0}", request.SiteId));

            PromotionViewModel viewModel = null;
            var response = await _promotionService.SaveAsync(request.AsPromotion());
            if (!response.HasError)
                viewModel = await GetAsync(request.SiteId);
            else
                viewModel = new PromotionViewModel(response.Error);

            return Json(viewModel);
        }

        public async Task<ActionResult> Get(string siteId)
        {
            siteId.NotNullOrWhiteSpace();
            _logger.Info(string.Format("siteId: {0}", siteId));

            var viewModel = await GetAsync(siteId);
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private async Task<PromotionViewModel> GetAsync(string siteId)
        {
            var response = await _promotionService.GetAsync(siteId);
            return new PromotionViewModel(response);
        }
    }
}