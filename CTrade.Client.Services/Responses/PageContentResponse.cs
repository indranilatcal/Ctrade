using CTrade.Client.Core;
using CTrade.Client.DataAccess.Responses;
using CTrade.Client.Services.Entities;
using System.Linq;

namespace CTrade.Client.Services.Responses
{
    public interface IPageContentResponse : IServiceResponse
    {
        bool IsEmpty { get; }
        PageContent PageContent { get; }
    }
    public class PageContentResponse : IPageContentResponse
    {
        private string _errorMessage;
        private PageContent _pageContent;

        internal PageContentResponse(string errorMessage)
        {
            errorMessage.NotNullOrWhiteSpace();

            _errorMessage = errorMessage;
        }

        internal PageContentResponse(SearchResponse searchResponse)
        {
            searchResponse.NotNull();

            SetPageContent(searchResponse);
        }

        private void SetPageContent(SearchResponse searchResponse)
        {
            if (!searchResponse.HttpHeaderInfo.HasError && searchResponse.RowCount > 0)
            {
                if (searchResponse.RowCount > 1)
                    _errorMessage = ErrorMessage.MoreThanOnePageExist;
                else
                    _pageContent = searchResponse.Rows.Single().IncludedDoc.AsPageContent();
            }
            else
                _errorMessage = searchResponse.HttpHeaderInfo.Error;
        }

        public bool HasError
        {
            get { return !string.IsNullOrWhiteSpace(_errorMessage); }
        }

        public string Error
        {
            get { return _errorMessage; }
        }

        public PageContent PageContent { get { return _pageContent; } }
        public bool IsEmpty { get { return _pageContent == null; } }
    }
}
