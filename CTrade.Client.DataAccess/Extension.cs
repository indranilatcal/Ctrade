using Newtonsoft.Json.Linq;
using System;

namespace CTrade.Client.DataAccess
{
	internal static class Extension
	{
		private const string _isEmptyPropertyName = "IsEmpty";
		internal static bool IsBlank(this JObject data)
		{
			JToken emptyValue;
			return data != null && data.Count == 1 &&
				data.TryGetValue(_isEmptyPropertyName, StringComparison.OrdinalIgnoreCase, out emptyValue)
				&& emptyValue.Type == JTokenType.Boolean;
		}

		internal static bool IsEmpty(this JObject data)
		{
			return data == null || data.Count == 0;
		}
	}
}
