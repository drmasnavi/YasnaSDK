namespace SmsPanelSms.Restful
{
	public class SentSmsDetails
	{
		public int? DeliveryStateID
		{
			get;
			set;
		}

		public string DeliveryStatus
		{
			get;
			set;
		}

		public string DeliveryStatusFetchError
		{
			get;
			set;
		}

		public long ID
		{
			get;
			set;
		}

		public string MobileNo
		{
			get;
			set;
		}

		public bool NeedsReCheck
		{
			get;
			set;
		}

		public string SendDateTime
		{
			get;
			set;
		}

		public bool SendIsErronous
		{
			get;
			set;
		}

		public string SMSMessageBody
		{
			get;
			set;
		}

		public SentSmsDetails()
		{
		}
	}
}