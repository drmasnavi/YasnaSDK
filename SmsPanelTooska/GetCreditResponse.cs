[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
public partial class GetCreditResponse
{
    
    [System.ServiceModel.MessageBodyMemberAttribute(Name="GetCreditResponse", Namespace="http://tempuri.org/", Order=0)]
    public GetCreditResponseBody Body;
    
    public GetCreditResponse()
    {
    }
    
    public GetCreditResponse(GetCreditResponseBody Body)
    {
        this.Body = Body;
    }
}