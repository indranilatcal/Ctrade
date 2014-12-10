using CTrade.Client.Core;
using CTrade.Client.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTrade.Client.Services.Test.FAQ
{

    [TestClass]
    public abstract class FAQTestBase
    {
        private IFAQService _faqService;

        public FAQTestBase()
        {
            _faqService = new FAQService(TestRuntime.CreateRepository(Constants.MasterConnectionKey), new LogService());
        }

        protected IFAQService FAQService
        {
            get { return _faqService; }
        }

        protected void Delete(string id, string rev)
        {
            var repository = TestRuntime.CreateRepository(Constants.MasterConnectionKey);
            var deleteResult = repository.DeleteAsync(id, rev).Result;
            Assert.IsFalse(deleteResult.HttpHeaderInfo.HasError);
        }
    }
}
