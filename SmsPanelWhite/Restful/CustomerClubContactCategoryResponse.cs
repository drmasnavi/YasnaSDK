namespace SmsPanelSms.Restful
{
	public class CustomerClubContactCategoryResponse : BaseResponseApiModel
	{
		public ContactsCustomerClubCategory[] ContactsCustomerClubCategories
		{
			get;
			set;
		}

		public CustomerClubContactCategoryResponse()
		{
		}
	}
}