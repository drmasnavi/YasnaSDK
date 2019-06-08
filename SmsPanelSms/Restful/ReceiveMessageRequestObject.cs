namespace SmsPanelSms.Restful
{
	public class ReceiveMessageRequestObject
	{
		public int RequestedPageNumber
		{
			get;
			set;
		}

		public int RowsPerPage
		{
			get;
			set;
		}

		public string Shamsi_FromDate
		{
			get;
			set;
		}

		public string Shamsi_ToDate
		{
			get;
			set;
		}

		public ReceiveMessageRequestObject()
		{
		}
	}
}