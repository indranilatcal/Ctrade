using CTrade.Client.DataAccess.Responses;
using CTrade.Client.Core;
using CTrade.Client.Services.Entities;
using System.Linq;

namespace CTrade.Client.Services.Responses
{
    public interface IQuestionResponse : IServiceResponse
    {
        Question[] Questions { get; }
        bool IsEmpty { get; }
    }

    public class QuestionResponse : IQuestionResponse
    {
        private readonly Question[] _questions;
        private string _errorMessage;
        internal QuestionResponse(SearchResponse searchResponse)
        {
            searchResponse.NotNull();

            _errorMessage = searchResponse.HttpHeaderInfo.Error;
            _questions = searchResponse.Rows.Select(r => r.IncludedDoc.AsQuestion()).ToArray();
        }

        internal QuestionResponse(string errorMessage)
        {
            _questions = new Question[] { };
            _errorMessage = errorMessage;
        }

        internal QuestionResponse(IDbDocResponse getResponse)
        {
            getResponse.NotNull();

            _errorMessage = getResponse.HttpHeaderInfo.Error;
            if (getResponse.Data != null)
                _questions = new[] { getResponse.Data.AsQuestion() };
        }

        public string Error { get { return _errorMessage; } }
        public bool HasError { get { return !string.IsNullOrWhiteSpace(_errorMessage); } }

        public Question[] Questions
        {
            get { return _questions; }
        }

        public bool IsEmpty
        {
            get { return _questions == null || _questions.Length == 0; }
        }
    }
}
