using CTrade.Client.Core;
using CTrade.Client.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTrade.Client.Services.Test.Promotions
{

    [TestClass]
    public abstract class PromotionTestBase
    {
        private IPromotionService _promotionService;
        public PromotionTestBase()
        {
            _promotionService = new PromotionService(TestRuntime.CreateRepository(Constants.MasterConnectionKey), new LogService());
        }

        protected IPromotionService PromotionService
        {
            get { return _promotionService; }
        }

        protected void Delete(string id, string rev)
        {
            var repository = TestRuntime.CreateRepository(Constants.MasterConnectionKey);
            var deleteResult = repository.DeleteAsync(id, rev).Result;
            Assert.IsFalse(deleteResult.HttpHeaderInfo.HasError);
        }
    }
}
