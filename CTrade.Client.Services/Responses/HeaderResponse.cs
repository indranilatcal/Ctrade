
using CTrade.Client.DataAccess.Responses;
namespace CTrade.Client.Services.Responses
{

    public interface IHeaderResponse : IServiceResponse
    {
        string QuestionId { get; }
    }

    public class HeaderResponse : IHeaderResponse
    {
        private readonly string _questionId;
        private readonly string _error;

        internal HeaderResponse(IDbDocHeaderResponse response)
        {
            _questionId = response.Id ?? string.Empty;
            _error = response.HttpHeaderInfo.Error;
        }
        internal HeaderResponse(string errorMessage)
        {
            _questionId = string.Empty;
            _error = errorMessage;
        }

        public string QuestionId { get { return _questionId; } }
        public string Error { get { return _error; } }
        public bool HasError { get { return !string.IsNullOrWhiteSpace(_error); } }
    }
}
