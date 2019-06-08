namespace SmsPanelSms.Restful
{
	public class SmsLineNumber : BaseResponseApiModel
	{
		public SMSLines[] SMSLines
		{
			get;
			set;
		}

		public SmsLineNumber()
		{
		}
	}
}