using System;
using YasnaSms;

namespace SmsPanelOpilo
{
    class YasnaSmsProcessor : ISmsProcessor
    {
        private string _username;
        private string _password;
        private string _sender;
        public string GetName(int lcid)
        {
            return "Opilo";
        }

        public string GetWebSite()
        {
            return "www.Opilo.com";
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
                if (LogStringValue != null)
                {
                    LogStringValue("متن پیام", txt);
                }
                if (LogStringValue != null)
                {
                    LogStringValue("گیرنده", receiver);
                }
                var op = new Opilo(_username, _password);
                string err;
                var res = (op.httpsend(_sender, new[] { receiver }, txt, out err))[0];
                return new SendReturnObject(string.Empty, SentStatus.Successful, -1);
              
            }
            catch (Exception ex)
            {
                if (LogInternalException != null)
                {
                    LogInternalException("SendSms", ex);
                }
            }
            return new SendReturnObject(string.Empty, SentStatus.Failed, -1);
        }

      

        public SendReturnObject SendFlashSms(string txt, string receiver)
        {
            if (LogInternalException != null)
            {
                LogInternalException("SendFlashSms", new NotSupportedException("ارسال فلش پشتيباني نمی شود"));
            }
            return null;
        }


        public SendReturnObject[] SendSmsList(string txt, string[] receivers)
        {
            if (LogInternalException != null)
            {
                LogInternalException("SendSmsList", new NotSupportedException("ارسال گروهی پشتیبانی نمی شود"));
            }
            return null;
        }

        public SendReturnObject[] SendFlashSmsList(string txt, string[] receivers)
        {
            if (LogInternalException != null)
            {
                LogInternalException("SendSmsList", new NotSupportedException("ارسال گروهی فلش پشتیبانی نمی شود"));
            }
            return null;
        }


        public double GetCurrentCredit()
        {
            try
            {
                var op = new Opilo(_username, _password);
                string err;
                return Convert.ToDouble(op.GetCredit(out err))*80;
                
            }
            catch (Exception ex)
            {
                if (LogInternalException != null)
                {
                    LogInternalException("GetCurrentCredit", ex);
                }
            }
            return 0;
        }

        public int GetTotalCount()
        {
            if (LogInternalException != null)
            {
                LogInternalException("GetTotalCount", new NotSupportedException("استفاده از باکس دریافتی پشتیبانی نمی شود"));
            }
            return 0;
        }

        public int GetUnreadCount()
        {
            if (LogInternalException != null)
            {
                LogInternalException("GetUnreadCount", new NotSupportedException("استفاده از باکس دریافتی پشتیبانی نمی شود"));
            }
            return 0;
        }

        public RetrievalStatus GetRetrievalStatus(string id)
        {
            if (LogInternalException != null)
            {
                LogInternalException("GetRetrievalStatus", new NotSupportedException("شناسايي وضعيت پيامك پشتیبانی نمی شود"));
            }
            return RetrievalStatus.Unknown;
            
        }

        public RetrievalStatus[] GetRetrievalStatuses(string[] id)
        {
            if (LogInternalException != null)
            {
                LogInternalException("GetRetrievalStatuses", new NotSupportedException("دریافت گروهی وضعیت پشتیبانی نمی شود"));
            }
            return null;
        }

        public bool IsNumberBlackList(string number)
        {
            if (LogInternalException != null)
            {
                LogInternalException("IsNumberBlackList", new NotSupportedException("دریافت وضعيت بلك ليست شماره پشتیبانی نمی شود"));
            }
            return false;
        }

        public string ProcessorKey
        {
            get { return "OpiloYasnaDriver"; }
        }

        public string DeveloperEmail
        {
            get { return "sup@yasnasystem.ir"; }
        }

        public bool SupportFlashSms
        {
            get { return false; }
        }

        public bool SupportMultipleSend
        {
            get { return false; }
        }

        public bool SupportRetrieval
        {
            get { return false; }
        }

        public bool SupportBlackListCheck
        {
            get { return false; }
        }

        public bool SupportMultipleRetreive
        {
            get { return false; }
        }

        public bool SupportInboxCheckCount
        {
            get { return false; }
        }

        public bool SupportScheduledSms
        {
            get { return false; }
           
        }

        public event LogInternalException LogInternalException;
        public event LogIntValue LogIntValue;
        public event LogStringValue LogStringValue;
        public event LogObjectValue LogObjectValue;
        public event LogMessageValue LogMessageValue;
        public event LogArrayValue LogArrayValue;
    }
}
