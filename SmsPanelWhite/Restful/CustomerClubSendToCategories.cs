using System;

namespace SmsPanelSms.Restful
{
	public class CustomerClubSendToCategories
	{
		public bool? CanContinueInCaseOfError
		{
			get;
			set;
		}

		public int[] contactsCustomerClubCategoryIds
		{
			get;
			set;
		}

		public string Messages
		{
			get;
			set;
		}

		public DateTime? SendDateTime
		{
			get;
			set;
		}

		public CustomerClubSendToCategories()
		{
		}
	}
}