namespace SmsPanelSms.Restful
{
	public class CustomerClubSendResponse : BaseResponseApiModel
	{
		public string BatchKey
		{
			get;
			set;
		}

		public SentSMSLog2[] Ids
		{
			get;
			set;
		}

		public CustomerClubSendResponse()
		{
		}
	}
}