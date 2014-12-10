using CTrade.Client.Core;
using CTrade.Client.DataAccess;
using CTrade.Client.Services;
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
    public interface IFAQServiceFactory
    {
        IFAQService Create();
    }
    public class FAQServiceFactory : IFAQServiceFactory
    {
        private readonly ILogger _logger;
        public FAQServiceFactory(ILogger logger)
        {
            _logger = logger;
        }
        public IFAQService Create()
        {
            return new FAQService(DbRepositoryFactory.Create(Environment.FAQ), _logger);
        }
    }
}