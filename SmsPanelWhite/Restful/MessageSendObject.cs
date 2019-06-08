using System;

namespace SmsPanelSms.Restful
{
	public class MessageSendObject
	{
		public bool? CanContinueInCaseOfError
		{
			get;
			set;
		}

		public string LineNumber
		{
			get;
			set;
		}

		public string[] Messages
		{
			get;
			set;
		}

		public string[] MobileNumbers
		{
			get;
			set;
		}

		public DateTime? SendDateTime
		{
			get;
			set;
		}

		public MessageSendObject()
		{
		}
	}
}