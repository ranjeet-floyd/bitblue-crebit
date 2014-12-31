using api.dhs.Logging;
using api.dhs.Models.BL.Common;
using com.dhs.webapi.Model.Common.CyberPlate;
using com.dhs.webapi.Model.DL.Common;
using com.dhs.webapi.Models.DL.Common;
using crebit.retailer.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApplication1.Models.BL.Common;

namespace com.dhs.webapi.Model.BL.Common
{
    public class BL_DashBoard
    {
        #region Variables

        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        [JsonIgnore]
        public bool _IsSuccess { get; set; }
        private string SpName { get; set; }
        public DL_TransactionReturn transReturn = null;
        public DL_ServiceReturn serviceReturn = null;
        public DL_BalanceUseSummaryReturn balanceReturn = null;
        public DL_ProfitSummaryReturn profitReturn = null;
        public DL_MobileRegistrationReturn mobileRegReturn = null;
        public List<DL_TransferFundReturn> fundReturn = null;
        public DL_ElectricityReturn electricityReturn = null;
        public DL_AccountRegistrationReturn accountRegReturn = null;
        public DL_BankTransReqReturn bankReqReturn = null;
        DataBase db = new DataBase();
        DataSet ds = null;

        #endregion

        #region TransactionSummary
        //Transaction Details
        public DL_TransactionReturn TransactionSummary(DL_Transaction trans)
        {
            transReturn = new DL_TransactionReturn();
            this.SpName = DL_StoreProcedure.SP_DHS_API_GetTransactionDetails; //Sp Name
            _IsSuccess = true;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@UserId", trans.UserId);
                param[1] = new SqlParameter("@FromDate", trans.FromDate);
                param[2] = new SqlParameter("@ToDate", trans.ToDate);
                param[3] = new SqlParameter("@StatusId", trans.StatusId);
                param[4] = new SqlParameter("@TypeId", trans.TypeId);

                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    //ds.Tables
                    var Json = JsonConvert.SerializeObject(ds.Tables[0], Formatting.None);
                    List<DL_TransactionReturns> transReturns = JsonConvert.DeserializeObject<List<DL_TransactionReturns>>(Json);
                    double totalAmount = 0, totalProfit = 0;

                    foreach (var item in transReturns)
                    {
                        if (item.OpType == 2 || item.OpType == 3 || item.OpType == 8 || item.OpType == 1100 || item.OpType != 1400)//show profit only for no-fixed charged services
                            totalProfit += Convert.ToDouble(item.Profit);
                        if (item.OpType != 1400 && item.Status == "Success")//refund type
                            totalAmount += Convert.ToDouble(item.Amount);
                    }
                    transReturn.dL_TransactionReturns = transReturns;
                    transReturn.TotalAmount = totalAmount; transReturn.TotalProfit = Math.Round(totalProfit, 2);
                }



            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Exception : " + ex.Message);
                _IsSuccess = false;
            }

