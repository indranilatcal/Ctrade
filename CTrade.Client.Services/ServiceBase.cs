using CTrade.Client.Core;
using CTrade.Client.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrade.Client.Services
{
    public abstract class ServiceBase
    {
        private readonly IDbRepository _repository;
        private readonly ILogger _logger;

        protected ServiceBase(IDbRepository dbRepository, ILogger logger)
        {
            dbRepository.NotNull();
            logger.NotNull();

            _logger = logger;
            _repository = dbRepository;
        }

        protected IDbRepository Repository
        {
            get { return _repository; }
        }

        protected ILogger Logger
        {
            get { return _logger; }
        }

        protected void HandleException(Exception ex)
        {
            _logger.Error(ex.Message, ex);
            throw new ServiceException(ErrorMessage.ServiceError);
        }
    }
}
