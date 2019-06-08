using System;
using System.Collections.Generic;

namespace SmsPanelSms.Restful
{
	public class SmsLine
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
				lock (SmsLine.HttpRequestFactoryLock)
				{
					defaultFactory = SmsLine._httpClientFactory ?? SmsLine.DefaultFactory;
				}
				return defaultFactory;
			}
			set
			{
				lock (SmsLine.HttpRequestFactoryLock)
				{
					SmsLine._httpClientFactory = value;
				}
			}
		}

		static SmsLine()
		{
			SmsLine.CachedClient = new Lazy<IHttpRequest>(() => new HttpGetRequest());
			SmsLine.DefaultFactory = () => SmsLine.CachedClient.Value;
			SmsLine.HttpRequestFactoryLock = new object();
		}

		public SmsLine()
		{
		}

		public SmsLineNumber GetSmsLines(string tokenKey)
		{
			SmsLineNumber smsLineNumber;
			try
			{
				string str = "http://restfulsms.com/api/SMSLine";
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				SmsLineNumber smsLineNumber1 = SmsLine.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str
				}, strs).Deserialize<SmsLineNumber>();
				if (smsLineNumber1 != null)
				{
					smsLineNumber = smsLineNumber1;
				}
				else
				{
					smsLineNumber = null;
				}
			}
			catch (Exception exception)
			{
				smsLineNumber = null;
			}
			return smsLineNumber;
		}
	}
}