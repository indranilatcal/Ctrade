using System;
using System.Net;

namespace CTrade.Client.Services
{
    public class ServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public ServiceException(string message)
            : base(message)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }
    }
}
