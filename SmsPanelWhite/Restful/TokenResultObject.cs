namespace SmsPanelSms.Restful
{
	public class TokenResultObject
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

		public string TokenKey
		{
			get;
			set;
		}

		public TokenResultObject()
		{
		}
	}
}