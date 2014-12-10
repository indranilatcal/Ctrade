
using CTrade.Client.DataAccess.Responses;
using CTrade.Client.Services.Entities;
using CTrade.Client.Core;
using System.Linq;
using System.Collections.Generic;

namespace CTrade.Client.Services.Responses
{
    public interface ICategoryResponse : IServiceResponse
    {
        Category[] Categories { get; }
        bool IsEmpty { get; }
    }

    internal class CategoryResponse : ICategoryResponse
    {
        private readonly Category[] _categories;
        private string _errorMessage;

        internal CategoryResponse(SearchResponse searchResponse)
        {
            searchResponse.NotNull();

            _errorMessage = searchResponse.HttpHeaderInfo.Error;
            _categories = searchResponse.Rows.Select(r => r.IncludedDoc.AsCategory()).ToArray();
        }

        internal CategoryResponse(string errorMessage)
        {
            errorMessage.NotNullOrWhiteSpace();

            _categories = new Category[] { };
            _errorMessage = errorMessage;
        }

        internal CategoryResponse(IEnumerable<Category> categories)
        {
            categories.NotNull();

            _errorMessage = string.Empty;
            _categories = categories.ToArray();
        }

        public Category[] Categories { get { return _categories; } }
        public string Error { get { return _errorMessage; } }
        public bool HasError { get { return !string.IsNullOrWhiteSpace(_errorMessage); } }
        public bool IsEmpty
        {
            get { return _categories == null || _categories.Length == 0; }
        }
    }

    internal class CategoryHeaderResponse : IServiceResponse
    {
        private readonly string _errorMessage;

        internal CategoryHeaderResponse(): this(string.Empty) { }
        internal CategoryHeaderResponse(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public string Error { get { return _errorMessage; } }
        public bool HasError { get { return !string.IsNullOrWhiteSpace(_errorMessage); } }
    }
}
