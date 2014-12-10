using CTrade.Client.Core;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace CTrade.Client.Services.Entities
{
    public class Question : EntityBase
    {
        private string[] _sites;
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
        public string[] Sites
        {
            get { return _sites ?? new string[] { }; }
            set { _sites = value; }
        }
        public bool HasSites { get { return Sites != null && Sites.Length > 0; } }
    }

    internal static class QuestionExtensions
    {
        internal static JObject AsJObject(this Question question)
        {
            question.NotNull();

            dynamic doc = new JObject();
            if (!string.IsNullOrWhiteSpace(question.Id))
                doc._id = question.Id;
            if (!string.IsNullOrWhiteSpace(question.Rev))
                doc._rev = question.Rev;
            doc.questionText = question.QuestionText;
            doc.answerText = question.AnswerText;
            doc.docType = DocType.Faq;
            if(question.Sites.Any())
                doc.sites = new JArray(question.Sites);

            return doc;
        }

        internal static Question AsQuestion(this JObject doc)
        {
            doc.NotNull();

            dynamic dynamicDoc = doc;
            Question question = new Question
            {
                Id = dynamicDoc._id,
                QuestionText = dynamicDoc.questionText,
                AnswerText = dynamicDoc.answerText,
                DocType = dynamicDoc.docType
            };

            JToken rev;
            if (doc.TryGetValue("_rev", out rev))
                question.Rev = rev.Value<string>();

            JToken sites;
            if (doc.TryGetValue("sites", out sites))
                question.Sites = sites.Values<string>().ToArray();

            return question;
        }
    }
}
