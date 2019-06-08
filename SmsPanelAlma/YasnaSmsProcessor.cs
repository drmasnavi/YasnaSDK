using System;
using System.ServiceModel;
using YasnaSms;

namespace SmsPanelAlma
{
    class YasnaSmsProcessor : ISmsProcessor
    {
        private string _username;
        private string _password;
        private string _sender;
        public string GetName(int lcid)
        {
            return "Alma";
        }

        public string GetWebSite()
        {
            return "sms.almasms.com";
        }

        public void SetParameters(string username, string password, string sender)
        {
            _username = username;
            _password = password;
            _sender = sender;
        }

        public SendReturnObject SendSms(string txt, string receiver, DateTime dt)
        {
            try
            {
                LogStringValue?.Invoke("متن پیام", txt);
                LogStringValue?.Invoke("گیرنده", receiver);
                var binding = new BasicHttpBinding();
                var alma = new AlmaSms.smsserverPortTypeClient(binding, new EndpointAddress("http://37.130.202.188/public/class/sms/webservice3/server.php?wsdl"));
                var res=alma.SendSMS(_sender, new[] {receiver}, txt, "", _username, _password);
                LogStringValue?.Invoke("Result", res);
                return new SendReturnObject(string.Empty, SentStatus.Successful, -1);
              
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("SendSms", ex);
            }
            return new SendReturnObject(string.Empty, SentStatus.Failed, -1);
        }

      

        public SendReturnObject SendFlashSms(string txt, string receiver)
        {
            LogInternalException?.Invoke("SendFlashSms", new NotSupportedException("ارسال فلش پشتيباني نمی شود"));
            return null;
        }


        public SendReturnObject[] SendSmsList(string txt, string[] receivers)
        {
            try
            {
                LogStringValue?.Invoke("متن پیام", txt);
                var binding = new BasicHttpBinding();
                var alma = new AlmaSms.smsserverPortTypeClient(binding, new EndpointAddress("http://37.130.202.188/public/class/sms/webservice3/server.php?wsdl"));
                var res = alma.SendSMS(_sender, receivers, txt, "", _username, _password);
                LogStringValue?.Invoke("Result", res);
                return new[] {new SendReturnObject(string.Empty, SentStatus.Successful, -1)};

            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("SendSms", ex);
            }
            return new[] {new SendReturnObject(string.Empty, SentStatus.Failed, -1)};
        }

        public SendReturnObject[] SendFlashSmsList(string txt, string[] receivers)
        {
            LogInternalException?.Invoke("SendSmsList", new NotSupportedException("ارسال گروهی فلش پشتیبانی نمی شود"));
            return null;
        }


        public double GetCurrentCredit()
        {
            try
            {
                var binding = new BasicHttpBinding();
                var alma = new AlmaSms.smsserverPortTypeClient(binding, new EndpointAddress("http://37.130.202.188/public/class/sms/webservice3/server.php?wsdl"));
                return Convert.ToDouble(alma.GetCredit(_username,_password));
                
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("GetCurrentCredit", ex);
            }
            return 0;
        }

        public int GetTotalCount()
        {
            LogInternalException?.Invoke("GetTotalCount", new NotSupportedException("استفاده از باکس دریافتی پشتیبانی نمی شود"));
            return 0;
        }

        public int GetUnreadCount()
        {
            LogInternalException?.Invoke("GetUnreadCount", new NotSupportedException("استفاده از باکس دریافتی پشتیبانی نمی شود"));
            return 0;
        }

        public RetrievalStatus GetRetrievalStatus(string id)
        {
            LogInternalException?.Invoke("GetRetrievalStatus", new NotSupportedException("شناسايي وضعيت پيامك پشتیبانی نمی شود"));
            return RetrievalStatus.Unknown;
            
        }

        public RetrievalStatus[] GetRetrievalStatuses(string[] id)
        {
            LogInternalException?.Invoke("GetRetrievalStatuses", new NotSupportedException("دریافت گروهی وضعیت پشتیبانی نمی شود"));
            return null;
        }

        public bool IsNumberBlackList(string number)
        {
            LogInternalException?.Invoke("IsNumberBlackList", new NotSupportedException("دریافت وضعيت بلك ليست شماره پشتیبانی نمی شود"));
            return false;
        }

        public string ProcessorKey => "AlmaYasnaDriver";

        public string DeveloperEmail => "sup@yasnasystem.ir";

        public bool SupportFlashSms => false;

        public bool SupportMultipleSend => true;

        public bool SupportRetrieval => false;

        public bool SupportBlackListCheck => false;

        public bool SupportMultipleRetreive => false;

        public bool SupportInboxCheckCount => false;

        public bool SupportScheduledSms => false;

        public event LogInternalException LogInternalException;
        public event LogIntValue LogIntValue;
        public event LogStringValue LogStringValue;
        public event LogObjectValue LogObjectValue;
        public event LogMessageValue LogMessageValue;
        public event LogArrayValue LogArrayValue;
    }
}
