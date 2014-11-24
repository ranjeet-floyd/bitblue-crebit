using Newtonsoft.Json;

namespace com.dhs.webapi.Models.DL.Common
{
    public class DL_OperatorReturn
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("operaterId")]
        public string OperaterId { get; set; }
        [JsonProperty("operaterName")]
        public string OperaterName { get; set; }
        [JsonProperty("serviceType")]
        public string ServiceType { get; set; }
    }
    public class DL_OperatorMarginReturn
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("margin")]
        public string Margin { get; set; }
    }
    public class DL_SessionCyberPlateStatus
    {
        public int OperatorId { get; set; }
        public string Session { get; set; }
    }
    public class DL_SessionCyberPlateStatusReturn
    {
        [JsonProperty("transId")]
        public string TransId { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("cyberCode")]
        public string CyberCode { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class MSEBUserDetailsReq
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public string ConsumerNo { get; set; }
        public int  BuCode { get; set; }
        public double Amount { get; set; }
    }

    public class MSEBUserDetailsReqReturn
    {
        public string SDToBePaid { get; set; }
        public string ConsumerType { get; set; }
        public int SD_ARREARS { get; set; }
        public string billingUnitDesc { get; set; }
        public string billToBePaid { get; set; }
        public int netPPDAmount { get; set; }
        public int billAmount { get; set; }
        public string dueDate { get; set; }
        public int consumptionUnits { get; set; }
        public string billingUnit { get; set; }
        public string consumerNo { get; set; }
        public string processingCycle { get; set; }
        public string billMonth { get; set; }
 
    }

    public class MSEBapiReq
    {
        public string ConsumerNo { get; set; }
        public int BuNumber { get; set; }

    }

    public class DeleteRegAccount
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public double Id { get; set; }
    }

    public class DeleteRegAccountReturn
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}