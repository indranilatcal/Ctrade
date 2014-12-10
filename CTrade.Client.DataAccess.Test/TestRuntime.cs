using CTrade.Client.DataAccess;
using CTrade.Client.DataAccess.Requests;
using MyCouch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrade.Client.DataAccess.Test
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

        internal static IDbRepository CreateRepository()
        {
            return CreateRepository(Constants.DefaultConnectionKey);
        }

        internal static IDbRepository CreateRepository(string connectionKey)
        {
            dynamic conn = _testDbConnections.First(c => (string)c.Key == connectionKey).Info;
            return new DbRepository((string)conn.UserName, (string)conn.DbName, (string)conn.Key, (string)conn.Password);
        }

        internal static void EnsureCleanup()
        {
            EnsureCleanup(Constants.DefaultConnectionKey);
        }

        internal static void EnsureCleanup(string connectionKey)
        {
            var repository = CreateRepository(connectionKey);
            var response = repository.QueryAsync(QueryRequestFactory.CreatePrimaryIndexQuery()
                .Configure(q => q.Stale(Stale.UpdateAfter))
                ).Result;
            if (response.IsEmpty)
                return;
            var bulkRequest = BulkRequestFactory.Create();

            foreach (var row in response.Rows)
            {
                if (row.Id.ToLower() == "_design/_replicator")
                    continue;
                dynamic value = row.Value;
                bulkRequest.Delete(row.Id, value.rev.ToString());
            }

            if (!bulkRequest.IsEmpty)
                repository.BulkAsync(bulkRequest).Wait();
        }
    }
}
