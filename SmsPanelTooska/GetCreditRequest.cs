[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
public partial class GetCreditRequest
{
    
    [System.ServiceModel.MessageBodyMemberAttribute(Name="GetCredit", Namespace="http://tempuri.org/", Order=0)]
    public GetCreditRequestBody Body;
    
    public GetCreditRequest()
    {
    }
    
    public GetCreditRequest(GetCreditRequestBody Body)
    {
        this.Body = Body;
    }
}