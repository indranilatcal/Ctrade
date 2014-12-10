using MyCouch.Responses;
using System;
using System.Net;
using System.Net.Http;

namespace CTrade.Client.DataAccess.Responses
{
	public interface IHttpHeaderInfo
	{
		HttpStatusCode StatusCode { get; }
		Uri RequestUri { get; }
		HttpMethod RequestMethod { get; }
		string ContentType { get; }
		long? ContentLength { get; }
		string Error { get; }
		string Reason { get; }
		bool HasError { get; }
	}

	public class HttpHeaderInfo : IHttpHeaderInfo
	{
		private Response _response;
		internal HttpHeaderInfo(Response response)
		{
			_response = response;
		}

		public HttpStatusCode StatusCode
		{
			get { return _response.StatusCode; }
		}

		public Uri RequestUri
		{
			get { return _response.RequestUri; }
		}

		public HttpMethod RequestMethod
		{
			get { return _response.RequestMethod; }
		}

		public string ContentType
		{
			get { return _response.ContentType; }
		}

		public long? ContentLength
		{
			get { return _response.ContentLength; }
		}

		public string Error
		{
			get { return _response.Error; }
		}

		public string Reason
		{
			get { return _response.Reason; }
		}

		public bool HasError
		{
			//get { return (int)StatusCode >= 200 && (int)StatusCode < 300; }
			get { return !_response.IsSuccess; }
		}
	}
}
