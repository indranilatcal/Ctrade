using CTrade.Client.Core;
using CTrade.Client.Services.Entities;
using CTrade.Client.Services.Responses;
using System;
using System.Web.Mvc;

namespace CTrade.Client.Web.Areas.Admin.Models
{
    public class PromotionRequestViewModel
    {
        public string SiteId { get; set; }
        [AllowHtml]
        public string PromotionText { get; set; }
        public bool Activate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        internal Promotion AsPromotion()
        {
            return new Promotion
            {
                Id = SiteId,
                Activate = Activate,
                PromotionText = PromotionText,
                StartDate = StartDate,
                EndDate = EndDate
            };
        }
    }

    public class PromotionViewModel
    {
        private IPromotionResponse _response;

        internal PromotionViewModel(IPromotionResponse response)
        {
            response.NotNull();

            _response = response;
            Error = _response.Error;
        }

        internal PromotionViewModel(string errorMessage)
        {
            errorMessage.NotNullOrWhiteSpace();

            _response = null;
            Error = errorMessage;
        }

        public bool IsEmpty { get { return _response == null || _response.IsEmpty; } }
        public bool HasError { get { return !string.IsNullOrWhiteSpace(Error); } }
        public string PromotionText { get { return !IsEmpty ? _response.Promotion.PromotionText : null; } }
        public string Error { get; set; }
        public DateTime? StartDate { get { return !IsEmpty ? _response.Promotion.StartDate : null; } }
        public DateTime? EndDate { get { return !IsEmpty ? _response.Promotion.EndDate : null; } }
        public bool Activate { get { return !IsEmpty && _response.Promotion.Activate; } }
    }
}