using System;
using YasnaSms;

namespace SmsPanelPayamakPanel
{
    class YasnaSmsProcessor : ISmsProcessor
    {
        private string _username;
        private string _password;
        private string _sender;
        public string GetName(int lcid)
        {
            return "PayamakPanel";
        }

        public string GetWebSite()
        {
            return "www.payamak-panel.com";
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
                //retval :
                // Invalid User Pass=0,
                // Successfull = 1,
                // No Credit = 2,
                // DailyLimit = 3,
                // SendLimit = 4,
                // Invalid Number = 5
                // System IS Disable = 6
                // Bad Words= 7
                // Pardis Minimum Receivers=8
                // Number Is Public=9

                //Status :
                // Sent=0,
                // Failed=1
                Send sms = new Send();
                long[] rec = null;
                byte[] status = null;
                int retval = sms.SendSms(_username, _password, receiver.Split(','), _sender, txt, false, "", ref rec, ref status);
                if (retval == 1)
                {
                    return new SendReturnObject(string.Empty, SentStatus.Successful, -1);
                }
                if(status.Length>0 && rec.Length>0)
                    return new SendReturnObject($"{status[0]}|{rec[0]}", SentStatus.Failed, -1);
                return new SendReturnObject(string.Empty, SentStatus.Failed, -1);
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("SendSms", ex);
            }
            return new SendReturnObject(string.Empty, SentStatus.Failed, -1);
        }



        public SendReturnObject SendFlashSms(string txt, string receiver)
        {
            try
            {
                LogStringValue?.Invoke("متن پیام", txt);
                LogStringValue?.Invoke("گیرنده", receiver);
                //retval :
                // Invalid User Pass=0,
                // Successfull = 1,
                // No Credit = 2,
                // DailyLimit = 3,
                // SendLimit = 4,
                // Invalid Number = 5
                // System IS Disable = 6
                // Bad Words= 7
                // Pardis Minimum Receivers=8
                // Number Is Public=9

                //Status :
                // Sent=0,
                // Failed=1
                Send sms = new Send();
                long[] rec = null;
                byte[] status = null;
                int retval = sms.SendSms(_username, _password, receiver.Split(','), _sender, txt, true, "", ref rec, ref status);
                if (retval == 1)
                {
                    return new SendReturnObject(string.Empty, SentStatus.Successful, -1);
                }
                if (status.Length > 0 && rec.Length > 0)
                    return new SendReturnObject($"{status[0]}|{rec[0]}", SentStatus.Failed, -1);
                return new SendReturnObject(string.Empty, SentStatus.Failed, -1);
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("SendSms", ex);
            }
            return new SendReturnObject(string.Empty, SentStatus.Failed, -1);
        }


        public SendReturnObject[] SendSmsList(string txt, string[] receivers)
        {
            LogInternalException?.Invoke("SendSmsList", new NotSupportedException("ارسال گروهی پشتیبانی نمی شود"));
            return null;
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
                Send sms = new Send();
                return sms.GetCredit(_username, _password);

            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("GetCurrentCredit", ex);
            }
            return 0;
        }

        public int GetTotalCount()
        {
            Send sms = new Send();
            return sms.GetInboxCount(_username, _password,false)+sms.GetInboxCount(_username, _password, true); 
        }

        public int GetUnreadCount()
        {
            Send sms = new Send();
            return sms.GetInboxCount(_username, _password, false);
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

        public string ProcessorKey => "PayamakPanelYasnaDriver";

        public string DeveloperEmail => "sup@yasnasystem.ir";

        public bool SupportFlashSms => true;

        public bool SupportMultipleSend => false;

        public bool SupportRetrieval => false;

        public bool SupportBlackListCheck => false;

        public bool SupportMultipleRetreive => false;

        public bool SupportInboxCheckCount => true;

        public bool SupportScheduledSms => false;

        public event LogInternalException LogInternalException;
        public event LogIntValue LogIntValue;
        public event LogStringValue LogStringValue;
        public event LogObjectValue LogObjectValue;
        public event LogMessageValue LogMessageValue;
        public event LogArrayValue LogArrayValue;
    }
}
