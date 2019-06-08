namespace SmsPanelSms.Restful
{
	public class ReceivedMessages
	{
		public long ID
		{
			get;
			set;
		}

		public string LatinReceiveDateTime
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

		public string ReceiveDateTime
		{
			get;
			set;
		}

		public string SMSMessageBody
		{
			get;
			set;
		}

		public string TypeOfMessage
		{
			get;
			set;
		}

		public ReceivedMessages()
		{
		}
	}
}