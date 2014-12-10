using System.IO;
using System.Web;

namespace CTrade.Client.Web.Helpers
{
    internal static class ConfigHelper
    {
        internal static string GetConfigFile(string configFile)
        {
            return Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), configFile);
        }
    }
}