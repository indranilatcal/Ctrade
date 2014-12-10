using CTrade.Client.DataAccess;
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
    static class DbRepositoryFactory
    {
        internal static IDbRepository Create(string connectionKey)
        {
            using (var rdr = new JsonTextReader(new StreamReader(GetEnvironmentJsonFile())))
            {
                var res = JArray.Load(rdr);
                if (res != null && res.Any())
                {
                    dynamic[] connections = res.Cast<dynamic>().ToArray();
                    dynamic conn = connections.First(c => (string)c.Key == connectionKey).Info;
                    return new DbRepository((string)conn.UserName, (string)conn.DbName, (string)conn.Key, (string)conn.Password);
                }

                throw new Exception("Connection details not found");
            }
        }

        private static string GetEnvironmentJsonFile()
        {
            return ConfigHelper.GetConfigFile("Environment.json");
        }
    }
}