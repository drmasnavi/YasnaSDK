[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
public partial class SendSMSRequestBody
{
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
    public string username;
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
    public string password;
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
    public tempuri.org.ArrayOfString senderNumbers;
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
    public tempuri.org.ArrayOfString recipientNumbers;
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
    public tempuri.org.ArrayOfString messageBodies;
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
    public tempuri.org.ArrayOfString sendDate;
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
    public tempuri.org.ArrayOfInt messageClasses;
    
    [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
    public tempuri.org.ArrayOfLong checkingMessageIds;
    
    public SendSMSRequestBody()
    {
    }
    
    public SendSMSRequestBody(string username, string password, tempuri.org.ArrayOfString senderNumbers, tempuri.org.ArrayOfString recipientNumbers, tempuri.org.ArrayOfString messageBodies, tempuri.org.ArrayOfString sendDate, tempuri.org.ArrayOfInt messageClasses, tempuri.org.ArrayOfLong checkingMessageIds)
    {
        this.username = username;
        this.password = password;
        this.senderNumbers = senderNumbers;
        this.recipientNumbers = recipientNumbers;
        this.messageBodies = messageBodies;
        this.sendDate = sendDate;
        this.messageClasses = messageClasses;
        this.checkingMessageIds = checkingMessageIds;
    }
}