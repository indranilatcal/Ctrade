using System;
using CTrade.Client.Core;
using CTrade.Client.DataAccess.Responses;

namespace CTrade.Client.Services.Responses
{
    public interface IPageContentHeaderResponse : IServiceResponse
    {
        string Id { get; }
        string Rev { get; }
    }
    public class PageContentHeaderResponse : IPageContentHeaderResponse
    {
        private readonly string _error;
        private readonly string _id;
        private readonly string _rev;

        public PageContentHeaderResponse(string errorMessage)
        {
            errorMessage.NotNullOrWhiteSpace();

            _id = _rev = string.Empty;
            _error = errorMessage;
        }

        internal PageContentHeaderResponse(IDbDocHeaderResponse response)
        {
            response.NotNull();

            _id = response.Id ?? string.Empty;
            _rev = response.Revision ?? string.Empty;
            _error = response.HttpHeaderInfo.Error;
        }
        public bool HasError { get { return !string.IsNullOrWhiteSpace(_error); } }

        public string Error { get { return _error; } }
        public string Id { get { return _id; } }
        public string Rev { get { return _rev; } }
    }
}
