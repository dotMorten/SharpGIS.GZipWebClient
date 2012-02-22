// (c) Copyright Morten Nielsen.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Net;
using System.Security;
using System.IO;
using System.Linq;

namespace SharpGIS
{
	/// <summary>
	/// This is an explicit web client class for doing webrequests.
	/// If you want to opt in for gzip support on all existing WebClients, consider
	/// using the <see cref="WebRequestCreator"/>
	/// </summary>
	public class GZipWebClient : WebClient
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GZipWebClient"/> class.
		/// </summary>
		[SecuritySafeCritical]
		public GZipWebClient()
		{
		}
		/// <summary>
		/// Returns a <see cref="T:System.Net.WebRequest"/> object for the specified resource.
		/// </summary>
		/// <param name="address">A <see cref="T:System.Uri"/> that identifies the resource to request.</param>
		/// <returns>
		/// A new <see cref="T:System.Net.WebRequest"/> object for the specified resource.
		/// </returns>
		protected override WebRequest GetWebRequest(Uri address)
		{
			var req = base.GetWebRequest(address);
			req.Headers[HttpRequestHeader.AcceptEncoding] = "gzip"; //Set GZIP header
			return req;
		}
		/// <summary>
		/// Returns the <see cref="T:System.Net.WebResponse"/> for the specified <see cref="T:System.Net.WebRequest"/> using the specified <see cref="T:System.IAsyncResult"/>.
		/// </summary>
		/// <param name="request">A <see cref="T:System.Net.WebRequest"/> that is used to obtain the response.</param>
		/// <param name="result">An <see cref="T:System.IAsyncResult"/> object obtained from a previous call to <see cref="M:System.Net.WebRequest.BeginGetResponse(System.AsyncCallback,System.Object)"/> .</param>
		/// <returns>
		/// A <see cref="T:System.Net.WebResponse"/> containing the response for the specified <see cref="T:System.Net.WebRequest"/>.
		/// </returns>
		protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			WebResponse response = null;
			try
			{
				response = base.GetWebResponse(request, result);
				if(!(response is GZipWebResponse) && //this would be the case if WebRequestCreator was also used
				 (response.Headers[HttpRequestHeader.ContentEncoding] == "gzip"))
					return new GZipWebResponse(response); //If gzipped response, uncompress
				else
					return response;
			}
			catch
			{
				return null;
			}
		}
		internal class GZipWebResponse : WebResponse
		{
			WebResponse response;
			internal GZipWebResponse(WebResponse resp)
			{
				response = resp;
			}

			public override System.IO.Stream GetResponseStream()
			{
				return new SharpGIS.ZLib.GZipStream(response.GetResponseStream());
			}
			public override void Close()
			{
				response.Close();
			}
			public override long ContentLength
			{
				get { return response.ContentLength; }
			}
			public override string ContentType
			{
				get { return response.ContentType; }
			}
			public override WebHeaderCollection Headers
			{
				get { return response.Headers; }
			}
			public override Uri ResponseUri
			{
				get { return response.ResponseUri; }
			}
			public override bool SupportsHeaders
			{
				get { return response.SupportsHeaders; }
			}
		}
	}
}
