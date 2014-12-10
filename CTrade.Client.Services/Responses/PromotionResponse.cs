using CTrade.Client.Core;
using CTrade.Client.DataAccess.Responses;
using CTrade.Client.Services.Entities;
using System;
using System.Net;

namespace CTrade.Client.Services.Responses
{
    public interface IPromotionResponse : IServiceResponse
    {
        bool IsEmpty { get; }
        Promotion Promotion { get; } 
    }
    public class PromotionResponse : IPromotionResponse
    {
        private Promotion _promotion;
        private string _error;

        internal PromotionResponse(string errorMessage)
        {
            errorMessage.NotNullOrWhiteSpace();

            _error = errorMessage;
            _promotion = null;
        }

        internal PromotionResponse(IDbDocResponse getResponse)
        {
            getResponse.NotNull();

            _error = (getResponse.HttpHeaderInfo.HasError && getResponse.HttpHeaderInfo.StatusCode != HttpStatusCode.NotFound) ?
                getResponse.HttpHeaderInfo.Error : string.Empty;
            _promotion = getResponse.Data != null ? getResponse.Data.AsPromotion() : null;
            if(!IsEmpty)
                ApplyValidation();
        }

        private void ApplyValidation()
        {
            if (_promotion.Activate)
            {
                if(string.IsNullOrWhiteSpace(_promotion.PromotionText))
                    _error = ErrorMessage.ActivePromotionShouldHaveText;
            }
            else if(_promotion.StartDate == null || _promotion.EndDate == null)
            {
                _error = ErrorMessage.PromotionShouldEitherBeActiveOrHaveDates;
            }
            else
            {
                DateTime currentDate = DateTime.Now;
                if (!(_promotion.StartDate.Value <= currentDate && currentDate <= _promotion.EndDate.Value))
                    _promotion = null;
            }

            if (HasError)
                _promotion = null;
        }

        public string Error { get { return _error; } }
        public bool HasError { get { return !string.IsNullOrWhiteSpace(_error); } }

        public Promotion Promotion
        {
            get { return _promotion; }
        }

        public bool IsEmpty
        {
            get { return _promotion == null; }
        }
    }
}
