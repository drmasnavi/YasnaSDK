using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using OpiloWebserviceLibrary;

namespace SmsPanelOpilo
{

    public class Opilo
    {
        private string _username;
        private string _password;
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public Dictionary<int, string> ResponseErrorDictionary = new Dictionary<int, string>();

       public Opilo(string username,string password)
        {
            this.Username = username;
            this.Password = password;

            ResponseErrorDictionary.Add(1, "یکی از پارامترهای مورد نیاز خالی است ، پارامترهای مورد نیاز عبارتند از : username,password,from,to,text");
            ResponseErrorDictionary.Add(2, "کلمه عبور و یا رمز وب سرویس اشتباه است.");
            ResponseErrorDictionary.Add(3, "حساب کاربری شما تایید نشده است .");
            ResponseErrorDictionary.Add(4, "وب سرویس شما غیرفعال است .");
            ResponseErrorDictionary.Add(5, "شماره خط اختصاصی ارسال کننده اشتباه است.");
            ResponseErrorDictionary.Add(6, "شماره موبایل گیرنده نامعتبر است .");
            ResponseErrorDictionary.Add(7, "اعتبار شما برای ارسال پیامک کافی نیست .");
            ResponseErrorDictionary.Add(8, "خطای داخلی سیستم ، نیاز به ارسال مجدد.");
        }
        public DataTable GetAllMessages(int count,out string errorMessage)
       {
           Stream objStream;
           StreamReader objSR;
           Encoding encode = Encoding.GetEncoding("utf-8");

           string str = "http://webservice.opilo.com/WS/getAllMessages/?username="+this.Username+"&password="+this.Password+"&count="+count.ToString();
           HttpWebRequest wrquest = (HttpWebRequest)WebRequest.Create(str);
           HttpWebResponse getresponse = null;
           getresponse = (HttpWebResponse)wrquest.GetResponse();

           objStream = getresponse.GetResponseStream();
           objSR = new StreamReader(objStream, encode, true);
           string strResponse = objSR.ReadToEnd();

           errorMessage= GetErrorMessage(strResponse);
            if(!string.IsNullOrEmpty(errorMessage))
            {
                return null;
            }

            if(strResponse.Trim()=="[[]]")
            {
                return null;
            }
           JavaScriptSerializer js = new JavaScriptSerializer();

           List<Sms> smsList;
           try
           {
               smsList = js.Deserialize<List<Sms>>(strResponse);
           }
           catch
           {
               errorMessage = "خطای غیر منتظره پیش آمده لطفا دوباره تلاش کنید.";
               return null;
           }

           return GetDataTableFromSmsList(smsList);
       }

        public DataTable Recieve(int count, string from, out string errorMessage)
        {
            Stream objStream;
            StreamReader objSR;
            Encoding encode = Encoding.GetEncoding("utf-8");

            string str = "http://webservice.opilo.com/WS/recieve/?username=" + this.Username + "&password=" + this.Password + "&from=" + from + "&count=" + count.ToString();
            HttpWebRequest wrquest = (HttpWebRequest)WebRequest.Create(str);
            HttpWebResponse getresponse = null;
            getresponse = (HttpWebResponse)wrquest.GetResponse();

            objStream = getresponse.GetResponseStream();
            objSR = new StreamReader(objStream, encode, true);
            string strResponse = objSR.ReadToEnd();

            errorMessage = GetErrorMessage(strResponse);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return null;
            }

            if (strResponse.Trim() == "[[]]")
            {
                return null;
            }
            JavaScriptSerializer js = new JavaScriptSerializer();

            List<Sms> smsList;
            try
            {
                 smsList = js.Deserialize<List<Sms>>(strResponse);
            }
            catch
            {
                errorMessage = "خطای غیر منتظره پیش آمده لطفا دوباره تلاش کنید.";
                return null;
            }
            return GetDataTableFromSmsList(smsList);
        }
        public object[] httpsend(string from,string[] toNumbers,string text, out string errorMessage)
        {
            Stream objStream;
            StreamReader objSR;
            Encoding encode = Encoding.GetEncoding("utf-8");

            string url = "http://webservice.opilo.com/WS/httpsend/?username=" + this.Username + "&password=" + this.Password + "&from=" + from + "&to=" + GetToNumberStringFromArray(toNumbers) + "&text=" + text;
            HttpWebRequest wrquest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse getresponse = null;
            getresponse = (HttpWebResponse)wrquest.GetResponse();

            objStream = getresponse.GetResponseStream();
            objSR = new StreamReader(objStream, encode, true);
            string strResponse = objSR.ReadToEnd();

            errorMessage = GetErrorMessage(strResponse);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return null;
            }

            if (strResponse.Trim() == "[[]]")
            {
                return null;
            }
            JavaScriptSerializer js = new JavaScriptSerializer();

            object[] smsList;
            try
            {
                smsList = new object[] {js.Deserialize<string>(strResponse)};
                    
               return smsList;
            }
            catch
            {
                errorMessage = "خطای غیر منتظره پیش آمده لطفا دوباره تلاش کنید.";
                return null;
            }

        }
        public object GetCredit(out string errorMessage)
        {
            Stream objStream;
            StreamReader objSR;
            Encoding encode = Encoding.GetEncoding("utf-8");

            string str = "http://webservice.opilo.com/WS/getCredit/?username=" + this.Username + "&password=" + this.Password;
            HttpWebRequest wrquest = (HttpWebRequest)WebRequest.Create(str);
            HttpWebResponse getresponse = null;
            getresponse = (HttpWebResponse)wrquest.GetResponse();

            objStream = getresponse.GetResponseStream();
            objSR = new StreamReader(objStream, encode, true);
            string strResponse = objSR.ReadToEnd();

            errorMessage = GetErrorMessage(strResponse);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return null;
            }

            if (strResponse.Trim() == "[[]]")
            {
                return null;
            }
            JavaScriptSerializer js = new JavaScriptSerializer();

            //List<OpiloWebserviceLibrary.Sms> smsList;
            try
            {
              object  smsList = js.Deserialize<dynamic>(strResponse);
              return smsList;
            }
            catch
            {
                errorMessage = "خطای غیر منتظره پیش آمده لطفا دوباره تلاش کنید.";
                return null;
            }

        }
        private DataTable GetDataTableFromSmsList(List<Sms> smsList)
        {
            DataTable dtSms = new DataTable();
            dtSms.Columns.Add("id");
            dtSms.Columns.Add("from");
            dtSms.Columns.Add("to");
            dtSms.Columns.Add("text");

            foreach(Sms sms in smsList)
            {
                DataRow dr = dtSms.NewRow();
                dr[0] = sms.id;
                dr[1] = sms.from;
                dr[2] = sms.to;
                dr[3] = sms.text;

                dtSms.Rows.Add(dr);
            }
            return dtSms;
        }
        private string GetErrorMessage(string response)
        {
            string errorMessage = "";
            int intErrorCode = -1;
            if (response.Trim().Length ==1 && int.TryParse(response, out intErrorCode))
            {
                try
                {
                    errorMessage = ResponseErrorDictionary[intErrorCode];
                }
                catch
                {
                    errorMessage = "";
                }
            }
            else
            {
                errorMessage = "";
            }
            return errorMessage;
        }
        private string GetToNumberStringFromArray(string[] arrToNumber)
        {
            string strTo="";
            for(int i=0;i<arrToNumber.Length;i++)
            {
                strTo += arrToNumber[i];
                if (i + 1 != arrToNumber.Length)
                    strTo += ",";
            }
            return strTo;
        }

    }
}
