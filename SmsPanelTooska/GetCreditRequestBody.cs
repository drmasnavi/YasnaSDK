[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
public partial class GetCreditRequestBody
{
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
    public string username;
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
    public string password;
    
    public GetCreditRequestBody()
    {
    }
    
    public GetCreditRequestBody(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}