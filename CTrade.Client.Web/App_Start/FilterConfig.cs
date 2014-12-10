using CTrade.Client.Core;
using System.Web;
using System.Web.Mvc;

namespace CTrade.Client.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(DependencyResolver.Current.GetService<ExceptionLoggerFilter>());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
