namespace SmsPanelSms.Restful
{
	public class SentMessageResponseByDate : BaseResponseApiModel
	{
		public int CountOfAll
		{
			get;
			set;
		}

		public SentMessage[] Messages
		{
			get;
			set;
		}

		public SentMessageResponseByDate()
		{
		}
	}
}