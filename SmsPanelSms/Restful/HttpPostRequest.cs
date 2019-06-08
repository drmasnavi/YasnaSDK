using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SmsPanelSms.Restful
{
	internal class HttpPostRequest : IHttpRequest
	{
		public HttpPostRequest()
		{
		}

		public string SendRequest(HttpObject httpObject, IDictionary<string, string> parameters)
		{
			HttpWebRequest str = (HttpWebRequest)WebRequest.Create(httpObject.Url);
			str.ContentType = "application/json";
			str.Method = EnumHttpMethod.Post.ToString();
			if (parameters != null)
			{
				foreach (KeyValuePair<string, string> parameter in parameters)
				{
					str.Headers.Add(parameter.Key, parameter.Value);
				}
			}
			string end = null;
			using (StreamWriter streamWriter = new StreamWriter(str.GetRequestStream()))
			{
				streamWriter.Write(httpObject.Json);
				streamWriter.Flush();
				streamWriter.Close();
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(((HttpWebResponse)str.GetResponse()).GetResponseStream()))
				{
					end = streamReader.ReadToEnd();
				}
			}
			catch (WebException webException)
			{
				using (StreamReader streamReader1 = new StreamReader(webException.Response.GetResponseStream()))
				{
					end = streamReader1.ReadToEnd();
				}
			}
			return end;
		}
	}
}