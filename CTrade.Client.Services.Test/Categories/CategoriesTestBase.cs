using CTrade.Client.Core;
using CTrade.Client.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTrade.Client.Services.Test.Categories
{

    [TestClass]
    public abstract class CategoriesTestBase
    {
        private ICategoryService _categoryService;

        public CategoriesTestBase()
        {
            _categoryService = new CategoryService(TestRuntime.CreateRepository(Constants.MasterConnectionKey), new LogService());
        }

        protected ICategoryService CategoryService
        {
            get { return _categoryService; }
        }
    }
}
