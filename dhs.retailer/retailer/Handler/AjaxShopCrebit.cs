//Ajax handler for retailer website
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
namespace retailer.Handler
{

    public class AjaxShopCrebit
    {
        SqlCommand cmd = null;

        [AjaxPro.AjaxMethod()]
        public string MobileRegis(string userId, string pass, string mobile, string name, string userType)
        {
            string retVal = string.Empty;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.CommandText = StoreProc.SP_GetOperaterMargin;
            User user = new User() { Password = pass, UserName = userId };
            try
            {
                //validate User
                Validation.UserCheck(user);
                if (Validation._IsSuccess)
                {
                    Api_MobileReg reg = new Api_MobileReg()
                    {
                        UserId = userId,
                        UserType = userType,
                        Mobile = mobile,
                        Name = name

                    };
                    retVal = WebApiHelper.PostWebApi("/dashboard/mobileRegistration", reg);
                }

                else
                    retVal = "Invalid User";
            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            return retVal;
        }


        [AjaxPro.AjaxMethod()]
        public string ProfitSummary(string userId, string pass, string fromDate, string toDate, string userType)
        {
            string retVal = string.Empty;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.CommandText = StoreProc.SP_GetOperaterMargin;
            User user = new User() { Password = pass, UserName = userId };
            try
            {
                //validate User
                Validation.UserCheck(user);
                if (Validation._IsSuccess)
                {
                    Api_Profit profit = new Api_Profit()
                    {
                        UserId = userId,
                        UserType = userType,
                        FromDate = fromDate,
                        ToDate = toDate

                    };
                    retVal = WebApiHelper.PostWebApi("/dashboard/profit", profit);
                }

                else
                    retVal = "Invalid User";
            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            return retVal;
        }

        [AjaxPro.AjaxMethod()]
        public string BalanceSummary(string userId, string pass, string fromDate, string toDate, string userType)
        {
            string retVal = string.Empty;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.CommandText = StoreProc.SP_GetOperaterMargin;
            User user = new User() { Password = pass, UserName = userId };
            try
            {
                //validate User
                Validation.UserCheck(user);
                if (Validation._IsSuccess)
                {
                    Api_Balance bal = new Api_Balance()
                    {
                        UserId = userId,
                        UserType = userType,
                        FromDate = fromDate,
                        ToDate = toDate

                    };
                    retVal = WebApiHelper.PostWebApi("/dashboard/balanceUse", bal);
                }

                else
                    retVal = "Invalid User";
            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            return retVal;
        }


        [AjaxPro.AjaxMethod()]
        public string TransactionSummary(string userId, string pass, string fromDate, string toDate, string userType)
        {
            string retVal = string.Empty;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.CommandText = StoreProc.SP_GetOperaterMargin;
            User user = new User() { Password = pass, UserName = userId };
            try
            {
                //validate User
                Validation.UserCheck(user);
                if (Validation._IsSuccess)
                {
                    Api_Trans trans = new Api_Trans()
                    {
                        UserId = userId,
                        UserType = userType,
                        FromDate = fromDate,
                        ToDate = toDate

                    };
                    retVal = WebApiHelper.PostWebApi("/dashboard/tranDetails", trans);
                }

                else
                    retVal = "Invalid User";
            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            return retVal;
        }

        [AjaxPro.AjaxMethod()]
        public string GetOperators()
        {
            string retVal = string.Empty;
            cmd = new SqlCommand();
            cmd.CommandText = StoreProc.SP_GetOperaters;
            try
            {
                DataSet ds = DataBase.SelectAdaptQry(cmd);
                if (ds.Tables.Count > 0)
                    retVal = JsonConvert.SerializeObject(ds.Tables[0]);

            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            return retVal;
        }


        [AjaxPro.AjaxMethod()]
        public string GetMarginList()
        {
            string retVal = string.Empty;
            cmd = new SqlCommand();
            cmd.CommandText = StoreProc.SP_GetOperaterMargin;
            try
            {
                DataSet ds = DataBase.SelectAdaptQry(cmd);
                if (ds.Tables.Count > 0)
                    retVal = JsonConvert.SerializeObject(ds.Tables[0]);

            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            return retVal;
        }

        [AjaxPro.AjaxMethod()]
        public string Services(string userId, string pass, string number, string userType, string operatorId, string amount, string transType)
        {
            string retVal = string.Empty;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.CommandText = StoreProc.SP_GetOperaterMargin;
            User user = new User() { Password = pass, UserName = userId };
            try
            {
                //validate User
                Validation.UserCheck(user);
                if (Validation._IsSuccess)
                {
                    Api_Services services = new Api_Services()
                    {
                        UserId = userId,
                        UserType = userType,
                        TransactionType = transType,
                        OperatorId = operatorId,
                        Number = number,
                        Amount = Convert.ToDouble(amount),
                        Source = "Crebit-Website"
                    };
                    retVal = WebApiHelper.PostWebApi("/dashboard/service", services);
                }

                else
                    retVal = "Invalid User";
            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            return retVal;

        }

        [AjaxPro.AjaxMethod()]
        public string CheckUser(string mobile, string key)
        {
            string retVal = string.Empty;
            User user = new User() { Password = key, UserName = mobile };
            try
            {
                retVal = Validation.UserCheck(user);
            }
            catch (Exception ex)
            {
                retVal = "500 : " + ex.Message;
            }
            return retVal;
        }

        [AjaxPro.AjaxMethod()]
        public string TransferFund(string userId, string pass, string mobileTo, string amount, string userType)
        {
            string retVal = string.Empty;
            cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.CommandText = StoreProc.SP_GetOperaterMargin;
            User user = new User() { Password = pass, UserName = userId };
            try
            {
                //validate User
                Validation.UserCheck(user);
                if (Validation._IsSuccess)
                {
                    Api_transferFund transFund = new Api_transferFund()
                    {
                        UserId = userId,
                        UserType = userType,
                        MobileTo = mobileTo,
                        Amount = amount
                    };
                    retVal = WebApiHelper.PostWebApi("/dashboard/transfer", transFund);
                }

                else
                    retVal = "Invalid User";
            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            return retVal;
        }

        //ForgotPass
        [AjaxPro.AjaxMethod()]
        public string ForgotPass(string mobileTo)
        {
            string retVal = string.Empty;
            //cmd = new SqlCommand();
            //cmd.Parameters.AddWithValue("@UserId", userId);
            //cmd.CommandText = StoreProc.SP_GetOperaterMargin;
            //User user = new User() { Password = pass, UserName = userId };
            try
            {
                //validate User
                //  Validation.UserCheck(user);
                //if (Validation._IsSuccess)
                //{
                Api_forgotPass forgotPass = new Api_forgotPass() { Mobile = mobileTo };
                //{
                //    UserId = userId,
                //    UserType = userType,
                //    MobileTo = mobileTo,
                //    Amount = amount
                //};
                retVal = WebApiHelper.PostWebApi("/dhs/forgotpassword", forgotPass);
                //}

                //else
                //  retVal = "Invalid User";
            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            return retVal;
        }

        [AjaxPro.AjaxMethod()]
        public string SignUp(string name, string pass, string mobile)
        {
            string retVal = string.Empty;
            try
            {
                Api_signUp signUp = new Api_signUp()
                    {
                        Mobile = mobile,
                        Name = name,
                        Pass = pass,
                        UserType = "4"
                    };
                retVal = WebApiHelper.PostWebApi("/dhs/signUp", signUp);

            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            return retVal;
        }

        [AjaxPro.AjaxMethod()]
        public string ChangePass(string userId, string oldPassword, string newPassword, string userType)
        {
            string retVal = string.Empty;
            try
            {
                Api_changePass changePass = new Api_changePass()
                {
                    UserId = userId,
                    OldPassword = oldPassword,
                    NewPassword = newPassword,
                    UserType = userType
                };
                retVal = WebApiHelper.PostWebApi("dhs/changePassword", changePass);

            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            return retVal;
        }

        [AjaxPro.AjaxMethod()]
        public string AdminUpdates()
        {
            string retVal = string.Empty;
            try
            {
                retVal = WebApiHelper.GetWebApi("dhs/adminUpdate");

            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            return retVal;
        }

        [AjaxPro.AjaxMethod()]
        public string AdminPopUp()
        {
            string retVal = string.Empty;
            try
            {
                retVal = WebApiHelper.GetWebApi("dhs/adminPopup");
            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }
            return retVal;
        }

    }
}