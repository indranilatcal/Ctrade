using CTrade.Client.Core;
using CTrade.Client.DataAccess;
using CTrade.Client.Services.Entities;
using CTrade.Client.Services.Responses;
using System;
using System.Threading.Tasks;
using CTrade.Client.Core;
using CTrade.Client.DataAccess.Requests;
using CTrade.Client.DataAccess.Responses;

namespace CTrade.Client.Services
{
    public interface IPageContentService
    {
        Task<IPageContentHeaderResponse> CreateAsync(PageContent pageContent);
        Task<IPageContentHeaderResponse> EditAsync(PageContent pageContent);
        Task<IPageContentResponse> GetAsync(string siteId, string pageId);
        Task<IPageContentListResponse> GetForSiteAsync(string siteId);
    }
    public class PageContentService : ServiceBase, IPageContentService
    {
        public PageContentService(IDbRepository dbRepository, ILogger logger): base(dbRepository, logger) { }

        public async Task<IPageContentHeaderResponse> CreateAsync(PageContent pageContent)
        {
            pageContent.NotNull();
            IPageContentHeaderResponse createResponse = null;
            string errorMessage = null;
            if (!ValidForSave(pageContent, false, out errorMessage))
                createResponse = new PageContentHeaderResponse(errorMessage);
            else
            {
                try
                {
                    var response = await Repository.CreateAsync(pageContent.AsJObject());
                    createResponse = new PageContentHeaderResponse(response);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }

            return createResponse;
        }

        public async Task<IPageContentHeaderResponse> EditAsync(PageContent pageContent)
        {
            pageContent.NotNull();
            IPageContentHeaderResponse editResponse = null;
            string errorMessage = null;
            if (!ValidForSave(pageContent, true, out errorMessage))
                editResponse = new PageContentHeaderResponse(errorMessage);
            else
            {
                try
                {
                    var getResponse = await GetAsync(pageContent.SiteId, pageContent.Id);
                    if (!getResponse.HasError)
                    {
                        if (getResponse.IsEmpty)
                            editResponse = new PageContentHeaderResponse(ErrorMessage.PageNotFound);
                        else
                        {
                            pageContent.Rev = getResponse.PageContent.Rev;
                            var response = await Repository.UpdateAsync(pageContent.Id,
                                pageContent.Rev, pageContent.AsJObject());
                            editResponse = new PageContentHeaderResponse(response);
                        }
                    }
                    else
                        editResponse = new PageContentHeaderResponse(getResponse.Error);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }

            return editResponse;
        }

        public async Task<IPageContentResponse> GetAsync(string siteId, string pageId)
        {
            IPageContentResponse getResponse = null;
            if (string.IsNullOrWhiteSpace(siteId))
                getResponse = new PageContentResponse(ErrorMessage.SiteIdIsMandatory);
            else if(string.IsNullOrWhiteSpace(pageId))
                getResponse = new PageContentResponse(ErrorMessage.PageIdIsMandatory);
            else
            {
                var searchRequest = SearchRequestFactory.Create(Index.DesignDoc, Index.PageContents);
                searchRequest.Configure(q => q.IncludeDocs(true)
                    .Expression(string.Format(Index.PageContentSearchExpressionFormat, pageId, siteId))
                    );
                try
                {
                    var searchResponse = await Repository.SearchAsync(searchRequest);
                    getResponse = new PageContentResponse(searchResponse);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
            
            return getResponse;
        }

        public async Task<IPageContentListResponse> GetForSiteAsync(string siteId)
        {
            IPageContentListResponse getForSiteResponse = null;

            if (string.IsNullOrWhiteSpace(siteId))
                getForSiteResponse = new PageContentListResponse(ErrorMessage.SiteIdIsMandatory);
            else
            {
                var searchRequest = SearchRequestFactory.Create(Index.DesignDoc, Index.PageContents);
                searchRequest.Configure(q => q.IncludeDocs(true)
                    .Expression(string.Format(Index.MatchSiteExpressionFormat, siteId))
                    );
                try
                {
                    var searchResponse = await Repository.SearchAsync(searchRequest);
                    getForSiteResponse = new PageContentListResponse(searchResponse);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }

            return getForSiteResponse;
        }

        #region Private Helpers
        private bool ValidForSave(PageContent pageContent, bool isEdit, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!isEdit && !string.IsNullOrWhiteSpace(pageContent.Id))
                errorMessage = ErrorMessage.PageIdCannotBeSpecifiedForCreate;
            else if (isEdit && string.IsNullOrWhiteSpace(pageContent.Id))
                errorMessage = ErrorMessage.PageIdIsMandatory;
            else if (string.IsNullOrWhiteSpace(pageContent.SiteId))
                errorMessage = ErrorMessage.SiteIdIsMandatory;
            else if (string.IsNullOrWhiteSpace(pageContent.Title))
                errorMessage = ErrorMessage.TitleIsMandatory;
            else if (string.IsNullOrWhiteSpace(pageContent.Content))
                errorMessage = ErrorMessage.ContentIsMandatory;

            return string.IsNullOrWhiteSpace(errorMessage);
        }

        private async Task<SearchResponse> SearchAsync(string searchExpression)
        {
            var searchRequest = SearchRequestFactory.Create(Index.DesignDoc, Index.PageContents);
            searchRequest.Configure(q => q.IncludeDocs(true)
                    .Expression(searchExpression)
                    );
            return await Repository.SearchAsync(searchRequest);
        }
        #endregion
    }
}
