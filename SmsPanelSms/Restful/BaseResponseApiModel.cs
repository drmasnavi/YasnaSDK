namespace SmsPanelSms.Restful
{
	public class BaseResponseApiModel
	{
		public bool IsSuccessful
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public BaseResponseApiModel()
		{
		}
	}
}