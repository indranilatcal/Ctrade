using CTrade.Client.Web.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CTrade.Client.Web.Services
{
    public interface ISiteService
    {
        IList<KeyValuePair<string, string>> GetSites();
    }
    public class SiteService : ISiteService
    {
        private static readonly IList<KeyValuePair<string, string>> _sites;

        static SiteService()
        {
            _sites = new List<KeyValuePair<string, string>>();

            using (var rdr = new JsonTextReader(new StreamReader(ConfigHelper.GetConfigFile("Sites.json"))))
            {
                var res = JArray.Load(rdr);
                if (res != null && res.Any())
                {
                    var sites = res.Cast<dynamic>().ToList();
                    sites.ForEach(s => _sites.Add(new KeyValuePair<string, string>((string)s.Key, (string)s.Value)));
                }
            }
        }
        public IList<KeyValuePair<string, string>> GetSites()
        {
            return _sites;
        }
    }
}