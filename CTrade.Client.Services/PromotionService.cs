using CTrade.Client.Core;
using CTrade.Client.DataAccess;
using CTrade.Client.Services.Entities;
using CTrade.Client.Services.Responses;
using System;
using System.Threading.Tasks;

namespace CTrade.Client.Services
{
    public interface IPromotionService
    {
        Task<IPromotionHeaderResponse> SaveAsync(Promotion promotion);
        Task<IPromotionResponse> GetAsync(string siteId);
    }
    public class PromotionService : ServiceBase, IPromotionService
    {
        public PromotionService(IDbRepository dbRepository, ILogger logger) : base(dbRepository, logger) { }
        public async Task<IPromotionHeaderResponse> SaveAsync(Promotion promotion)
        {
            promotion.NotNull();
            IPromotionHeaderResponse saveResponse = null;
            string errorMessage = null;
            if (!ValidForSave(promotion, out errorMessage))
                saveResponse = new PromotionHeaderResponse(errorMessage);
            else
            {
                try
                {
                    var getResponse = await GetAsync(promotion.Id);
                    if (!getResponse.HasError)
                    {
                        if (getResponse.IsEmpty)
                            saveResponse = await CreateAsync(promotion);
                        else
                        {
                            promotion.Rev = getResponse.Promotion.Rev;
                            saveResponse = await UpdateAsync(promotion);
                        }
                    }
                    else
                        saveResponse = new PromotionHeaderResponse(getResponse.Error);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }

            return saveResponse;
        }
        
        public async Task<IPromotionResponse> GetAsync(string siteId)
        {
            if (string.IsNullOrWhiteSpace(siteId))
                return new PromotionResponse(ErrorMessage.SiteIdIsMandatory);
            IPromotionResponse promotionResponse = null;

            try
            {
                var getResponse = await Repository.GetAsync(siteId);
                promotionResponse = new PromotionResponse(getResponse);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return promotionResponse;
        }

        #region Private Helpers
        private async Task<IPromotionHeaderResponse> UpdateAsync(Promotion promotion)
        {
            var updateResponse = await Repository.UpdateAsync(promotion.Id, promotion.Rev, promotion.AsJObject());
            return new PromotionHeaderResponse(updateResponse);
        }

        private async Task<IPromotionHeaderResponse> CreateAsync(Promotion promotion)
        {
            var createResponse = await Repository.CreateAsync(promotion.AsJObject());
            return new PromotionHeaderResponse(createResponse);
        }

        private bool ValidForSave(Promotion promotion, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(promotion.Id))
                errorMessage = ErrorMessage.SiteIdIsMandatory;
            else if (string.IsNullOrWhiteSpace(promotion.PromotionText))
                errorMessage = ErrorMessage.PromotionTextIsMandatory;
            else if (!promotion.Activate)
            {
                if (promotion.StartDate == null || promotion.EndDate == null)
                    errorMessage = ErrorMessage.DatesMandatoryInAbsenceOfActivate;
                else if (promotion.StartDate >= promotion.EndDate)
                    errorMessage = ErrorMessage.EndDateShouldSucceedStartDate;
            }
            else
            {
                if (promotion.StartDate.HasValue || promotion.EndDate.HasValue)
                    errorMessage = ErrorMessage.EitherActivateOrDatesShouldBeSpecified;
            }

            return string.IsNullOrWhiteSpace(errorMessage);
        }
        #endregion
    }
}