            return transReturn;
        }
        #endregion

        #region Transaction Search
        public DL_TransactionReturn TransactionSearch(DL_TransactionSearch tranSearch)
        {
            this.transReturn = new DL_TransactionReturn();
            this.SpName = DL_StoreProcedure.SP_DHS_API_GetTransactionSearch; //Sp Name
            this._IsSuccess = true;
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@UserId", tranSearch.UserId);
                param[1] = new SqlParameter("@Value", tranSearch.Value);
                param[2] = new SqlParameter("@TypeId", tranSearch.TypeId);
                this.ds = db.GetDataSet(this.SpName, param);
                if (this.ds != null && this.ds.Tables.Count > 0)
                {
                    //ds.Tables
                    var Json = JsonConvert.SerializeObject(ds.Tables[0], Formatting.None);
                    List<DL_TransactionReturns> transReturns = JsonConvert.DeserializeObject<List<DL_TransactionReturns>>(Json);
                    double totalAmount = 0, totalProfit = 0;

                    foreach (var item in transReturns)
                    {
                        totalAmount += Convert.ToDouble(item.Amount);
                        totalProfit += Convert.ToDouble(item.Profit);
                    }

                    transReturn.dL_TransactionReturns = transReturns;
                    transReturn.TotalAmount = totalAmount; transReturn.TotalProfit = totalProfit;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Exception : " + ex.Message);
                this._IsSuccess = false;
            }

            return this.transReturn;
        }
        #endregion

        #region Services || Recharge

        public DL_ServiceReturn Service(DL_Service service)
        {
            try
            {
                this._IsSuccess = true;
                this.serviceReturn = new DL_ServiceReturn();
                ProcessReqReturn rechargeStatus = new ProcessReqReturn();

                #region Check Account available balance and servie status.

                this.SpName = DL_StoreProcedure.SP_DHS_API_AvailableBalanceNServieStatus; //Sp Name || Get the Available balance and Service status.
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@UserId", service.UserId);
                p[1] = new SqlParameter("@OpId", service.OperatorId);
                ds = db.GetDataSet(this.SpName, p);
                UserInfo userInfo = null;

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataRow drc = ds.Tables[0].Rows[0];

                    userInfo = new UserInfo()
                   {
                       AvailBal = Convert.ToDouble(drc["AvailableBalance"] != null ? drc["AvailableBalance"] : 0),
                       Margin = Convert.ToDouble(drc["Margin"]),
                       ServiceStatus = Convert.ToBoolean(drc["ServiceStatus"]),
                       UType = Convert.ToInt32(drc["UType"]),
                       IsFixedCharge = Convert.ToBoolean(drc["IsFixedCharge"])
                   };
                }

                #endregion

                #region Call Cyber Plate APIs

                if (userInfo.AvailBal >= service.Amount && userInfo.ServiceStatus)
                {
                    //if (!string.IsNullOrEmpty(service.InsuranceDob) && (Convert.ToInt32(service.OperatorId) == 243) || Convert.ToInt32(service.OperatorId) == 242)//Insurance
                    //account = service.InsuranceDob;
                    rechargeStatus = new RechargeProcess().ProcessRequest(service.OperatorId, service.Number, service.Amount, service.Account);

                    //double profit = 0;
                    string transactionId = string.IsNullOrEmpty(rechargeStatus.TransactionId) ? "-1" : rechargeStatus.TransactionId;
                    string status = rechargeStatus.StatusCode == 1 ? "1" : "0";//check once
                    int apiId = (int)APIName.Cyber_Plate;//"CyberPate";
                    string billRecieptId = string.Empty;

                    //Profit ||
                    //check || Postpaid,electricity, gas bill, insurense and broadband 
                    if (rechargeStatus.StatusCode == 1)
                    {
                        //if (userInfo.IsFixedCharge)
                        //{
                        //    profit = 0; //No Profit
                        //    //service.Amount = service.Amount+ userInfo.Margin;//Fixed charges from Crebit
                        //}
                        //else
                        //{
                        //    profit = ((service.Amount * userInfo.Margin) / 100); //Percentage margin
                        //}

                    }
                    else //Fail 
                    {
                        //profit = 0;
                        service.Amount = 0;
                    }

                    #region SQLParameters

                    this.SpName = DL_StoreProcedure.SP_DHS_API_Service; //Sp Name
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@UserId", service.UserId);
                    param[1] = new SqlParameter("@Number", service.Number);
                    param[2] = new SqlParameter("@Amount", service.Amount);
                    //param[3] = new SqlParameter("@Profit", profit);
                    param[3] = new SqlParameter("@ApiTransactionId", transactionId);//transId from CyberPlate 
                    param[4] = new SqlParameter("@OperaterId", service.OperatorId);
                    param[5] = new SqlParameter("@Status", status); //'1-pass' || '0-fail'
                    param[6] = new SqlParameter("@Source", service.Source); //'website/androidapp'
                    param[7] = new SqlParameter("@ApiId", apiId); //CyberPlate
                    param[8] = new SqlParameter("@Date", indianTime);
                    param[9] = new SqlParameter("@Session", rechargeStatus.SessionId);

                    #endregion

                    ds = db.GetDataSet(this.SpName, param);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        serviceReturn.TransId = dr["TransId"].ToString();
                        serviceReturn.AvailableBalance = dr["AvBalance"].ToString();
                        string reMobile = dr["Mobile"].ToString();
                        //AvBalance
                        serviceReturn.StatusCode = rechargeStatus.StatusCode; //success
                        #region commented || SMS || Not required || 05-Aug-14
                        if (serviceReturn.StatusCode == 1)
                        {
                            //send message to retailer
                            //Dear USER ##Field##. Recharge successful on ##Field## for Rs. ##Field##.Your Available Balance is ##Field##.CREBIT Customer Experience Team.
                            //string message = "Dear USER " + service.UserId + ". Recharge successful on " + service.Number + " for Rs.  " + service.Amount + ".Your Available Balance is " + serviceReturn.AvailableBalance + ".CREBIT Customer Experience Team.";
                            //string message = "Dear User" + service.UserId + ". Recharge successful on " + service.Number + " for Rs. " + service.Amount + ".Your Available Balance is " + serviceReturn.AvailableBalance + " .Crebit Customer Experience Team.";

                            //send meesage to customer > amount => 250
                            string message = "Dear User, Recharge successful on " + service.Number + " for Rs." + service.Amount + " .CREBIT Customer Experience Team.";

                            if (service.Amount >= 250)
                            {
                                Task t = new Task(() => BL_SMS.SendSMS(service.Number, message));
                                t.Start();
                            }
                            //send message to customer
                            //if (service.TransactionType == "1" || service.TransactionType == "2")
                            {
                                //  message = "Dear Customer, Your account has been successfully recharged for Rs. " + service.Amount + " .Crebit Customer Experience Team.";
                                //BL_SMS.SendSMS(service.Number, message);
                            }
                        }
                        #endregion
                    }
                    serviceReturn.Message = rechargeStatus.Message;
                }
                #endregion

                #region If Either not enough balance or Servie if Off
                else
                {

                    //serviceReturn.IsSuccess = false;
                    serviceReturn.TransId = null;

                    if (userInfo.AvailBal < service.Amount)
                    {
                        //serviceReturn.Message = "Not enough Balance in account";
                        serviceReturn.StatusCode = 2;
                    }
                    else
                    {
                        //serviceReturn.Message = "Serice is Off";
                        serviceReturn.StatusCode = 3;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "@Service | Exception : " + ex.Message);
                serviceReturn.StatusCode = 0; //error
                serviceReturn.TransId = null;
                _IsSuccess = false;
            }

            return serviceReturn;
        }

        #endregion

        #region Balance Summary
        //Balance
        public DL_BalanceUseSummaryReturn BalanceUseSummary(DL_BalanceUseSummary balanceUseSummary)
        {
            this.SpName = DL_StoreProcedure.SP_DHS_API_BalanceUse; //Sp Name
            _IsSuccess = true;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@UserId", balanceUseSummary.UserId);
                param[1] = new SqlParameter("@FromDate", balanceUseSummary.FromDate);
                param[2] = new SqlParameter("@ToDate", balanceUseSummary.ToDate);
                param[3] = new SqlParameter("@TypeId", balanceUseSummary.TypeId);
                param[4] = new SqlParameter("@Value", balanceUseSummary.Value);

                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    var Json = JsonConvert.SerializeObject(ds.Tables[0], Formatting.None);
                    var balanceUse = JsonConvert.DeserializeObject<List<DL_BalanceUse>>(Json);
                    double totalGivenBalance = 0;
                    double totalTakenBalance = 0;
                    foreach (var item in balanceUse)
                    {
                        if (item.Type == 1) //1 => Given
                            totalGivenBalance += Convert.ToDouble(item.Amount);
                        if (item.Type == 2)//2=> Taken
                            totalTakenBalance += Convert.ToDouble(item.Amount);
                    }
                    balanceReturn = new DL_BalanceUseSummaryReturn() { BalanceUse = balanceUse, TotalBalanceGiven = totalGivenBalance, TotalBalanceTaken = totalTakenBalance };
                }

            }
            catch (Exception ex)
            {
                this._IsSuccess = false;
                Logger.WriteLog(LogLevelL4N.ERROR, "BalanceUseSummary return | Exception : " + ex.Message);
            }

            return balanceReturn;
        }
        #endregion

        #region Profit
        //Profit Summary
        public DL_ProfitSummaryReturn ProfitSummary(DL_ProfitSummary profitSummary)
        {
            this.SpName = DL_StoreProcedure.SP_DHS_API_Profit; //Sp Name
            profitReturn = new DL_ProfitSummaryReturn();
            _IsSuccess = true;
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@UserId", profitSummary.UserId);
                param[1] = new SqlParameter("@FromDate", profitSummary.FromDate);
                param[2] = new SqlParameter("@ToDate", profitSummary.ToDate);
                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    //ds.Tables[0].TableName = "Transaction";
                    //ds.Tables[1].TableName = "Retailer";
                    //ds.Tables
                    var Json = JsonConvert.SerializeObject(ds.Tables[0]);
                    var profits = JsonConvert.DeserializeObject<List<DL_ProfitSummarys>>(Json);
                    Double totalProfit = 0;
                    double totalAmount = 0;
                    foreach (var item in profits)
                    {
                        totalAmount += Convert.ToDouble(item.Amount);
                        totalProfit += Convert.ToDouble(item.UserProfit);
                    }
                    profitReturn = new DL_ProfitSummaryReturn() { dL_ProfitSummarys = profits, TotalAmount = totalAmount, TotalProfit = totalProfit };

                }

            }
            catch (Exception ex)
            {
                _IsSuccess = false;
                Logger.WriteLog(LogLevelL4N.ERROR, "DL_ProfitSummaryReturn | Exception : " + ex.Message);
            }

            return profitReturn;
        }

        #endregion

        #region Mobile Registration
        //Mobile Regis
        public DL_MobileRegistrationReturn MobileRegistration(DL_MobileRegistration mobileReg)
        {
            this.SpName = DL_StoreProcedure.SP_InsertRegisterMobile; //Sp Name
            mobileRegReturn = new DL_MobileRegistrationReturn();
            _IsSuccess = true;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@Name", mobileReg.Name);
                param[1] = new SqlParameter("@UserType", mobileReg.Key);//change in future
                param[2] = new SqlParameter("@MobileNo", mobileReg.Mobile);
                param[3] = new SqlParameter("@UserId", mobileReg.UserId);

                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    //ds.Tables
                    var Json = JsonConvert.SerializeObject(ds.Tables[0]).TrimEnd(']').TrimStart('[');
                    mobileRegReturn = JsonConvert.DeserializeObject<DL_MobileRegistrationReturn>(Json);
                    string message = "Dear " + mobileReg.Name + " , Mobile No.  " + mobileReg.Mobile + " has been successfully registered. CREBIT Customer Experience Team.";
                    BL_SMS.SendSMS(mobileReg.Mobile, message);
                }

            }
            catch (Exception ex)
            {
                Exception err = ex;
                _IsSuccess = false;
            }

            return mobileRegReturn;
        }


        #endregion

        #region TransferFund
        //Fund Transfer 
        public List<DL_TransferFundReturn> TransferFund(DL_TransferFund fund)
        {
            this.SpName = DL_StoreProcedure.SP_DHS_API_TransferFund; //Sp Name
            fundReturn = new List<DL_TransferFundReturn>();
            _IsSuccess = true;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@UserId", fund.UserId);
                param[1] = new SqlParameter("@MobileTo", fund.MobileTo);
                param[2] = new SqlParameter("@Amount", fund.Amount);
                param[3] = new SqlParameter("@Date", indianTime);
                param[4] = new SqlParameter("@Source", 1);

                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    //ds.Tables
                    var Json = JsonConvert.SerializeObject(ds.Tables[0]);
                    fundReturn = JsonConvert.DeserializeObject<List<DL_TransferFundReturn>>(Json);
                    string reMobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                    string status = ds.Tables[0].Rows[0]["Status"].ToString();
                    if (status == "2" || status == "1")
                    {
                        //Added Task for SMS message 
                        //Modified : SMS template 
                        //By : Ranjeet | 12-Dec-14
                        Task t = new Task(() =>
                        {
                            //Rs. ##Field## has been successfully transfer to Mobile No. ##Field## User. Crebit Customer Experience Team.
                            string message = "Rs. " + fund.Amount + " has been successfully transfer to Mobile No. " + fund.MobileTo + " User. Crebit Customer Experience Team.";
                            //string message = "Rs." + fund.Amount + "has been successfully transfer to Mobile No. " + fund.MobileTo + " User. Crebit Customer Experience Team.";
                            BL_SMS.SendSMS(reMobile, message);
                            //Rs. ##Field# #has been successfully received from Mobile No. ##Field## User. Crebit Customer Experience Team.
                            message = "Rs. " + fund.Amount + " has been successfully received from Mobile No. " + reMobile + " User. Crebit Customer Experience Team.";
                            BL_SMS.SendSMS(fund.MobileTo, message);
                        });
                        t.Start();//Run task
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Error @ TransferFund : " + ex.Message);
                _IsSuccess = false;
            }

            return fundReturn;
        }
        #endregion

        #region Electricity

        //Electricity Request
        public DL_ElectricityReturn PayElectrictyBills(DL_Electricity electricity)
        {
            try
            {
                #region Check Account available balance and servie status.

                this.SpName = DL_StoreProcedure.SP_DHS_API_AvailableBalanceNServieStatus; //Sp Name || Get the Available balance and Service status.
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@UserId", electricity.UserId);
                p[1] = new SqlParameter("@OpId", electricity.ServiceId);

                ds = db.GetDataSet(this.SpName, p);
                UserInfo userInfo = null;

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataRow drc = ds.Tables[0].Rows[0];

                    userInfo = new UserInfo()
                    {
                        AvailBal = Convert.ToDouble(drc["AvailableBalance"] != null ? drc["AvailableBalance"] : 0),
                        Margin = Convert.ToDouble(drc["Margin"]),
                        ServiceStatus = Convert.ToBoolean(drc["ServiceStatus"]),
                        UType = Convert.ToInt32(drc["UType"]),
                        IsFixedCharge = Convert.ToBoolean(drc["IsFixedCharge"])
                    };
                }

                #endregion
                double _fixedCharge = 2;
                if (userInfo.IsFixedCharge)
                    _fixedCharge = (userInfo.Margin);

                if (userInfo.AvailBal >= (electricity.Amount + _fixedCharge)) //Margin will be fixed amount in this case
                {
                    this.SpName = DL_StoreProcedure.SP_DHS_API_PayElectricity; //Sp Name
                    electricityReturn = new DL_ElectricityReturn();
                    _IsSuccess = true;
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@UserId", electricity.UserId.Trim());
                    param[1] = new SqlParameter("@Key", electricity.Key);
                    param[2] = new SqlParameter("@ServiceId", electricity.ServiceId);
                    param[3] = new SqlParameter("@Amount", electricity.Amount);
                    param[4] = new SqlParameter("@BU", electricity.BU);
                    param[5] = new SqlParameter("@CusAcc", electricity.CusAcc);
                    param[6] = new SqlParameter("@CusMob", electricity.CusMob);
                    param[7] = new SqlParameter("@CyDiv", electricity.CyDiv);
                    param[8] = new SqlParameter("@DueDate", electricity.DueDate);
                    param[9] = new SqlParameter("@Date", indianTime);
                    // param[10] = new SqlParameter("@Charge", _fixedCharge);

                    ds = db.GetDataSet(this.SpName, param);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        electricityReturn.AvaiBal = Convert.ToDouble(dr["AvaiBal"]);
                        electricityReturn.Status = Convert.ToInt32(dr["Status"]);
                    }

                }
                else //not enough balance
                    electricityReturn = new DL_ElectricityReturn() { AvaiBal = userInfo.AvailBal, Status = 2 };
                this._IsSuccess = true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Error @ PayElectrictyBills : " + ex.Message);
                _IsSuccess = false;
            }

            return electricityReturn;
        }
        #endregion

        #region Account Registration
        public DL_AccountRegistrationReturn AccountRegistration(DL_AccountRegistration accountReg)
        {
            this.SpName = DL_StoreProcedure.SP_DHS_API_BankRegistration; //Sp Name
            accountRegReturn = new DL_AccountRegistrationReturn();
            _IsSuccess = true;
            try
            {
                int otp = (new Random(1000)).Next(9999);
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@Name", accountReg.Name);
                param[1] = new SqlParameter("@Key", accountReg.Key);//change in future
                param[2] = new SqlParameter("@Mobile", accountReg.Mobile);
                param[3] = new SqlParameter("@UserId", accountReg.UserId);
                param[4] = new SqlParameter("@Account", accountReg.Account);
                param[5] = new SqlParameter("@IFSC", accountReg.IFSC);
                param[6] = new SqlParameter("@OTP", otp);
                param[7] = new SqlParameter("@Amount", accountReg.Amount);

                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    var row = ds.Tables[0].Rows[0];
                    accountRegReturn = new DL_AccountRegistrationReturn { status = row["status"].ToString() };
                    //  string message = "Dear " + accountReg.Name + " , Account No.  " + accountReg.Account + " has been successfully registered. CREBIT Customer Experience Team.";

                    //Non Blocking thread
                    //Task t = new Task(() => BL_SMS.SendSMS(accountReg.Mobile, message));
                    //t.Start();
                }
            }
            catch (Exception ex)
            {
                Exception err = ex;
                _IsSuccess = false;
            }

            return accountRegReturn;
        }



        #endregion

        #region Bank Account Transfer req.
        public DL_BankTransReqReturn BankTransferReq(DL_BankTransReq bankReq)
        {
            this.SpName = DL_StoreProcedure.SP_DHS_API_BankTransReq; //Sp Name
            bankReqReturn = new DL_BankTransReqReturn();
            _IsSuccess = true;
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@UserId", bankReq.UserId);
                param[1] = new SqlParameter("@Account", bankReq.Account);
                param[2] = new SqlParameter("@IFSC", bankReq.IFSC);
                param[3] = new SqlParameter("@CusName", bankReq.Name);
                param[4] = new SqlParameter("@Amount", bankReq.Amount);
                param[5] = new SqlParameter("@Mobile", bankReq.Mobile);
                param[6] = new SqlParameter("@Date", indianTime);

                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    var row = ds.Tables[0].Rows[0];
                    bankReqReturn = new DL_BankTransReqReturn { Status = row["Status"].ToString(), AvailableBalance = row["AvailableBalance"].ToString(), RefId = row["RefId"].ToString() };
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Error @ BankTransferReq : " + ex.Message);
                _IsSuccess = false;
            }

            return bankReqReturn;

        }

        #endregion

        #region Refund or Trans Status

        //Modified: Ranjeet | 26-Dec | Fixed recharger status bug
        public DL_RefundOrTransStatusReturn RefundorTransStatus(DL_RefundOrTransStatus dL_RefundOrTransStatus)
        {
            this.SpName = DL_StoreProcedure.SP_DHS_API_RefundorTransStatus; //Sp Name
            DL_RefundOrTransStatusReturn dL_RefundOrTransStatusReturn = null;
            _IsSuccess = true;
            try
            {
                //For Refund
                if (dL_RefundOrTransStatus.TypeId == 1)
                {
                    SqlParameter[] param = new SqlParameter[5];
                    param[0] = new SqlParameter("@UserId", dL_RefundOrTransStatus.UserId);
                    param[1] = new SqlParameter("@TransId", dL_RefundOrTransStatus.TransId);
                    param[2] = new SqlParameter("@Date", indianTime);
                    param[3] = new SqlParameter("@Comments", dL_RefundOrTransStatus.Comments);
                    param[4] = new SqlParameter("@TypeId", dL_RefundOrTransStatus.TypeId);

                    ds = db.GetDataSet(this.SpName, param);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        var row = ds.Tables[0].Rows[0];
                        dL_RefundOrTransStatusReturn = new DL_RefundOrTransStatusReturn
                        {
                            Status = Convert.ToInt32(row["Status"]),
                            CyberTransId = row["CyberTransId"].ToString(),
                            OperatorId = Convert.ToInt32(row["OperatorId"])
                        };
                    }
                }
                //check cyber status
                else if (dL_RefundOrTransStatus.TypeId == 2)
                {
                    BL_Operator bl = new BL_Operator();
                    CyberPlateStatus cyberPlateStatus = bl.GetSessionId(dL_RefundOrTransStatus.TransId); //Added | Ranjeet |26-Dec |Get cyber Session and Opid

                    DL_SessionCyberPlateStatusReturn dL_SessionCyberPlateStatusReturn = bl.GetCyberPlateStatus(
                        new DL_SessionCyberPlateStatus() { TransactionId = dL_RefundOrTransStatus.TransId });
                    dL_RefundOrTransStatusReturn = new DL_RefundOrTransStatusReturn(); //Added | Ranjeet | to fix Null Exception 
                    dL_RefundOrTransStatusReturn.CyberTransId = dL_SessionCyberPlateStatusReturn.TransId;
                    dL_RefundOrTransStatusReturn.Message = dL_SessionCyberPlateStatusReturn.CyberCode + " : " + dL_SessionCyberPlateStatusReturn.Message;
                    dL_RefundOrTransStatusReturn.Status = Convert.ToInt32(dL_SessionCyberPlateStatusReturn.Status);
                }
                dL_RefundOrTransStatusReturn.TypeId = dL_RefundOrTransStatus.TypeId;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Error @ RefundorTransStatus : " + ex.Message);
                _IsSuccess = false;

            }

            return dL_RefundOrTransStatusReturn;
        }

        #endregion
    }
}