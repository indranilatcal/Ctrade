using CTrade.Client.Core;
using Newtonsoft.Json.Linq;
using System;

namespace CTrade.Client.Services.Entities
{
    public class Promotion : EntityBase
    {
        public string PromotionText { get; set; }
        public bool Activate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    internal static class PromotionExtensions
    {
        internal static JObject AsJObject(this Promotion promotion)
        {
            promotion.NotNull();

            dynamic doc = new JObject();
            doc._id = promotion.Id;
            if (!string.IsNullOrWhiteSpace(promotion.Rev))
                doc._rev = promotion.Rev;
            doc.promotionText = promotion.PromotionText;
            doc.activate = promotion.Activate;
            if (promotion.StartDate.HasValue)
                doc.startDate = promotion.StartDate.Value;
            if (promotion.EndDate.HasValue)
                doc.endDate = promotion.EndDate.Value;
            doc.docType = DocType.Promotion;

            return doc;
        }

        internal static Promotion AsPromotion(this JObject doc)
        {
            doc.NotNull();

            dynamic dynamicDoc = doc;
            var promotion = new Promotion
                {
                    Id = dynamicDoc._id,
                    PromotionText = dynamicDoc.promotionText,
                    DocType = dynamicDoc.docType
                };
            
            JToken rev;
            if (doc.TryGetValue("_rev", out rev))
                promotion.Rev = rev.Value<string>();
            JToken activate;
            if (doc.TryGetValue("activate", out activate))
                promotion.Activate = activate.Value<bool>();
            JToken startDate;
            if (doc.TryGetValue("startDate", out startDate))
                promotion.StartDate = startDate.Value<DateTime>();
            JToken endDate;
            if (doc.TryGetValue("endDate", out endDate))
                promotion.EndDate = endDate.Value<DateTime>();

            return promotion;
        }
    }
}
