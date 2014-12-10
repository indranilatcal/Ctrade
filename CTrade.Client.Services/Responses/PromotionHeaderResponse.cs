using CTrade.Client.DataAccess.Responses;
using CTrade.Client.Core;

namespace CTrade.Client.Services.Responses
{
    public interface IPromotionHeaderResponse : IServiceResponse
    {
        string SiteId { get; }
    }

    public class PromotionHeaderResponse : IPromotionHeaderResponse
    {
        private readonly string _siteId;
        private readonly string _error;

        internal PromotionHeaderResponse(IDbDocHeaderResponse response)
        {
            response.NotNull();

            _siteId = response.Id ?? string.Empty;
            _error = response.HttpHeaderInfo.Error;
        }

        internal PromotionHeaderResponse(string errorMessage)
        {
            errorMessage.NotNullOrWhiteSpace();

            _siteId = string.Empty;
            _error = errorMessage;
        }
        public string SiteId { get { return _siteId; } }
        public string Error { get { return _error; } }
        public bool HasError { get { return !string.IsNullOrWhiteSpace(_error); } }
    }
}
