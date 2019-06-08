namespace SmsPanelSms.Restful
{
	public class MessageSendResponseObject : BaseResponseApiModel
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

		public MessageSendResponseObject()
		{
		}
	}
}