using CTrade.Client.DataAccess;
using CTrade.Client.DataAccess.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCouch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace CTrade.Client.Services.Test
{
    internal static class TestRuntime
    {
        private static dynamic[] _testDbConnections;

        static TestRuntime()
        {
            using (var rdr = new JsonTextReader(new StreamReader(Constants.EnvironmentJsonFile)))
            {
                var res = JArray.Load(rdr);
                if(res != null && res.Any())
                    _testDbConnections = res.Cast<dynamic>().ToArray();
            }
        }

        internal static IDbRepository CreateRepository(string connectionKey)
        {
            dynamic conn = _testDbConnections.First(c => (string)c.Key == connectionKey).Info;
            return new DbRepository((string)conn.UserName, (string)conn.DbName, (string)conn.Key, (string)conn.Password);
        }

        internal static void ResetRepository(string connectionKey)
        {
            var repository = CreateRepository(connectionKey);
            var response = repository.QueryAsync(QueryRequestFactory.CreatePrimaryIndexQuery()
                .Configure(q => q.Stale(Stale.UpdateAfter))
                ).Result;
            if (!response.IsEmpty)
            {
                var bulkRequest = BulkRequestFactory.Create();

                foreach (var row in response.Rows)
                {
                    if (row.Id.ToLower() == "_design/_replicator")
                        continue;
                    dynamic value = row.Value;
                    bulkRequest.Delete(row.Id, value.rev.ToString());
                }

                if (!bulkRequest.IsEmpty)
                {
                    var bulkResponse = repository.BulkAsync(bulkRequest).Result;
                    Assert.IsFalse(bulkResponse.HttpHeaderInfo.HasError);
                }
            }
        }

        internal static void CreateIndex(string connectionKey, string indexContent)
        {
            var repository = CreateRepository(connectionKey);
            var createResponse = repository.CreateAsync(JObject.Parse(indexContent)).Result;
            Assert.IsFalse(createResponse.HttpHeaderInfo.HasError);
        }
    }
}
