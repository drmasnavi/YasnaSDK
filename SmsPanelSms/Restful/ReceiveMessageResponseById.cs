namespace SmsPanelSms.Restful
{
	public class ReceiveMessageResponseById : BaseResponseApiModel
	{
		public int CountOfAll
		{
			get;
			set;
		}

		public ReceivedMessages[] Messages
		{
			get;
			set;
		}

		public ReceiveMessageResponseById()
		{
		}
	}
}