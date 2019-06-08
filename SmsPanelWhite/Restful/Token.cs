using System;

namespace SmsPanelSms.Restful
{
	public class Token
	{
		private readonly static Lazy<IHttpRequest> CachedClient;

		private readonly static Func<IHttpRequest> DefaultFactory;

		private static Func<IHttpRequest> _httpClientFactory;

		private readonly static object HttpRequestFactoryLock;

		internal static Func<IHttpRequest> HttpRequestFactory
		{
			get
			{
				Func<IHttpRequest> defaultFactory;
				lock (Token.HttpRequestFactoryLock)
				{
					defaultFactory = Token._httpClientFactory ?? Token.DefaultFactory;
				}
				return defaultFactory;
			}
			set
			{
				lock (Token.HttpRequestFactoryLock)
				{
					Token._httpClientFactory = value;
				}
			}
		}

		static Token()
		{
			Token.CachedClient = new Lazy<IHttpRequest>(() => new HttpPostRequest());
			Token.DefaultFactory = () => Token.CachedClient.Value;
			Token.HttpRequestFactoryLock = new object();
		}

		public Token()
		{
		}

		public string GetToken(string userApiKey, string secretKey)
		{
			string tokenKey;
			try
			{
				string str = (new TokenRequestObject()
				{
					UserApiKey = userApiKey,
					SecretKey = secretKey
				}).Serialize<TokenRequestObject>();
				string str1 = "http://restfulsms.com/api/Token";
				TokenResultObject tokenResultObject = Token.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str1,
					Json = str
				}, null).Deserialize<TokenResultObject>();
				if (tokenResultObject == null || !tokenResultObject.IsSuccessful)
				{
					return null;
				}
				else
				{
					tokenKey = tokenResultObject.TokenKey;
				}
			}
			catch (Exception exception)
			{
				tokenKey = null;
			}
			return tokenKey;
		}
	}
}