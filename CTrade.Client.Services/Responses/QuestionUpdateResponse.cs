using CTrade.Client.DataAccess.Responses;
using CTrade.Client.Core;
using System.Collections.Generic;
using System.Linq;

namespace CTrade.Client.Services.Responses
{
    public interface IQuestionUpdateResponse
    {
        string[] Errors { get; }
        bool HasErrors { get; }
        void AddErrors(params string[] errorMessages);
        void ProcessResponse(BulkResponse bulkResponse);
    }
    public class QuestionUpdateResponse : IQuestionUpdateResponse
    {
        private IList<string> _errors;

        public QuestionUpdateResponse()
        {
            _errors = new List<string>();
        }

        public void ProcessResponse(BulkResponse bulkResponse)
        {
            bulkResponse.NotNull();
            if (bulkResponse.HttpHeaderInfo.HasError)
                _errors.Add(bulkResponse.HttpHeaderInfo.Error);
        }

        public void AddErrors(params string[] errorMessages)
        {
            errorMessages.HasItems();
            _errors = errorMessages.ToList();
        }

        public bool HasErrors
        {
            get { return _errors.Any(); }
        }

        public string[] Errors
        {
            get { return _errors.ToArray(); }
        }
    }
}
