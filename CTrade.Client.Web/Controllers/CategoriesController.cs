using CTrade.Client.Core;
using CTrade.Client.Services;
using CTrade.Client.Services.Entities;
using CTrade.Client.Web.Areas.Admin.Models;
using CTrade.Client.Web.Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CTrade.Client.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryServiceFactory categoryServiceFactory, ILogger logger)
        {
            logger.NotNull();
            categoryServiceFactory.NotNull();

            _logger = logger;
            _categoryService = categoryServiceFactory.Create();
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Categories(string siteId)
        {
            siteId.NotNullOrWhiteSpace();
            _logger.Info(string.Format("siteId: {0}", siteId));

            ManageCategoriesViewModel viewModel = null;

            if (!string.IsNullOrWhiteSpace(siteId))
            {
                var response = await _categoryService.GetCategoriesForSiteAsync(siteId);
                if (!response.HasError)
                    viewModel = new ManageCategoriesViewModel(siteId, response.Categories);
                else
                {
                    _logger.Error(response.Error);
                    throw new Exception(ErrorMessage.CategoryRetrieval);
                }
            }

            return PartialView(viewModel ?? new ManageCategoriesViewModel(siteId, new Category[] { }));
        }
    }
}