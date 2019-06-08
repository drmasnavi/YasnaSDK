namespace SmsPanelTooska
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="v2Soap")]
    public interface IV2Soap
    {
    
        // CODEGEN: Generating message contract since element name username from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SendSMS", ReplyAction="*")]
        SendSMSResponse SendSMS(SendSMSRequest request);
    
        // CODEGEN: Generating message contract since element name username from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetMessageStatus", ReplyAction="*")]
        GetMessageStatusResponse GetMessageStatus(GetMessageStatusRequest request);
    
        // CODEGEN: Generating message contract since element name username from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetCredit", ReplyAction="*")]
        GetCreditResponse GetCredit(GetCreditRequest request);
    
        // CODEGEN: Generating message contract since element name username from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CheckMessageIDs", ReplyAction="*")]
        CheckMessageIDsResponse CheckMessageIDs(CheckMessageIDsRequest request);
    
        // CODEGEN: Generating message contract since element name username from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetReceiveMessages", ReplyAction="*")]
        GetReceiveMessagesResponse GetReceiveMessages(GetReceiveMessagesRequest request);
    
        // CODEGEN: Generating message contract since element name username from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetCountForPostalCode", ReplyAction="*")]
        GetCountForPostalCodeResponse GetCountForPostalCode(GetCountForPostalCodeRequest request);
    
        // CODEGEN: Generating message contract since element name username from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SendBulkSmsToPostalCode", ReplyAction="*")]
        SendBulkSmsToPostalCodeResponse SendBulkSmsToPostalCode(SendBulkSmsToPostalCodeRequest request);
    
        // CODEGEN: Generating message contract since element name username from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetBulkRequestInfo", ReplyAction="*")]
        GetBulkRequestInfoResponse GetBulkRequestInfo(GetBulkRequestInfoRequest request);
    
        // CODEGEN: Generating message contract since element name username from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetNewsList", ReplyAction="*")]
        GetNewsListResponse GetNewsList(GetNewsListRequest request);
    }
}