using System;
using YasnaSms;

namespace SmsPanelTextSMS
{
    class YasnaSmsProcessor : ISmsProcessor
    {
        private string _username;
        private string _password;
        private string _sender;
        public string GetName(int lcid)
        {
            return "TextSMS";
        }

        public string GetWebSite()
        {
            return "www.TextSms.ir";
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
                using (var proxy = new sms_webservice())
                {
                    var retObject = proxy.send_sms(_username, _password, _sender, receiver, txt, null, null, 0, null);
                    var status = HandleSendErrorMessage(retObject);
                    int id = 0;
                    if (status == SentStatus.Successful)
                    {
                        id = Convert.ToInt32(status);
                    }
                    return new SendReturnObject(retObject, status, id);
                }
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("SendSms", ex);
            }
            return new SendReturnObject(string.Empty, SentStatus.Failed, -1);
        }

        private SentStatus HandleSendErrorMessage(string retObject)
        {
            int value;
            return int.TryParse(retObject, out value) ? SentStatus.Successful : SentStatus.Failed;
        }

        public SendReturnObject SendFlashSms(string txt, string receiver)
        {
            try
            {
                LogStringValue?.Invoke("متن پیام", txt);
                LogStringValue?.Invoke("گیرنده", receiver);
                using (var proxy = new sms_webservice())
                {
                    var retObject = proxy.send_sms(_username, _password, _sender, receiver, txt, "True", null, 0, null);
                    var status = HandleSendErrorMessage(retObject);
                    int id = 0;
                    if (status == SentStatus.Successful)
                    {
                        id = Convert.ToInt32(status);
                    }
                    return new SendReturnObject(retObject, status, id);
                }
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

                using (var proxy = new sms_webservice())
                {
                    return proxy.sms_credit(_username, _password);
                }
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
            try
            {
                LogStringValue?.Invoke("شناسه جهت پیگیری وضعیت ارسال", id);

                using (var proxy = new sms_webservice())
                {
                    return HandleRetrievalStatusMessage(proxy.sms_deliver(id));
                }
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("GetRetrievalStatus", ex);
            }
            return RetrievalStatus.Unknown;
        }

        private RetrievalStatus HandleRetrievalStatusMessage(string smsDeliver)
        {
            switch (smsDeliver)
            {
                case "0":
                    return RetrievalStatus.Delivered;
                case "5":
                    return RetrievalStatus.BlackList;
                default:
                    return RetrievalStatus.Unknown;
            }
        }

        public RetrievalStatus[] GetRetrievalStatuses(string[] id)
        {
            LogInternalException?.Invoke("GetRetrievalStatuses", new NotSupportedException("دریافت گروهی وضعیت پشتیبانی نمی شود"));
            return null;
        }

        public bool IsNumberBlackList(string number)
        {
            try
            {
                LogStringValue?.Invoke("شماره جهت بررسی وضعیت لیست سیاه", number);
                using (var proxy = new sms_webservice())
                {
                    return proxy.is_number_in_blacklist(number) == 1;
                }
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("IsNumberBlackList", ex);
            }
            return false;
        }

        public string ProcessorKey => "TextSmsYasnaDriver";

        public string DeveloperEmail => "sup@yasnasystem.ir";

        public bool SupportFlashSms => true;

        public bool SupportMultipleSend => false;

        public bool SupportRetrieval => true;

        public bool SupportBlackListCheck => true;

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
