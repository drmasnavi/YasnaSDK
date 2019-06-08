using System;
using System.Collections.Generic;

namespace SmsPanelSms.Restful
{
	public class UltraFast
	{
		private static Lazy<IHttpRequest> CachedClient;

		private static Func<IHttpRequest> DefaultFactory;

		private static Func<IHttpRequest> _httpClientFactory;

		private static object HttpRequestFactoryLock;

		internal static Func<IHttpRequest> HttpRequestFactory
		{
			get
			{
				Func<IHttpRequest> defaultFactory;
				lock (UltraFast.HttpRequestFactoryLock)
				{
					defaultFactory = UltraFast._httpClientFactory ?? UltraFast.DefaultFactory;
				}
				return defaultFactory;
			}
			set
			{
				lock (UltraFast.HttpRequestFactoryLock)
				{
					UltraFast._httpClientFactory = value;
				}
			}
		}

		static UltraFast()
		{
			UltraFast.CachedClient = new Lazy<IHttpRequest>(() => new HttpPostRequest());
			UltraFast.DefaultFactory = () => UltraFast.CachedClient.Value;
			UltraFast.HttpRequestFactoryLock = new object();
		}

		public UltraFast()
		{
		}

		public UltraFastSendRespone Send(string tokenKey, UltraFastSend model)
		{
			UltraFastSendRespone ultraFastSendRespone;
			try
			{
				string str = model.Serialize<UltraFastSend>();
				string str1 = "http://restfulsms.com/api/UltraFastSend";
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				UltraFastSendRespone ultraFastSendRespone1 = UltraFast.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str1,
					Json = str
				}, strs).Deserialize<UltraFastSendRespone>();
				if (ultraFastSendRespone1 != null)
				{
					ultraFastSendRespone = ultraFastSendRespone1;
				}
				else
				{
					ultraFastSendRespone = null;
				}
			}
			catch (Exception exception)
			{
				ultraFastSendRespone = null;
			}
			return ultraFastSendRespone;
		}
	}
}