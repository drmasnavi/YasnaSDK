using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using SmsPanelSms.YasnaServiceSendReceive;

namespace SmsPanelSms
{
    public class ServiceValidator
    {
        internal WSHttpBinding BindingConfig;
        internal EndpointIdentity DnsIdentity;
        internal Uri SmsUri;
        internal Uri UserUri;

        private ContractDescription ConfDescription;

        public ServiceValidator()
        {
            // In constructor initializing configuration elements by code
            BindingConfig = ConfigBinding();
            DnsIdentity = ConfigEndPoint();
            SmsUri = ConfigSmsUri();
            UserUri = ConfigUserUri();
            ConfDescription = ConfigContractDescription();
        }


        /*  public void MainOperation()
        {
            var Address = new EndpointAddress(URI, DNSIdentity);
            var Client = new EvalServiceClient(BindingConfig, Address);
            Client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.PeerTrust;
            Client.Endpoint.Contract = ConfDescription;
            Client.ClientCredentials.UserName.UserName = "companyUserName";
            Client.ClientCredentials.UserName.Password = "companyPassword";
            Client.Open();

            string CatchData = Client.CallServiceMethod();

            Client.Close();
        }*/


        private static WSHttpBinding ConfigBinding()
        {
            // ----- Programmatic definition of the SomeService Binding -----
            var wsHttpBinding = new WSHttpBinding
            {
                Name = "BindingName",
                CloseTimeout = TimeSpan.FromMinutes(1),
                OpenTimeout = TimeSpan.FromMinutes(1),
                ReceiveTimeout = TimeSpan.FromMinutes(10),
                SendTimeout = TimeSpan.FromMinutes(1),
                BypassProxyOnLocal = false,
                TransactionFlow = false,
                HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                MaxBufferPoolSize = 524288,
                MaxReceivedMessageSize = 65536,
                MessageEncoding = WSMessageEncoding.Text,
                TextEncoding = Encoding.UTF8,
                UseDefaultWebProxy = true,
                AllowCookies = false,
                ReaderQuotas =
                {
                    MaxDepth = 32,
                    MaxArrayLength = 16384,
                    MaxStringContentLength = 8192,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384
                }
            };



            wsHttpBinding.ReliableSession.Ordered = true;
            wsHttpBinding.ReliableSession.InactivityTimeout = TimeSpan.FromMinutes(10);
            wsHttpBinding.ReliableSession.Enabled = false;

            wsHttpBinding.Security.Mode = SecurityMode.Message;
            wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            wsHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            wsHttpBinding.Security.Transport.Realm = "";

            wsHttpBinding.Security.Message.NegotiateServiceCredential = true;
            wsHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            wsHttpBinding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Basic256;
            // ----------- End Programmatic definition of the SomeServiceServiceBinding --------------

            return wsHttpBinding;

        }

        private static Uri ConfigSmsUri()
        {
            // ----- Programmatic definition of the Service URI configuration -----
            var uri = new Uri("http://n.sms.ir/ws/SendReceive.asmx");
            return uri;
        }
        private static Uri ConfigUserUri()
        {
            // ----- Programmatic definition of the Service URI configuration -----
            var uri = new Uri("http://n.sms.ir/ws/Users.asmx");
            return uri;
        }
        private static EndpointIdentity ConfigEndPoint()
        {
            // ----- Programmatic definition of the Service EndPointIdentitiy configuration -----
            var dnsIdentity = EndpointIdentity.CreateDnsIdentity("tempCert");

            return dnsIdentity;
        }


        private static ContractDescription ConfigContractDescription()
        {
            //ToDo Change Service & Contract
            // ----- Programmatic definition of the Service ContractDescription Binding -----
            var contract = ContractDescription.GetContract(typeof (SendReceiveSoap),
                typeof (SendReceiveSoapClient));

            return contract;
        }
    }
}
