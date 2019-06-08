[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
public partial class GetCreditResponseBody
{
    
    [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
    public double GetCreditResult;
    
    public GetCreditResponseBody()
    {
    }
    
    public GetCreditResponseBody(double GetCreditResult)
    {
        this.GetCreditResult = GetCreditResult;
    }
}