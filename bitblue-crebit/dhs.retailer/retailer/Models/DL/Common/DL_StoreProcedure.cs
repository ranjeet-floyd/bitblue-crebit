
namespace com.dhs.webapi.Model.DL.Common
{
    public class DL_StoreProcedure
    {
        public const string SP_DHS_API_ValidateUser = "DHS_API_ValidateUser";
        public const string SP_DHS_API_Login = "DHS_API_Login";
        public const string SP_DHS_API_SignUp = "DHS_API_SignUp";
        public const string SP_DHS_API_GetDashBoard = "DHS_API_GetDashBoard";
        public const string SP_DHS_API_GetTransactionDetails = "DHS_API_TransactionSummary";
        public const string SP_DHS_API_Service = "DHS_API_Service";//used for recharge services
        public const string SP_DHS_API_AvailableBalanceNServieStatus = "DHS_API_AvailableBalanceNServieStatus";
        public const string SP_DHS_API_BalanceUse = "DHS_API_BalanceUse";
        public const string SP_DHS_API_Profit = "DHS_API_ProfitSummary";
        public const string SP_GetOperaters = "DHS_API_Operators";
        public const string SP_GetOperaterServiceOffById = "GetOperaterServiceOffById";// @UserType int,
        //@UserName nvarchar(50)
        public const string SP_GetOperaterMargin = "DHS_API_Operators";
        public const string SP_GetApis = "GetApis";
        public const string SP_InsertRegisterMobile = "InsertRegisterMobile";
        public const string SP_DHS_API_ChangePassword = "DHS_API_ChangePassword";
        //
        public const string SP_DHS_API_AdminUpdate = "DHS_API_AdminUpdate";
        public const string SP_DHS_API_AdminPopUp = "DHS_API_AdminPopUp";
        public const string SP_DHS_API_TransferFund = "DHS_API_TransferFund";
        public const string SP_DHS_API_ForgotPassword = "DHS_API_ForgotPassword";
        public const string SP_DHS_API_DHS_CREBIT_MONTHLYCHARGES = "DHS_API_CREBIT_MONTHLYCHARGES";
        public const string SP_DHS_API_CheckUser = "DHS_API_CheckUser";
        public const string SP_DHS_API_PayElectricity = "DHS_API_PayElectricity";
        public const string SP_DHS_API_GetTransactionSearch = "DHS_API_GetTransactionSearch";
        public const string SP_DHS_API_BankRegistration = "DHS_API_BankRegistration";
        public const string SP_DHS_API_BankAccounts = "DHS_API_GetBankAccounts";
        public const string SP_DHS_API_BankTransReq = "DHS_API_BankTransReq";
        public const string SP_DHS_DeleteRegAccount = "DHS_DeleteRegAccount";
        public const string SP_DHS_API_RefundorTransStatus = "DHS_API_RefundorTransStatus";
        public const string SP_DHS_GetSessionId = "GetSessionId";
    }
}