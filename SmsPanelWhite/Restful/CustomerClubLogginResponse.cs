namespace SmsPanelSms.Restful
{
	public class CustomerClubLogginResponse : BaseResponseApiModel
	{
		public string AccountCharge
		{
			get;
			set;
		}

		public string CompanyName
		{
			get;
			set;
		}

		public string ContactCount
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public string Result
		{
			get;
			set;
		}

		public CustomerClubLogginResponse()
		{
		}
	}
}