using CTrade.Client.Services.Entities;
using CTrade.Client.Services;
using CTrade.Client.Web.Areas.Admin.Models;
using CTrade.Client.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CTrade.Client.Core;

namespace CTrade.Client.Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryServiceFactory categoryServiceFactory, ILogger logger)
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
                var response = await _categoryService.GetCategoriesAsync();
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

        [HttpPost]
        public async Task<ActionResult> Update(string siteId, string[] subCategories)
        {
            siteId.NotNullOrWhiteSpace();

            if (subCategories.Any())
            {
                var updateResponse = await _categoryService.UpdateSubCategoriesAsync(siteId, subCategories);
                if (updateResponse.HasError)
                {
                    _logger.Error(updateResponse.Error);
                    throw new Exception(ErrorMessage.CategoryUpdateForSiteError);
                }
            }

            return Json(Url.Action("Index"));
        }

        public async Task<ActionResult> Overrides(string siteId)
        {
            siteId.NotNullOrWhiteSpace();
            _logger.Info(string.Format("siteId: {0}", siteId));

            ManageOverridesViewModel viewModel = null;

            var response = await _categoryService.GetCategoriesForSiteAsync(siteId);
            if (!response.HasError)
                viewModel = new ManageOverridesViewModel(siteId, response.Categories);
            else
            {
                _logger.Error(response.Error);
                throw new Exception(ErrorMessage.CategoryRetrieval);
            }

            return PartialView(viewModel ?? new ManageOverridesViewModel(siteId, new Category[] { }));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateOverrides(string siteId, ManageOverridesViewModel overrides)
        {
            siteId.NotNullOrWhiteSpace();
            overrides.NotNull();
            overrides.Categories.HasItems();

            _logger.Info(string.Format("siteId: {0}", siteId));

            var response = await _categoryService.OverrideAsync(siteId,
                overrides.Categories.Select(c => c.AsCategory(siteId)).ToArray());
            if (response.HasError)
            {
                _logger.Error(response.Error);
                throw new Exception(ErrorMessage.OverrideUpdateForSiteError);
            }

            return RedirectToAction("Index");
        }
    }
}