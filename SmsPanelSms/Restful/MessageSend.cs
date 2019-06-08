using System;
using System.Collections.Generic;

namespace SmsPanelSms.Restful
{
	public class MessageSend
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
				lock (MessageSend.HttpRequestFactoryLock)
				{
					defaultFactory = MessageSend._httpClientFactory ?? MessageSend.DefaultFactory;
				}
				return defaultFactory;
			}
			set
			{
				lock (MessageSend.HttpRequestFactoryLock)
				{
					MessageSend._httpClientFactory = value;
				}
			}
		}

		static MessageSend()
		{
			MessageSend.CachedClient = new Lazy<IHttpRequest>(() => new HttpPostRequest());
			MessageSend.DefaultFactory = () => MessageSend.CachedClient.Value;
			MessageSend.HttpRequestFactoryLock = new object();
		}

		public MessageSend()
		{
		}

		public SentMessageResponseByDate GetByDate(string tokenKey, string shamsi_FromDate, string shamsi_ToDate, int rowsPerPage, int requestedPageNumber)
		{
			SentMessageResponseByDate sentMessageResponseByDate;
			try
			{
				string str = string.Format("http://restfulsms.com/api/MessageSend?Shamsi_FromDate={0}&Shamsi_ToDate={1}&RowsPerPage={2}&RequestedPageNumber={3}", new object[] { shamsi_FromDate, shamsi_ToDate, rowsPerPage, requestedPageNumber });
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				MessageSend.HttpRequestFactory = () => new HttpGetRequest();
				SentMessageResponseByDate sentMessageResponseByDate1 = MessageSend.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str
				}, strs).Deserialize<SentMessageResponseByDate>();
				if (sentMessageResponseByDate1 != null)
				{
					sentMessageResponseByDate = sentMessageResponseByDate1;
				}
				else
				{
					sentMessageResponseByDate = null;
				}
			}
			catch (Exception exception)
			{
				sentMessageResponseByDate = null;
			}
			return sentMessageResponseByDate;
		}

		public SentMessageResponseById GetById(string tokenKey, int id)
		{
			SentMessageResponseById sentMessageResponseById;
			try
			{
				string str = string.Format("http://restfulsms.com/api/MessageSend?id={0}", id);
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				MessageSend.HttpRequestFactory = () => new HttpGetRequest();
				SentMessageResponseById sentMessageResponseById1 = MessageSend.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str
				}, strs).Deserialize<SentMessageResponseById>();
				if (sentMessageResponseById1 != null)
				{
					sentMessageResponseById = sentMessageResponseById1;
				}
				else
				{
					sentMessageResponseById = null;
				}
			}
			catch (Exception exception)
			{
				sentMessageResponseById = null;
			}
			return sentMessageResponseById;
		}

		public MessageSendResponseObject Send(string tokenKey, MessageSendObject model)
		{
			MessageSendResponseObject messageSendResponseObject;
			try
			{
				string str = model.Serialize<MessageSendObject>();
				string str1 = "http://restfulsms.com/api/MessageSend";
				Dictionary<string, string> strs = new Dictionary<string, string>()
				{
					{ "x-sms-ir-secure-token", tokenKey }
				};
				MessageSend.HttpRequestFactory = () => new HttpPostRequest();
				MessageSendResponseObject messageSendResponseObject1 = MessageSend.HttpRequestFactory().Execute(new HttpObject()
				{
					Url = str1,
					Json = str
				}, strs).Deserialize<MessageSendResponseObject>();
				if (messageSendResponseObject1 != null)
				{
					messageSendResponseObject = messageSendResponseObject1;
				}
				else
				{
					messageSendResponseObject = null;
				}
			}
			catch (Exception exception)
			{
				messageSendResponseObject = null;
			}
			return messageSendResponseObject;
		}
	}
}