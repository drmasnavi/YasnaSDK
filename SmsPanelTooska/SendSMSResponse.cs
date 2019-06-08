[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
public partial class SendSMSResponse
{
    
    [System.ServiceModel.MessageBodyMemberAttribute(Name="SendSMSResponse", Namespace="http://tempuri.org/", Order=0)]
    public SendSMSResponseBody Body;
    
    public SendSMSResponse()
    {
    }
    
    public SendSMSResponse(SendSMSResponseBody Body)
    {
        this.Body = Body;
    }
}