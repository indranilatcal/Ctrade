using MyCouch.Cloudant.Requests;
using System;

namespace CTrade.Client.DataAccess.Requests
{
	public static class SearchRequestFactory
	{
		private const string _searchRequiresDocAndIndex = "A search requires a deign document name and an index name";
		public static SearchIndexRequest Create(string designDoc, string searchIndexName)
		{
			if (string.IsNullOrWhiteSpace(designDoc) || string.IsNullOrWhiteSpace(searchIndexName))
				throw new Exception(_searchRequiresDocAndIndex);

			return new SearchIndexRequest(designDoc, searchIndexName);
		}
	}
}
