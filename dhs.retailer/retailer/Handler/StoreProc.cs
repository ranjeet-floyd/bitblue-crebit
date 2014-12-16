//Store Procedure List
namespace retailer.Handler
{
    public static class StoreProc
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
        public const string SP_GetOperaters = "GetOperaters";
        public const string SP_GetOperaterServiceOffById = "GetOperaterServiceOffById";// @UserType int,
        //@UserName nvarchar(50)
        public const string SP_GetOperaterMargin = "GetOperaterMargin";
        public const string SP_GetApis = "GetApis";
        public const string SP_InsertRegisterMobile = "InsertRegisterMobile";
        public const string SP_DHS_API_ChangePassword = "DHS_API_ChangePassword";
    }
}