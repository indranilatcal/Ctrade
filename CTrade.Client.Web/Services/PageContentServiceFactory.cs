using CTrade.Client.Core;
using CTrade.Client.Services;

namespace CTrade.Client.Web.Services
{
    public interface IPageContentServiceFactory
    {
        IPageContentService Create();
    }
    public class PageContentServiceFactory : IPageContentServiceFactory
    {
        private readonly ILogger _logger;

        public PageContentServiceFactory(ILogger logger)
        {
            logger.NotNull();

            _logger = logger;
        }

        public IPageContentService Create()
        {
            return new PageContentService(DbRepositoryFactory.Create(Environment.DynamicPages), _logger);
        }
    }
}