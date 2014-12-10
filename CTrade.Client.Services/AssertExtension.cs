using System;
using System.Collections.Generic;
using System.Linq;

namespace CTrade.Client.Services.Assertions
{
    internal static class AssertExtensions
    {
        private const string _nonEmptyParameterMessage = "parameter cannot be empty.";
        internal static void NotNullOrWhiteSpace(this string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
                throw new ArgumentNullException(_nonEmptyParameterMessage);
        }

        internal static void HasItems<T>(this IEnumerable<T> parameter)
        {
            if(parameter == null || !parameter.Any())
                throw new ArgumentNullException(_nonEmptyParameterMessage);
        }

        internal static void NotNull<T>(this T parameter) where T : class
        {
            if (parameter == null)
                throw new ArgumentNullException(_nonEmptyParameterMessage);
        }
    }
}
