using System;
using System.Collections.Generic;

namespace SmsPanelSms.Restful
{
	public class VerificationCode
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
				lock (VerificationCode.HttpRequestFactoryLock)
				{
					defaultFactory = VerificationCode._httpClientFactory ?? VerificationCode.DefaultFactory;
				}
				return defaultFactory;
			}
			set
			{
				lock (VerificationCode.HttpRequestFactoryLock)
				{
					VerificationCode._httpClientFactory = value;
				}
			}
		}

		static VerificationCode()
		{
			VerificationCode.CachedClient = new Lazy<IHttpRequest>(() => new HttpPostRequest());
			VerificationCode.DefaultFactory = () => VerificationCode.CachedClient.Value;
			VerificationCode.HttpRequestFactoryLock = new object();
		}

		public VerificationCode()
		{
		}

		public RestVerificationCodeRespone Send(string tokenKey, RestVerificationCode model)
		{
			RestVerificationCodeRespone restVerificationCodeRespone;
			try
			{
				string str = model.Serialize<RestVerificationCode>();
				string str1 = "http://restfulsms.com/api/VerificationCode";
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				RestVerificationCodeRespone restVerificationCodeRespone1 = VerificationCode.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str1,
					Json = str
				}, strs).Deserialize<RestVerificationCodeRespone>();
				if (restVerificationCodeRespone1 != null)
				{
					restVerificationCodeRespone = restVerificationCodeRespone1;
				}
				else
				{
					restVerificationCodeRespone = null;
				}
			}
			catch (Exception exception)
			{
				restVerificationCodeRespone = null;
			}
			return restVerificationCodeRespone;
		}
	}
}