using CTrade.Client.DataAccess.Requests;
using CTrade.Client.Services.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrade.Client.Services.Test.Categories
{
    internal class CategoriesFixture
    {
        internal void Init()
        {
            TestRuntime.ResetRepository(Constants.MasterConnectionKey);
            TestRuntime.CreateIndex(Constants.MasterConnectionKey, DesignDocumentBuilder.CreateIndexContent());
            CreateData();
        }

        private void CreateData()
        {
            using (var rdr = new JsonTextReader(new StreamReader(Constants.CategoriesDataFile)))
            {
                var res = JArray.Load(rdr);
                if (res != null && res.Any())
                {
                    var repository = TestRuntime.CreateRepository(Constants.MasterConnectionKey);
                    var categories = res.Cast<JObject>().ToArray();
                    var bulkRequest = BulkRequestFactory.Create();
                    bulkRequest.Include(categories);
                    if (!bulkRequest.IsEmpty)
                    {
                        var bulkResponse = repository.BulkAsync(bulkRequest).Result;
                        Assert.IsFalse(bulkResponse.HttpHeaderInfo.HasError);
                    }
                }
            }
        }
    }
}
