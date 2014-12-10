using System.Collections.Generic;

namespace CTrade.Client.Web.Areas.Admin.Models
{
    public class ManageQuestionsForSiteViewModel
    {
        public string SiteId { get; set; }
        public IList<QuestionAssociationViewModel> Questions { get; set; }
    }
}