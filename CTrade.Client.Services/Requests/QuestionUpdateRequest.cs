
using CTrade.Client.Core;

namespace CTrade.Client.Services.Requests
{
    public enum UpdateType
    {
        Add,
        Delete
    }
    public class QuestionUpdateRequest
    {
        internal string QuestionId { get; private set; }
        internal string SiteId { get; private set; }
        internal UpdateType UpdateType { get; private set; }
        public QuestionUpdateRequest(string questionId, string siteId, UpdateType updateType)
        {
            questionId.NotNullOrWhiteSpace();
            siteId.NotNullOrWhiteSpace();

            QuestionId = questionId;
            SiteId = siteId;
            UpdateType = updateType;
        }
    }
}
