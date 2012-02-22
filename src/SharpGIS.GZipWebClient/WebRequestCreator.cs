// (c) Copyright Morten Nielsen.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Net;

namespace SharpGIS
{
	/// <summary>
	/// Provides objects for specifying whether the to use the SharpGIS.GZIP handler
	///  for handling  HTTP requests and responses.
	/// </summary>
	/// <remarks>
	/// To enable gzip compression on all webclient requests, call the following method at app startup:
	/// WebRequest.RegisterPrefix("http://", SharpGIS.WebRequestCreator.GZip);
	/// </remarks>
	public static class WebRequestCreator
	{
		private static GZipClientHttpRequestCreator gzipCreator;

		public static IWebRequestCreate GZip
		{
			get
			{
				if (gzipCreator == null)
				{
					gzipCreator = new GZipClientHttpRequestCreator();
				}
				return gzipCreator;
			}
		}

		private class GZipClientHttpRequestCreator : IWebRequestCreate
		{
			public WebRequest Create(Uri uri)
			{
				return new GZipHttpWebRequest(uri);
			}
		}
	}
}
