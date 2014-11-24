using Newtonsoft.Json;
using System.Collections.Generic;

namespace com.dhs.webapi.Model.DL.Common
{


    public class DL_Transaction
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
    }

    public class DL_TransactionReturn
    {
        [JsonProperty("totalAmount")]
        public double TotalAmount { get; set; }
        [JsonProperty("totalProfit")]
        public double TotalProfit { get; set; }
        public List<DL_TransactionReturns> dL_TransactionReturns { get; set; }
    }
    public class DL_TransactionReturns
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("cBalance")]
        public string CBalance { get; set; }
        [JsonProperty("profit")]
        public string Profit { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("tDate")]
        public string TDate { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("operaterName")]
        public string OperaterName { get; set; }
        [JsonProperty("operaterId")]
        public int OperaterId { get; set; }
        [JsonProperty("OpType")]
        public int OpType { get; set; }
        [JsonProperty("charge")]
        public string Charge { get; set; }
    }


    public class DL_TransactionSearch
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int TypeId { get; set; }
    }

    public class DL_Service
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public int OperatorId { get; set; }
        public string Number { get; set; }
        public double Amount { get; set; }
        public string Source { get; set; } //app || website
        //Electricity
        public string Account { get; set; }
        //Insurance
        public string InsuranceDob { get; set; }
    }


    public class DL_ServiceReturn
    {
        [JsonProperty("transId")]
        public string TransId { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
        [JsonProperty("availableBalance")]
        public string AvailableBalance { get; set; }
    }

    public class DL_BalanceUseSummary
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int TypeId { get; set; }
        public string Value { get; set; }
    }

    public class DL_BalanceUse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        // [JsonProperty("contact")]
        [JsonProperty("contact")]//given to
        public string Contact { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }


    }
    public class DL_BalanceUseSummaryReturn
    {
        [JsonProperty("balanceUse")]
        public List<DL_BalanceUse> BalanceUse { get; set; }
        [JsonProperty("totalBalanceGiven")]
        public double TotalBalanceGiven { get; set; }
        [JsonProperty("totalBalanceTaken")]
        public double TotalBalanceTaken { get; set; }

    }

    public class DL_ProfitSummary
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }


    //public class ProfitBy
    //{
    //    [JsonProperty("profit")]
    //    public object Profit { get; set; }
    //    [JsonProperty("transactionType")]
    //    public string TransactionType { get; set; }
    //}

    //public class DL_ProfitSummaryReturn
    //{
    //    [JsonProperty("totaleProfit")]
    //    public string TotaleProfit { get; set; }
    //    [JsonProperty("profitBy")]
    //    public List<ProfitBy> ProfitBy { get; set; }
    //    [JsonProperty("takenBalance")]
    //    public List<object> TakenBalance { get; set; }
    //}

    //public class DL_ProfitSummaryReturn
    //{
    //    public Retailer retailer { get; set; }
    //    [JsonProperty("totalSucTrans")]
    //    public string TotalSucTrans { get; set; }
    //    [JsonProperty("totalFailTrans")]
    //    public string TotalFailTrans { get; set; }
    //    [JsonProperty("totalPorTrans")]
    //    public string TotalPorTrans { get; set; }
    //    [JsonProperty("totalAndrTrans")]
    //    public string TotalAndrTrans { get; set; }
    //}
    public class Transaction
    {
        public int TotalSucTrans { get; set; }
        public int TotalFailTrans { get; set; }
        public int TotalPorTrans { get; set; }
        public int TotalAndrTrans { get; set; }
        public object TotalJ2METrans { get; set; }
    }

    public class Retailer
    {
        [JsonProperty("totaleProfit")]
        public string TotaleProfit { get; set; }
        [JsonProperty("totalTakenBal")]
        public string TotalTakenBal { get; set; }
    }
    public class DL_ProfitSummarys
    {
        public string TransactionId { get; set; }
        public string Amount { get; set; }
        public string PreBal { get; set; }
        public string CurrBal { get; set; }
        public string UserProfit { get; set; }
        public string OperatorId { get; set; }
        public string Date { get; set; }
    }

    public class DL_ProfitSummaryReturn
    {
        public List<DL_ProfitSummarys> dL_ProfitSummarys { get; set; }
        public double TotalProfit { get; set; }
        public double TotalAmount { get; set; }
    }
    public class DL_MobileRegistration
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
    }

    public class DL_MobileRegistrationReturn
    {
        [JsonProperty("mobileRegisterId")]
        public string MobileRegisterId { get; set; }
    }

    public class DL_AccountRegistration
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }
        public string Account { get; set; }
        public string IFSC { get; set; }
    }

    public class DL_AccountRegistrationReturn
    {
        [JsonProperty("status")]
        public string status { get; set; }
        public string AvailBal { get; set; }
    }

    public class DL_BankTransReq
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string IFSC { get; set; }
        public float Amount { get; set; }
    }
    public class DL_BankTransReqReturn
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("availableBalance")]
        public string AvailableBalance { get; set; }
        [JsonProperty("refId")]
        public string RefId { get; set; }
    }

    public class DL_TransferFund
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public string MobileTo { get; set; }
        public string Amount { get; set; }
    }

    public class DL_TransferFundReturn
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("availableBalance")]
        public string AvailableBalance { get; set; }
    }

    public class DL_Electricity
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public int ServiceId { get; set; }
        public string CusAcc { get; set; }
        public int BU { get; set; }
        public string CyDiv { get; set; }
        public double Amount { get; set; }
        public string CusMob { get; set; }
        public string DueDate { get; set; }
    }
    public class DL_ElectricityReturn
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("avaiBal")]
        public double AvaiBal { get; set; }
    }

    public class DL_RefundOrTransStatus
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public int TypeId { get; set; }
        public string TransId { get; set; }
        public string Comments { get; set; }
    }

    public class DL_RefundOrTransStatusReturn
    {
        [JsonProperty("typeId")]
        public int TypeId { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("cybertransId")]
        public string CyberTransId { get; set; }
        [JsonProperty("operatorId")]
        public int OperatorId { get; set; }
        [JsonProperty("isRefundAlreadyApplied")]
        public bool IsRefundAlreadyApplied { get; set; }
    }
}