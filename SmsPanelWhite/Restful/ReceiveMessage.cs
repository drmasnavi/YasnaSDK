using System;
using System.Collections.Generic;

namespace SmsPanelSms.Restful
{
	public class ReceiveMessage
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
				lock (ReceiveMessage.HttpRequestFactoryLock)
				{
					defaultFactory = ReceiveMessage._httpClientFactory ?? ReceiveMessage.DefaultFactory;
				}
				return defaultFactory;
			}
			set
			{
				lock (ReceiveMessage.HttpRequestFactoryLock)
				{
					ReceiveMessage._httpClientFactory = value;
				}
			}
		}

		static ReceiveMessage()
		{
			ReceiveMessage.CachedClient = new Lazy<IHttpRequest>(() => new HttpGetRequest());
			ReceiveMessage.DefaultFactory = () => ReceiveMessage.CachedClient.Value;
			ReceiveMessage.HttpRequestFactoryLock = new object();
		}

		public ReceiveMessage()
		{
		}

		public ReceivedMessageResponseByDate GetByDate(string tokenKey, string shamsi_FromDate, string shamsi_ToDate, int rowsPerPage, int requestedPageNumber)
		{
			ReceivedMessageResponseByDate receivedMessageResponseByDate;
			try
			{
				string str = string.Format("http://restfulsms.com/api/ReceiveMessage?Shamsi_FromDate={0}&Shamsi_ToDate={1}&RowsPerPage={2}&RequestedPageNumber={3}", new object[] { shamsi_FromDate, shamsi_ToDate, rowsPerPage, requestedPageNumber });
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				ReceivedMessageResponseByDate receivedMessageResponseByDate1 = ReceiveMessage.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str
				}, strs).Deserialize<ReceivedMessageResponseByDate>();
				if (receivedMessageResponseByDate1 != null)
				{
					receivedMessageResponseByDate = receivedMessageResponseByDate1;
				}
				else
				{
					receivedMessageResponseByDate = null;
				}
			}
			catch (Exception exception)
			{
				receivedMessageResponseByDate = null;
			}
			return receivedMessageResponseByDate;
		}

		public ReceiveMessageResponseById GetByLastMessageID(string tokenKey, int id)
		{
			ReceiveMessageResponseById receiveMessageResponseById;
			try
			{
				string str = string.Format("http://restfulsms.com/api/ReceiveMessage?id={0}", id);
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				ReceiveMessageResponseById receiveMessageResponseById1 = ReceiveMessage.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str
				}, strs).Deserialize<ReceiveMessageResponseById>();
				if (receiveMessageResponseById1 != null)
				{
					receiveMessageResponseById = receiveMessageResponseById1;
				}
				else
				{
					receiveMessageResponseById = null;
				}
			}
			catch (Exception exception)
			{
				receiveMessageResponseById = null;
			}
			return receiveMessageResponseById;
		}
	}
}