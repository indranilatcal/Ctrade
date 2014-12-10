using System.Web.Mvc;

namespace CTrade.Client.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller="Admin", action = "Index", area="Admin", id = UrlParameter.Optional },
                new string[] { "CTrade.Client.Web.Areas.Admin.Controllers" }
            );
        }
    }
}