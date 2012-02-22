// (c) Copyright Morten Nielsen.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.
using System;
using System.Net;
using System.Threading;

namespace SharpGIS
{
	/// <summary>
	/// This is really just a wrapper class for HttpWebRequest that adds the gzip header,
	/// and checks the response for gzip. If it's gzip'ed, it will uncompress the stream.
	/// </summary>
	/// <remarks>
	/// This class is only used by the <see cref="WebRequestCreator"/>.
	/// </remarks>
	internal class GZipHttpWebRequest : HttpWebRequest
	{
		private System.Net.WebRequest internalWebRequest;

		public GZipHttpWebRequest(Uri uri)
		{
			internalWebRequest = System.Net.WebRequest.CreateHttp(uri);
			Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
		}

		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			return internalWebRequest.BeginGetRequestStream(callback, state);
		}

		public override System.IO.Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			return internalWebRequest.EndGetRequestStream(asyncResult);
		}

		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			return internalWebRequest.BeginGetResponse(callback, state);
		}

		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			var response = internalWebRequest.EndGetResponse(asyncResult);
			if (response.Headers[HttpRequestHeader.ContentEncoding] == "gzip")
				return new SharpGIS.GZipWebClient.GZipWebResponse(response);
			else
				return response;
		}

		public override string Method
		{
			get { return internalWebRequest.Method; }
			set { internalWebRequest.Method = value; }
		}

		public override string ContentType
		{
			get { return internalWebRequest.ContentType; }
			set { internalWebRequest.ContentType = value; }
		}

		public override ICredentials Credentials
		{
			get { return internalWebRequest.Credentials; }
			set { internalWebRequest.Credentials = value; }
		}

		public override WebHeaderCollection Headers
		{
			get { return internalWebRequest.Headers; }
			set { internalWebRequest.Headers = value; }
		}

		public override void Abort()
		{
			internalWebRequest.Abort();
		}

		public override bool UseDefaultCredentials
		{
			get { return internalWebRequest.UseDefaultCredentials; }
			set { internalWebRequest.UseDefaultCredentials = value; }
		}

		public override Uri RequestUri
		{
			get { return internalWebRequest.RequestUri; }
		}
	}
}
