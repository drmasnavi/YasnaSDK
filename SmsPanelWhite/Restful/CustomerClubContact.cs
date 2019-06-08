using System;
using System.Collections.Generic;

namespace SmsPanelSms.Restful
{
	public class CustomerClubContact
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
				lock (CustomerClubContact.HttpRequestFactoryLock)
				{
					defaultFactory = CustomerClubContact._httpClientFactory ?? CustomerClubContact.DefaultFactory;
				}
				return defaultFactory;
			}
			set
			{
				lock (CustomerClubContact.HttpRequestFactoryLock)
				{
					CustomerClubContact._httpClientFactory = value;
				}
			}
		}

		static CustomerClubContact()
		{
			CustomerClubContact.CachedClient = new Lazy<IHttpRequest>(() => new HttpPostRequest());
			CustomerClubContact.DefaultFactory = () => CustomerClubContact.CachedClient.Value;
			CustomerClubContact.HttpRequestFactoryLock = new object();
		}

		public CustomerClubContact()
		{
		}

		public CustomerClubContactResponse Create(string tokenKey, CustomerClubContactObject model)
		{
			CustomerClubContactResponse customerClubContactResponse;
			try
			{
				string str = model.Serialize<CustomerClubContactObject>();
				string str1 = "http://restfulsms.com/api/CustomerClubContact";
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				CustomerClubContact.HttpRequestFactory = () => new HttpPostRequest();
				CustomerClubContactResponse customerClubContactResponse1 = CustomerClubContact.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str1,
					Json = str
				}, strs).Deserialize<CustomerClubContactResponse>();
				if (customerClubContactResponse1 != null)
				{
					customerClubContactResponse = customerClubContactResponse1;
				}
				else
				{
					customerClubContactResponse = null;
				}
			}
			catch (Exception exception)
			{
				customerClubContactResponse = null;
			}
			return customerClubContactResponse;
		}

		public CustomerClubContactCategoryResponse GetCategories(string tokenKey)
		{
			CustomerClubContactCategoryResponse customerClubContactCategoryResponse;
			try
			{
				string str = "http://restfulsms.com/api/CustomerClubContact/GetCategories";
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				CustomerClubContact.HttpRequestFactory = () => new HttpGetRequest();
				CustomerClubContactCategoryResponse customerClubContactCategoryResponse1 = CustomerClubContact.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str,
					Json = null
				}, strs).Deserialize<CustomerClubContactCategoryResponse>();
				if (customerClubContactCategoryResponse1 != null)
				{
					customerClubContactCategoryResponse = customerClubContactCategoryResponse1;
				}
				else
				{
					customerClubContactCategoryResponse = null;
				}
			}
			catch (Exception exception)
			{
				customerClubContactCategoryResponse = null;
			}
			return customerClubContactCategoryResponse;
		}

		public CustomerClubContactsResponse GetContacts(string tokenKey, int pageNumber)
		{
			CustomerClubContactsResponse customerClubContactsResponse;
			try
			{
				string str = string.Format("http://restfulsms.com/api/CustomerClubContact/GetContacts?pageNumber={0}", pageNumber);
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				CustomerClubContact.HttpRequestFactory = () => new HttpGetRequest();
				CustomerClubContactsResponse customerClubContactsResponse1 = CustomerClubContact.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str,
					Json = null
				}, strs).Deserialize<CustomerClubContactsResponse>();
				if (customerClubContactsResponse1 != null)
				{
					customerClubContactsResponse = customerClubContactsResponse1;
				}
				else
				{
					customerClubContactsResponse = null;
				}
			}
			catch (Exception exception)
			{
				customerClubContactsResponse = null;
			}
			return customerClubContactsResponse;
		}

		public CustomerClubContactsResponse GetContactsByCategoryId(string tokenKey, int categoryId, int pageNumber)
		{
			CustomerClubContactsResponse customerClubContactsResponse;
			try
			{
				string str = string.Format("http://restfulsms.com/api/CustomerClubContact/GetContactsByCategoryById?categoryId={0}&pageNumber={1}", categoryId, pageNumber);
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				CustomerClubContact.HttpRequestFactory = () => new HttpGetRequest();
				CustomerClubContactsResponse customerClubContactsResponse1 = CustomerClubContact.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str,
					Json = null
				}, strs).Deserialize<CustomerClubContactsResponse>();
				if (customerClubContactsResponse1 != null)
				{
					customerClubContactsResponse = customerClubContactsResponse1;
				}
				else
				{
					customerClubContactsResponse = null;
				}
			}
			catch (Exception exception)
			{
				customerClubContactsResponse = null;
			}
			return customerClubContactsResponse;
		}

		public CustomerClubContactResponse Update(string tokenKey, CustomerClubContactObject model)
		{
			CustomerClubContactResponse customerClubContactResponse;
			try
			{
				string str = model.Serialize<CustomerClubContactObject>();
				string str1 = "http://restfulsms.com/api/CustomerClubContact";
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				CustomerClubContact.HttpRequestFactory = () => new HttpPutRequest();
				CustomerClubContactResponse customerClubContactResponse1 = CustomerClubContact.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str1,
					Json = str
				}, strs).Deserialize<CustomerClubContactResponse>();
				if (customerClubContactResponse1 != null)
				{
					customerClubContactResponse = customerClubContactResponse1;
				}
				else
				{
					customerClubContactResponse = null;
				}
			}
			catch (Exception exception)
			{
				customerClubContactResponse = null;
			}
			return customerClubContactResponse;
		}
	}
}