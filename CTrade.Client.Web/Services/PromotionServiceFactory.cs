using CTrade.Client.Core;
using CTrade.Client.Services;

namespace CTrade.Client.Web.Services
{
    public interface IPromotionServiceFactory
    {
        IPromotionService Create();
    }
    public class PromotionServiceFactory : IPromotionServiceFactory
    {
        private readonly ILogger _logger;

        public PromotionServiceFactory(ILogger logger)
        {
            logger.NotNull();

            _logger = logger;
        }

        public IPromotionService Create()
        {
            return new PromotionService(DbRepositoryFactory.Create(Environment.Promotions), _logger);
        }
    }
}