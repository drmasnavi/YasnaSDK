using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.ServiceModel;
using SmsPanelSms.Restful;
using SmsPanelSms.Service_References.YasnaService;
using SmsPanelSms.Service_References.YasnaServiceSendReceive;
using YasnaSms;

namespace SmsPanelSms
{
    class YasnaSmsProcessor : ISmsProcessor
    {
        private string _username;
        private string _password;
        private string _sender;
        public string GetName(int lcid)
        {
            return "SMS.ir";
        }

        public string GetWebSite()
        {
            return "www.Sms.ir";
        }

        public void SetParameters(string username, string password, string sender)
        {
            _username = username;
            _password = password;
            _sender = sender;
        }

        public SendReturnObject OldSendSms(string txt, string receiver, DateTime dt)
        {
            try
            {
                LogStringValue?.Invoke("متن پیام", txt);
                LogStringValue?.Invoke("گیرنده", receiver);
                long resNum;
                var resOfConvert = long.TryParse(receiver, out resNum);
                if (!resOfConvert) throw new Exception("شماره موبايل مي بايست به شكل يك عدد باشد");
                var binding = new BasicHttpBinding();
                using (var proxy = new SendReceiveSoapClient(binding, new EndpointAddress("http://n.sms.ir/ws/SendReceive.asmx")))
                {
                    var message = "";
                    var smsLines = proxy.GetSMSLines(_username, _password, ref message);
                    if (!string.IsNullOrEmpty(message.Trim()))
                        throw new Exception(message);
                    var lst = new List<WebServiceSmsSend>
                    {
                        new WebServiceSmsSend
                        {
                             IsFlash = false,
                             MessageBody = txt,
                             MobileNo =resNum
                        }
                    };
                    var retObject = proxy.SendMessage(_username, _password, lst.ToArray(), smsLines.First(x=>x.LineNumber==Convert.ToInt64(_sender)).ID, DateTime.Now, ref message);
                    if (!string.IsNullOrEmpty(message.Trim()))
                        throw new Exception(message);
                    var status = SentStatus.Successful;
                    var id = Convert.ToInt32(retObject[0].ToString());
                    return new SendReturnObject("", status, id);
                }
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("SendSms", ex);
            }
            return new SendReturnObject(string.Empty, SentStatus.Failed, -1);
        }
        public SendReturnObject SendSms(string txt, string receiver, DateTime dt)
        {
            try
            {
                LogStringValue?.Invoke("متن پیام", txt);
                LogStringValue?.Invoke("گیرنده", receiver);
                long resNum;
                if (receiver.StartsWith("+98")) receiver=receiver.Replace("+98", "0");
                if (receiver.StartsWith("0098")) receiver=receiver.Replace("0098", "0");
                if (receiver.StartsWith("98")) receiver=receiver.Replace("98", "0");
                if (!receiver.StartsWith("0")) receiver = "0" + receiver;
                var resOfConvert = long.TryParse(receiver, out resNum);
                if (!resOfConvert) throw new Exception("شماره موبايل مي بايست به شكل يك عدد باشد");
                var token = new Token().GetToken(_password, _username);

                var messageSendObject = new MessageSendObject()
                {
                    Messages = new List<string> { txt }.ToArray(),
                    MobileNumbers = new List<string> { receiver }.ToArray(),
                    LineNumber = _sender,
                    SendDateTime = null,
                    CanContinueInCaseOfError = true
                };

                MessageSendResponseObject messageSendResponseObject = new MessageSend().Send(token, messageSendObject);

                if (messageSendResponseObject.IsSuccessful)
                {
                    var id = Convert.ToInt32(messageSendResponseObject.Ids[0].ID);
                    return new SendReturnObject("", SentStatus.Successful, id);
                }
                else
                {
                    throw new Exception(messageSendResponseObject.Message);
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

        public SendReturnObject OldSendFlashSms(string txt, string receiver)
        {
            try
            {
                LogStringValue?.Invoke("متن پیام", txt);
                LogStringValue?.Invoke("گیرنده", receiver);
                long resNum;
                var resOfConvert = long.TryParse(receiver, out resNum);
                if (!resOfConvert) throw new Exception("شماره موبايل مي بايست به شكل يك عدد باشد");
                var binding = new BasicHttpBinding();
                using (var proxy = new SendReceiveSoapClient(binding, new EndpointAddress("http://n.sms.ir/ws/SendReceive.asmx")))
                {
                var message = "";
                    var smsLines = proxy.GetSMSLines(_username, _password, ref message);
                    if (!string.IsNullOrEmpty(message.Trim()))
                        throw new Exception(message);
                    var lst = new List<WebServiceSmsSend>
                    {
                        new WebServiceSmsSend
                        {
                             IsFlash = true,
                             MessageBody = txt,
                             MobileNo =resNum
                        }
                    };
                    var retObject = proxy.SendMessage(_username, _password, lst.ToArray(), smsLines.First(x => x.LineNumber == Convert.ToInt64(_sender)).ID, DateTime.Now, ref message);
                    if (!string.IsNullOrEmpty(message.Trim()))
                        throw new Exception(message);
                    var status = SentStatus.Successful;
                    var id = Convert.ToInt32(retObject[0].ToString());
                    return new SendReturnObject("", status, id);
                }
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
                long resNum;
                var resOfConvert = long.TryParse(receiver, out resNum);
                if (!resOfConvert) throw new Exception("شماره موبايل مي بايست به شكل يك عدد باشد");
                var token = new Token().GetToken(_password, _username);

                var messageSendObject = new MessageSendObject()
                {
                    Messages = new List<string> { txt }.ToArray(),
                    MobileNumbers = new List<string> { receiver }.ToArray(),
                    LineNumber = _sender,
                    SendDateTime = null,
                    CanContinueInCaseOfError = true
                };

                MessageSendResponseObject messageSendResponseObject = new MessageSend().Send(token, messageSendObject);

                if (messageSendResponseObject.IsSuccessful)
                {
                    var id = Convert.ToInt32(messageSendResponseObject.Ids[0]);
                    return new SendReturnObject("", SentStatus.Successful, id);
                }
                else
                {
                    throw new Exception(messageSendResponseObject.Message);
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


        public double OldGetCurrentCredit()
        {
            try
            {
                var binding = new BasicHttpBinding();
                using (var proxy = new UsersSoapClient(binding, new EndpointAddress("http://n.sms.ir/ws/Users.asmx")))
                {
                    var message = "";
                    var userid = proxy.GetUserIDByUserNameAndPassword(_username, _password, ref message);
                    if (!string.IsNullOrEmpty(message.Trim()))
                        throw new Exception(message);
                    int countOfAll = 0;
                    if (userid != null)
                        return Convert.ToInt64(proxy.GetCreditArticles(_username, _password, userid.Value, SqlDateTime.MinValue.Value, SqlDateTime.MaxValue.Value, 1, int.MaxValue, ref countOfAll, ref message)[0].Remain*120);
                    throw new Exception("شناسه كاربري در بانك اطلاعاتي سامانه ايده پردازان الكترونيك يافت نشد");
                }
            }
            catch (Exception ex)
            {
                LogInternalException?.Invoke("GetCurrentCredit", ex);
            }
            return 0;
        }
        public double GetCurrentCredit()
        {
            try
            {
                var token = new Token().GetToken(_password, _username);

                CreditResponse credit = new Credit().GetCredit(token);

                if (credit.IsSuccessful)
                {
                    return credit.Credit;
                }
                else
                {
                    throw new Exception(credit.Message);
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

        public string ProcessorKey => "SmsIrYasnaDriver";

        public string DeveloperEmail => "sup@yasnasystem.ir";

        public bool SupportFlashSms => true;

        public bool SupportMultipleSend => false;

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
