using System;
using System.Collections.Generic;

namespace SmsPanelSms.Restful
{
	public class Credit
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
				lock (Credit.HttpRequestFactoryLock)
				{
					defaultFactory = Credit._httpClientFactory ?? Credit.DefaultFactory;
				}
				return defaultFactory;
			}
			set
			{
				lock (Credit.HttpRequestFactoryLock)
				{
					Credit._httpClientFactory = value;
				}
			}
		}

		static Credit()
		{
			Credit.CachedClient = new Lazy<IHttpRequest>(() => new HttpPostRequest());
			Credit.DefaultFactory = () => Credit.CachedClient.Value;
			Credit.HttpRequestFactoryLock = new object();
		}

		public Credit()
		{
		}

		public CreditResponse GetCredit(string tokenKey)
		{
			CreditResponse creditResponse;
			try
			{
				string str = "http://restfulsms.com/api/credit";
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				Credit.HttpRequestFactory = () => new HttpGetRequest();
				CreditResponse creditResponse1 = Credit.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str
				}, strs).Deserialize<CreditResponse>();
				if (creditResponse1 == null || !creditResponse1.IsSuccessful)
				{
					return null;
				}
				else
				{
					creditResponse = creditResponse1;
				}
			}
			catch (Exception exception)
			{
				creditResponse = null;
			}
			return creditResponse;
		}
	}
}