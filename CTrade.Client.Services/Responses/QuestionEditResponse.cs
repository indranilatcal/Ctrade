using CTrade.Client.DataAccess.Responses;
using CTrade.Client.Core;

namespace CTrade.Client.Services.Responses
{
    public interface IQuestionEditResponse : IServiceResponse
    {

    }
    public class QuestionEditResponse : IQuestionEditResponse
    {
        private string _error;

        internal QuestionEditResponse(IDbDocHeaderResponse docHeaderResponse)
        {
            docHeaderResponse.NotNull();
            _error = docHeaderResponse.HttpHeaderInfo.Error;
        }

        internal QuestionEditResponse(string errorMessage)
        {
            _error = errorMessage;
        }

        public bool HasError
        {
            get { return !string.IsNullOrWhiteSpace(_error); }
        }

        public string Error
        {
            get { return _error; }
        }
    }
}
