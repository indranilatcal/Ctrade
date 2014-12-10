using System;
using System.Net;

namespace CTrade.Client.DataAccess
{
    public class RepositoryException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public RepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }
    }
}
