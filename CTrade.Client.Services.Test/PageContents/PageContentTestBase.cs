using CTrade.Client.Core;
using CTrade.Client.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTrade.Client.Services.Test.PageContents
{

    [TestClass]
    public abstract class PageContentTestBase
    {
        private IPageContentService _pageContentService;

        public PageContentTestBase()
        {
            _pageContentService = new PageContentService(TestRuntime.CreateRepository(Constants.MasterConnectionKey), new LogService());
        }

        protected IPageContentService PageContentService
        {
            get { return _pageContentService; }
        }

        protected void Delete(string id, string rev)
        {
            var repository = TestRuntime.CreateRepository(Constants.MasterConnectionKey);
            var deleteResult = repository.DeleteAsync(id, rev).Result;
            Assert.IsFalse(deleteResult.HttpHeaderInfo.HasError);
        }
    }
}
