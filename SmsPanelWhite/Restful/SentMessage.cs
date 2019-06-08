namespace SmsPanelSms.Restful
{
	public class SentMessage
	{
		public long ID
		{
			get;
			set;
		}

		public string LineNumber
		{
			get;
			set;
		}

		public string MobileNo
		{
			get;
			set;
		}

		public string NativeDeliveryStatus
		{
			get;
			set;
		}

		public string PersianSendDateTime
		{
			get;
			set;
		}

		public string SendDateTime
		{
			get;
			set;
		}

		public string SMSMessageBody
		{
			get;
			set;
		}

		public string ToBeSentAt
		{
			get;
			set;
		}

		public string TypeOfMessage
		{
			get;
			set;
		}

		public SentMessage()
		{
		}
	}
}