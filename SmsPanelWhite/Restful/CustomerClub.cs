using System;
using System.Collections.Generic;

namespace SmsPanelSms.Restful
{
	public class CustomerClub
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
				lock (CustomerClub.HttpRequestFactoryLock)
				{
					defaultFactory = CustomerClub._httpClientFactory ?? CustomerClub.DefaultFactory;
				}
				return defaultFactory;
			}
			set
			{
				lock (CustomerClub.HttpRequestFactoryLock)
				{
					CustomerClub._httpClientFactory = value;
				}
			}
		}

		static CustomerClub()
		{
			CustomerClub.CachedClient = new Lazy<IHttpRequest>(() => new HttpPostRequest());
			CustomerClub.DefaultFactory = () => CustomerClub.CachedClient.Value;
			CustomerClub.HttpRequestFactoryLock = new object();
		}

		public CustomerClub()
		{
		}

		public CustomerClubSendResponse AddContactAndSend(string tokenKey, CustomerClubInsertAndSendMessage[] model)
		{
			CustomerClubSendResponse customerClubSendResponse;
			try
			{
				string str = model.Serialize<CustomerClubInsertAndSendMessage[]>();
				string str1 = "http://restfulsms.com/api/CustomerClub/AddContactAndSend";
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				CustomerClubSendResponse customerClubSendResponse1 = CustomerClub.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str1,
					Json = str
				}, strs).Deserialize<CustomerClubSendResponse>();
				if (customerClubSendResponse1 != null)
				{
					customerClubSendResponse = customerClubSendResponse1;
				}
				else
				{
					customerClubSendResponse = null;
				}
			}
			catch (Exception exception)
			{
				customerClubSendResponse = null;
			}
			return customerClubSendResponse;
		}

		public CustomerClubSendResponse Send(string tokenKey, CustomerClubSend model)
		{
			CustomerClubSendResponse customerClubSendResponse;
			try
			{
				string str = model.Serialize<CustomerClubSend>();
				string str1 = "http://restfulsms.com/api/CustomerClub/Send";
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				CustomerClubSendResponse customerClubSendResponse1 = CustomerClub.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str1,
					Json = str
				}, strs).Deserialize<CustomerClubSendResponse>();
				if (customerClubSendResponse1 != null)
				{
					customerClubSendResponse = customerClubSendResponse1;
				}
				else
				{
					customerClubSendResponse = null;
				}
			}
			catch (Exception exception)
			{
				customerClubSendResponse = null;
			}
			return customerClubSendResponse;
		}

		public CustomerClubSendResponse SendToCategories(string tokenKey, CustomerClubSendToCategories model)
		{
			CustomerClubSendResponse customerClubSendResponse;
			try
			{
				string str = model.Serialize<CustomerClubSendToCategories>();
				string str1 = "http://restfulsms.com/api/CustomerClub/SendToCategories";
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				CustomerClubSendResponse customerClubSendResponse1 = CustomerClub.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str1,
					Json = str
				}, strs).Deserialize<CustomerClubSendResponse>();
				if (customerClubSendResponse1 != null)
				{
					customerClubSendResponse = customerClubSendResponse1;
				}
				else
				{
					customerClubSendResponse = null;
				}
			}
			catch (Exception exception)
			{
				customerClubSendResponse = null;
			}
			return customerClubSendResponse;
		}
	}
}