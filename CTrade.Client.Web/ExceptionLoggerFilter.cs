using CTrade.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTrade.Client.Web
{
    public class ExceptionLoggerFilter : IExceptionFilter
    {
        private readonly ILogger _logger;
        public ExceptionLoggerFilter(ILogger logger)
        {
            logger.NotNull();

            _logger = logger;
        }
        public void OnException(ExceptionContext filterContext)
        {
            _logger.Error(filterContext.Exception.Message, filterContext.Exception);
        }
    }
}