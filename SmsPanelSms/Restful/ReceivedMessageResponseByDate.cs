namespace SmsPanelSms.Restful
{
	public class ReceivedMessageResponseByDate : BaseResponseApiModel
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

		public ReceivedMessageResponseByDate()
		{
		}
	}
}