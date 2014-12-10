using System;

namespace CTrade.Client.DataAccess
{
    internal static class AssertExtension
    {
        internal static void IsNotNullOrWhiteSpace(this string parameter)
        {
            const string _nonEmptyParameterMessageFormat = "{0} cannot be empty.";
            if (string.IsNullOrWhiteSpace(parameter))
                throw new ArgumentNullException(parameter, string.Format(_nonEmptyParameterMessageFormat, parameter));
        }
    }
}
