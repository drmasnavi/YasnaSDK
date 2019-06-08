namespace SmsPanelSms.Restful
{
	public class TokenRequestObject
	{
		public string SecretKey
		{
			get;
			set;
		}

		public string UserApiKey
		{
			get;
			set;
		}

		public TokenRequestObject()
		{
		}
	}
}