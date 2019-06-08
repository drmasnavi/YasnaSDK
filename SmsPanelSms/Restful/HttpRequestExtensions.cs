using System;
using System.Collections.Generic;

namespace SmsPanelSms.Restful
{
	internal static class HttpRequestExtensions
	{
		public static string Execute(this IHttpRequest client, HttpObject httpObject, IDictionary<string, string> parameters)
		{
			if (client == null)
			{
				throw new ArgumentNullException("client");
			}
			return client.SendRequest(httpObject, parameters);
		}
	}
}