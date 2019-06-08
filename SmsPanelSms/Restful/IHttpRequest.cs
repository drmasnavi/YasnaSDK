using System.Collections.Generic;

namespace SmsPanelSms.Restful
{
	internal interface IHttpRequest
	{
		string SendRequest(HttpObject httpObject, IDictionary<string, string> parameters);
	}
}