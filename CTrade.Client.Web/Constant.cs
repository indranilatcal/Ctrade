using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTrade.Client.Web
{
    static class Environment
    {
        internal const string Master = "Master";
        internal const string FAQ = "FAQ";
        internal const string Categories = "Categories";
        internal const string Promotions = "Promotions";
        internal const string DynamicPages = "DynamicPages";
    }

    static class ErrorMessage
    {
        internal const string QuestionRetrieval = "Error in retrieving questions!";
        internal const string PageContentRetrieval = "Error in retrieving page contents!";
        internal const string ErrorCreatingAQuestion = "An errr occured while creating the question!";
        internal const string CategoryRetrieval = "Error in retrieving categories!";
        internal const string QuestionUpdateForSiteError = "Error in updating questions for site!";
        internal const string CategoryUpdateForSiteError = "Error in updating categores for site!";
        internal const string OverrideUpdateForSiteError = "Error in overriding labels for site!";
        internal const string PromotionRetrieval = "Error in retrieving promotion for site!";
        internal const string PromotionUpdate = "Error in updating promotion for site!";
    }
}