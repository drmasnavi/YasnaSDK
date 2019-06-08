using System;
using System.Collections.Generic;

namespace SmsPanelSms.Restful
{
	internal abstract class HttpClient : IHttpRequest
	{
		protected HttpClient()
		{
		}

		public string SendRequest(HttpObject httpObject, IDictionary<string, string> parameters)
		{
			throw new NotImplementedException();
		}
	}
}