using System;
using System.Collections.Generic;
using System.Linq;

namespace CTrade.Client.Core
{
    public static class AssertExtensions
    {
        private const string _nonEmptyParameterMessage = "parameter cannot be empty.";
        public static void NotNullOrWhiteSpace(this string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
                throw new ArgumentNullException(_nonEmptyParameterMessage);
        }

        public static void HasItems<T>(this IEnumerable<T> parameter)
        {
            if (parameter == null || !parameter.Any())
                throw new ArgumentNullException(_nonEmptyParameterMessage);
        }

        public static void NotNull<T>(this T parameter) where T : class
        {
            if (parameter == null)
                throw new ArgumentNullException(_nonEmptyParameterMessage);
        }
    }
}
