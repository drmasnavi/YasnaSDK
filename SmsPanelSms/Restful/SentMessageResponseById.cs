namespace SmsPanelSms.Restful
{
	public class SentMessageResponseById : BaseResponseApiModel
	{
		public SentSmsDetails Messages
		{
			get;
			set;
		}

		public SentMessageResponseById()
		{
		}
	}
}