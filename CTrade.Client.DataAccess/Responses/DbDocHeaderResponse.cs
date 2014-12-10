using MyCouch.Responses;
using System.Net;

namespace CTrade.Client.DataAccess.Responses
{
	public interface IDbDocHeaderResponse : IDbResponse
	{
		string Id { get; }
		string Revision { get; }
	}

	public class DbDocHeaderResponse : IDbDocHeaderResponse
	{
		private readonly DocumentHeaderResponse _docHeaderResponse;

		internal DbDocHeaderResponse(DocumentHeaderResponse docHeaderResponse)
		{
			_docHeaderResponse = docHeaderResponse;
		}

        internal DbDocHeaderResponse(string error,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : this(new DocumentHeaderResponse { StatusCode = statusCode, Error = error })
        { }

		public string Id
		{
			get { return _docHeaderResponse.Id; }
		}

		public string Revision
		{
			get { return _docHeaderResponse.Rev; }
		}

		public IHttpHeaderInfo HttpHeaderInfo
		{
			get { return new HttpHeaderInfo(_docHeaderResponse); }
		}
	}
}
