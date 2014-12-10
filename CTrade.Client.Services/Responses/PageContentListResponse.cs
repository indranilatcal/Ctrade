using CTrade.Client.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTrade.Client.Core;
using CTrade.Client.DataAccess.Responses;

namespace CTrade.Client.Services.Responses
{
    public interface IPageContentListResponse : IServiceResponse
    {
        bool IsEmpty { get; }
        PageContent[] PageContents { get; }
    }

    public class PageContentListResponse : IPageContentListResponse
    {
        private string _errorMessage;
        private PageContent[] _pageContents;

        internal PageContentListResponse(string errorMessage)
        {
            errorMessage.NotNullOrWhiteSpace();

            _errorMessage = errorMessage;
        }

        public PageContentListResponse(SearchResponse searchResponse)
        {
            searchResponse.NotNull();

            _errorMessage = searchResponse.HttpHeaderInfo.Error;
            _pageContents = searchResponse.Rows.Select(r => r.IncludedDoc.AsPageContent()).ToArray();
        }

        public bool IsEmpty
        {
            get { return _pageContents == null || _pageContents.Length == 0; }
        }

        public PageContent[] PageContents
        {
            get { return _pageContents; }
        }

        public bool HasError
        {
            get { return !string.IsNullOrWhiteSpace(_errorMessage); }
        }

        public string Error
        {
            get { return _errorMessage; }
        }
    }
}
