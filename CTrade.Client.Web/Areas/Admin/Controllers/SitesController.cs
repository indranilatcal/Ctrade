using CTrade.Client.Web.Services;
using System.Web.Mvc;

namespace CTrade.Client.Web.Areas.Admin.Controllers
{
    public class SitesController : Controller
    {
        private readonly ISiteService _siteService;
        public SitesController(ISiteService siteService)
        {
            _siteService = siteService;
        }

        [ChildActionOnly]
        public ActionResult List()
        {
            return PartialView(_siteService.GetSites());
        }
    }
}