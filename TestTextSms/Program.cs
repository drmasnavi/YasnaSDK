using System;

namespace TestTextSms
{
    class Program
    {
        static void Main(string[] args)
        {
            Credit();
        }

        private static void Credit()
        {
            var proxy = new sms_webservice();
            var retValue = proxy.sms_credit("iranapplecenter","09121116695");
            Console.WriteLine(retValue); 
        }

        private static void TestBlack()
        {
            var proxy = new sms_webservice();
            var retValue = proxy.is_number_in_blacklist("09121991334");
            Console.WriteLine(retValue);
            
        }

        private static void TestRetValue()
        {
            var proxy = new sms_webservice();
            var retValue = proxy.sms_deliver("133090073");
            Console.WriteLine(retValue);
            //"5" BlackList
            //"0" OK!
        }

        private static void TestSend()
        {
            var proxy = new sms_webservice();
            var retValue = proxy.send_sms("iranapplecenter", "09121116695", "10002130663210", "09122800039", "با تشکر از خرید شما", null, null, 0, null);
            //"error [ wrong username OR password ] [ error_login_not_like_password ]"
            Console.WriteLine(retValue);
            //133090072
            //133090073
        }
        private static void TestSendFlash()
        {
            var proxy = new sms_webservice();
            var retValue = proxy.send_sms("iranapplecenter", "09121116695", "10002130663210", "09122800039", "با تشکر از خرید شما", "True", null, 0, null);
            Console.WriteLine(retValue);
        }
    }
}
