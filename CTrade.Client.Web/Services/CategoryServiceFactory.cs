using CTrade.Client.Core;
using CTrade.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTrade.Client.Web.Services
{
    public interface ICategoryServiceFactory
    {
        ICategoryService Create();
    }
    public class CategoryServiceFactory : ICategoryServiceFactory
    {
        private readonly ILogger _logger;
        public CategoryServiceFactory(ILogger logger)
        {
            _logger = logger;
        }
        public ICategoryService Create()
        {
            return new CategoryService(DbRepositoryFactory.Create(Environment.Categories), _logger);
        }
    }
}