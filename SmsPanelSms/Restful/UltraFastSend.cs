namespace SmsPanelSms.Restful
{
	public class UltraFastSend
	{
		public long Mobile
		{
			get;
			set;
		}

		public UltraFastParameters[] ParameterArray
		{
			get;
			set;
		}

		public int TemplateId
		{
			get;
			set;
		}

		public UltraFastSend()
		{
		}
	}
}