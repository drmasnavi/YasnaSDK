namespace SmsPanelSms.Restful
{
	public class CustomerClubContactsResponse : BaseResponseApiModel
	{
		public float AllPages
		{
			get;
			set;
		}

		public float AllRecords
		{
			get;
			set;
		}

		public Contactscustomerclubresponsedetail[] ContactsCustomerClubResponseDetails
		{
			get;
			set;
		}

		public CustomerClubContactsResponse()
		{
		}
	}
}