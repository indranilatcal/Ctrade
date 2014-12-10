using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrade.Client.Services
{
    internal static class ErrorMessage
    {
        internal const string SiteIdIsMandatory = "Site id is mandatory.";
        internal const string QuestionTextIsMandatory = "Question text is mandatory.";
        internal const string AnswerTextIsMandatory = "Answer text is mandatory.";
        internal const string QuestionIdIsMandatory = "Question id is mandatory.";
        internal const string QuestionIdNotFoundFormat = "Question with id: {0} not found.";
        internal const string ServiceError = "There was an error in processing.";
        internal const string ItemsToOverrideShouldNotBeEmpty = "There should be some items to override for Override operation.";
        internal const string PromotionTextIsMandatory = "Promotion text is mandatory.";
        internal const string DatesMandatoryInAbsenceOfActivate = "Should have start and end dates when activate is not specified.";
        internal const string EndDateShouldSucceedStartDate = "End date should succeed start date.";
        internal const string EitherActivateOrDatesShouldBeSpecified = "Either activate or dates should be specified.";
        internal const string ActivePromotionShouldHaveText = "Active promotion should have text.";
        internal const string PromotionShouldEitherBeActiveOrHaveDates = "Promotion should either be active or have both the dates.";
        internal const string TitleIsMandatory = "Title is mandatory.";
        internal const string ContentIsMandatory = "Content is mandatory.";
        internal const string PageIdCannotBeSpecifiedForCreate = "Page id cannot be specified for create.";
        internal const string PageIdIsMandatory = "Page id is mandatory.";
        internal const string MoreThanOnePageExist = "More than on page exist.";
        internal const string PageNotFound = "Page not found";
    }
}
