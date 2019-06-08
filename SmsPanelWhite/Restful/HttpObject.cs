namespace SmsPanelSms.Restful
{
	internal class HttpObject
	{
		public string Json
		{
			get;
			set;
		}

		public EnumHttpMethod Method
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public HttpObject()
		{
		}
	}
}