using System;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using com.dhs.webapi.Model.DL.Common;
using com.dhs.webapi.Model.BL.Common;
using System.Collections.Generic;
using api.dhs.Logging;
using com.dhs.webapi.Model.Common;

//use for Transaction | Service | Balance Use | Profit | Mobile Regis | Other Details for User
namespace api.dhs.Controllers
{
    public class DashBoardController : ApiController
    {
        BL_DashBoard dash = new BL_DashBoard();//Bl class

        [Route("dashboard/tranDetails")]
        [HttpPost]
        public HttpResponseMessage TransactionDetails(HttpRequestMessage req, DL_Transaction trans)
        {
            User user = new User() { Password = trans.Key, UserId = trans.UserId };
            Validation.UserCheck(user);

            if (Validation._IsSuccess)
            {
                try
                {
                    // Logger.WriteLog(LogLevelL4N.INFO, "Params | UserId : " + trans.UserId + "UserType :" + trans.Pass + " FromDate : " + trans.FromDate + " ToDate :" + trans.ToDate);
                    if (trans != null && !string.IsNullOrEmpty(trans.UserId) && !string.IsNullOrEmpty(trans.Key))
                    {
                        if (string.IsNullOrEmpty(trans.FromDate) && string.IsNullOrEmpty(trans.ToDate))
                        {
                            trans.ToDate = null;
                            trans.FromDate = null;
                        }

                        DL_TransactionReturn transReturn = dash.TransactionSummary(trans);
                        if (dash._IsSuccess)
                            return req.CreateResponse<DL_TransactionReturn>(HttpStatusCode.OK, transReturn);
                        else
                            return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                    }
                    Logger.WriteLog(LogLevelL4N.FATAL, "Bad Request");
                    return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(LogLevelL4N.ERROR, "Inside the transactionDetails api | Error : " + ex.Message);
                }
            }
            Logger.WriteLog(LogLevelL4N.FATAL, "Unauthorized Request");
            return req.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
        }

        [Route("dashboard/tranSearch")]
        [HttpPost]
        public HttpResponseMessage TransactionSearch(HttpRequestMessage req, DL_TransactionSearch tranSearch)
        {
            User user = new User() { Password = tranSearch.Key, UserId = tranSearch.UserId };
            Validation.UserCheck(user);

            if (Validation._IsSuccess)
            {
                try
                {
                    // Logger.WriteLog(LogLevelL4N.INFO, "Params | UserId : " + trans.UserId + "UserType :" + trans.Pass + " FromDate : " + trans.FromDate + " ToDate :" + trans.ToDate);
                    if (tranSearch != null && !string.IsNullOrEmpty(tranSearch.UserId) && !string.IsNullOrEmpty(tranSearch.Key))
                    {

                        if (string.IsNullOrEmpty(tranSearch.Value))
                            tranSearch.Value = null;
                        if (tranSearch.TypeId < 0)
                            tranSearch.TypeId = 0;


                        DL_TransactionReturn tranSearchReturn = dash.TransactionSearch(tranSearch);
                        if (dash._IsSuccess)
                            return req.CreateResponse<DL_TransactionReturn>(HttpStatusCode.OK, tranSearchReturn);
                        else
                            return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                    }
                    Logger.WriteLog(LogLevelL4N.FATAL, "Bad Request");
                    return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(LogLevelL4N.ERROR, "Inside the transactionDetails api | Error : " + ex.Message);
                }
            }
            Logger.WriteLog(LogLevelL4N.FATAL, "Unauthorized Request");
            return req.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
        }

        [Route("dashboard/service")]
        [HttpPost]
        public HttpResponseMessage Service(HttpRequestMessage req, DL_Service service)
        {
            User user = new User() { Password = service.Key, UserId = service.UserId };
            Validation.UserCheck(user);
            if (Validation._IsSuccess)
            {
                if (service != null && !string.IsNullOrEmpty(service.Amount.ToString()) && !string.IsNullOrEmpty(service.Number) && service.OperatorId > 0
                       && !string.IsNullOrEmpty(service.Source) && !string.IsNullOrEmpty(service.UserId) && !string.IsNullOrEmpty(service.Key))
                {
                    try
                    {

                        //Modify: Ranjeet | 12-Dec|| Moved if condtion above try/catch
                        //  {
                        DL_ServiceReturn serviceReturn = dash.Service(service);//call to process the transaction.
                        if (dash._IsSuccess)
                            return req.CreateResponse<DL_ServiceReturn>(HttpStatusCode.OK, serviceReturn);
                        else
                            return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                        //}
                        //Logger.WriteLog(LogLevelL4N.FATAL, "Bad Request");
                        //return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(LogLevelL4N.ERROR, "Inside the Service api | Error : " + ex.Message);
                    }
                }
                else
                {
                    Logger.WriteLog(LogLevelL4N.FATAL, "Bad Request");
                    return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
            }

            Logger.WriteLog(LogLevelL4N.FATAL, "Unauthorized Request");
            return req.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
        }

        [Route("dashboard/balanceUse")]
        [HttpPost]
        public HttpResponseMessage BalanceUseSummary(HttpRequestMessage req, DL_BalanceUseSummary balance)
        {
            User user = new User() { Password = balance.Key, UserId = balance.UserId };
            Validation.UserCheck(user);
            if (Validation._IsSuccess)
            {
                try
                {
                    if (balance != null && !string.IsNullOrEmpty(balance.UserId) && !string.IsNullOrEmpty(balance.Key))
                    {
                        if (string.IsNullOrEmpty(balance.FromDate) && string.IsNullOrEmpty(balance.ToDate))
                        {
                            balance.ToDate = null;
                            balance.FromDate = null;
                        }
                        if (string.IsNullOrEmpty(balance.Value))
                            balance.Value = null;

                        DL_BalanceUseSummaryReturn balanceReturn = dash.BalanceUseSummary(balance);
                        if (dash._IsSuccess)
                            return req.CreateResponse<DL_BalanceUseSummaryReturn>(HttpStatusCode.OK, balanceReturn);
                        else
                            return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                    }

                    return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");

                }
                catch (Exception ex)
                {
                    Logger.WriteLog(LogLevelL4N.ERROR, "Inside the BalanceUseSummary api | Error : " + ex.Message);
                    return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                }
            }
            Logger.WriteLog(LogLevelL4N.FATAL, "Unauthorized Request");
            return req.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
        }

        [Route("dashboard/profit")]
        [HttpPost]
        public HttpResponseMessage ProfitSummary(HttpRequestMessage req, DL_ProfitSummary profit)
        {
            Logger.WriteLog(LogLevelL4N.ERROR, "Got ProfitSummary req.");
            User user = new User() { Password = profit.Key, UserId = profit.UserId };
            Validation.UserCheck(user);
            if (Validation._IsSuccess)
            {
                try
                {
                    if (profit != null && !string.IsNullOrEmpty(profit.UserId) && !string.IsNullOrEmpty(profit.Key))
                    {
                        //Logger.WriteLog(LogLevelL4N.INFO, "Process request |  UserId : " + profit.UserId + " UserType : " + profit.Key);
                        DL_ProfitSummaryReturn profitSumReturn = dash.ProfitSummary(profit);
                        if (dash._IsSuccess)
                            return req.CreateResponse<DL_ProfitSummaryReturn>(HttpStatusCode.OK, profitSumReturn);
                        else
                            return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                    }
                    Logger.WriteLog(LogLevelL4N.FATAL, "Bad Request");
                    return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(LogLevelL4N.ERROR, "Inside the ProfitSummary api | Error : " + ex.Message);
                }
            }
            Logger.WriteLog(LogLevelL4N.FATAL, "Unauthorized Request");
            return req.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
        }

        [Route("dashboard/mobileRegistration")]
        [HttpPost]
        public HttpResponseMessage MobileRegistration(HttpRequestMessage req, DL_MobileRegistration mobileReg)
        {
            Logger.WriteLog(LogLevelL4N.INFO, "Got  MobileRegistration req. ");
            User user = new User() { Password = mobileReg.Key, UserId = mobileReg.UserId };
            Validation.UserCheck(user);
            if (Validation._IsSuccess)
            {
                try
                {
                    if (mobileReg != null && !string.IsNullOrEmpty(mobileReg.Name) && !string.IsNullOrEmpty(mobileReg.UserId)
                        && !string.IsNullOrEmpty(mobileReg.Mobile) && !string.IsNullOrEmpty(mobileReg.Key))
                    {
                        // Logger.WriteLog(LogLevelL4N.INFO, "Process request | Mobile :" + mobileReg.Mobile + " UserId : " + mobileReg.UserId + " UserType : " + mobileReg.Key);
                        DL_MobileRegistrationReturn mobileRegReturn = dash.MobileRegistration(mobileReg);
                        if (dash._IsSuccess)
                            return req.CreateResponse<DL_MobileRegistrationReturn>(HttpStatusCode.OK, mobileRegReturn);
                        else
                            return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                    }
                    Logger.WriteLog(LogLevelL4N.FATAL, "Bad Request");
                    return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(LogLevelL4N.ERROR, "Inside the MobileRegistration api | Error : " + ex.Message);
                }
            }
            Logger.WriteLog(LogLevelL4N.FATAL, "Unauthorized Request");
            return req.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
        }

        [Route("dashboard/transfer")]
        [HttpPost]
        public HttpResponseMessage TransferFund(HttpRequestMessage req, DL_TransferFund fund)
        {
            if (fund != null && !string.IsNullOrEmpty(fund.Amount) && Convert.ToInt32(fund.Amount) > 0 && !string.IsNullOrEmpty(fund.Key) && !string.IsNullOrEmpty(fund.UserId)
               && !string.IsNullOrEmpty(fund.MobileTo))
            {
                User user = new User() { Password = fund.Key, UserId = fund.UserId };
                Validation.UserCheck(user);
                if (Validation._IsSuccess)
                {
                    try
                    {
                        if (fund != null && !string.IsNullOrEmpty(fund.UserId) && !string.IsNullOrEmpty(fund.Key)
                            && !string.IsNullOrEmpty(fund.MobileTo) && !string.IsNullOrEmpty(fund.Amount))
                        {
                            List<DL_TransferFundReturn> fundReturn = dash.TransferFund(fund);
                            if (dash._IsSuccess)
                                return req.CreateResponse<List<DL_TransferFundReturn>>(HttpStatusCode.OK, fundReturn);
                            else
                                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                        }
                        Logger.WriteLog(LogLevelL4N.FATAL, "Bad Request");
                        return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(LogLevelL4N.ERROR, "Inside the TransferFund api | Error : " + ex.Message);
                    }
                }

                Logger.WriteLog(LogLevelL4N.FATAL, "Unauthorized Request");
                return req.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }
            Logger.WriteLog(LogLevelL4N.FATAL, "Bad Request");
            return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
        }
        
        //pay ele bill req.
        [Route("dashboard/electricity")]
        [HttpPost]
        public HttpResponseMessage Electricity(HttpRequestMessage req, DL_Electricity electricity)
        {

            if (electricity != null && !string.IsNullOrEmpty(electricity.UserId) && !string.IsNullOrEmpty(electricity.Key)
                    && !string.IsNullOrEmpty(electricity.CusAcc) && electricity.Amount > 0
                    && !string.IsNullOrEmpty(electricity.DueDate) && electricity.ServiceId > 0 && !string.IsNullOrEmpty(electricity.CusMob))
            {
                try
                {
                    User user = new User() { Password = electricity.Key, UserId = electricity.UserId };
                    //validate user
                    Validation.UserCheck(user);
                    if (Validation._IsSuccess)
                    {
                        DL_ElectricityReturn electricityReturn = dash.PayElectrictyBills(electricity);
                        if (dash._IsSuccess)
                            return req.CreateResponse<DL_ElectricityReturn>(HttpStatusCode.OK, electricityReturn);
                        else
                            return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                    }
                    Logger.WriteLog(LogLevelL4N.FATAL, "Unauthorized Request");
                    return req.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(LogLevelL4N.ERROR, "Inside the TransferFund api | Error : " + ex.Message);
                    Logger.WriteLog(LogLevelL4N.FATAL, "Bad Request");
                    return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
            }

            Logger.WriteLog(LogLevelL4N.FATAL, "Bad Request");
            return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
        }

        [Route("dashboard/accountRegistration")] //acting as account pay
        [HttpPost]
        public HttpResponseMessage AccountRegistration(HttpRequestMessage req, DL_AccountRegistration accountReg)
        {
            User user = new User() { Password = accountReg.Key, UserId = accountReg.UserId };
            Validation.UserCheck(user);
            if (Validation._IsSuccess)
            {
                try
                {
                    if (accountReg != null && !string.IsNullOrEmpty(accountReg.Name) && !string.IsNullOrEmpty(accountReg.UserId)
                        && !string.IsNullOrEmpty(accountReg.Mobile) && !string.IsNullOrEmpty(accountReg.Key))
                    {
                        DL_AccountRegistrationReturn accountRegReturn = dash.AccountRegistration(accountReg);
                        if (dash._IsSuccess)
                            return req.CreateResponse<DL_AccountRegistrationReturn>(HttpStatusCode.OK, accountRegReturn);
                        else
                            return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                    }
                    Logger.WriteLog(LogLevelL4N.FATAL, "Bad Request");
                    return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(LogLevelL4N.ERROR, "Inside the Account Registration api | Error : " + ex.Message);
                    return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
            }
            Logger.WriteLog(LogLevelL4N.FATAL, "Unauthorized Request");
            return req.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
        }

        [Route("dashboard/bankReq")]
        [HttpPost]
        public HttpResponseMessage BankTransReq(HttpRequestMessage req, DL_BankTransReq bankTransReq)
        {
            try
            {
                User user = new User() { Password = bankTransReq.Key, UserId = bankTransReq.UserId };
                Validation.UserCheck(user);
                if (Validation._IsSuccess)
                {

                    if (bankTransReq != null && bankTransReq.Amount > 0 && !string.IsNullOrEmpty(bankTransReq.UserId)
                        && !string.IsNullOrEmpty(bankTransReq.IFSC) && !string.IsNullOrEmpty(bankTransReq.Key) && !string.IsNullOrEmpty(bankTransReq.Mobile))
                    {
                        DL_BankTransReqReturn accountRegReturn = dash.BankTransferReq(bankTransReq);
                        if (dash._IsSuccess)
                            return req.CreateResponse<DL_BankTransReqReturn>(HttpStatusCode.OK, accountRegReturn);
                        else
                            return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                    }
                    Logger.WriteLog(LogLevelL4N.FATAL, "Bad Request");
                    return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Inside the Account Registration api | Error : " + ex.Message);
                return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
            Logger.WriteLog(LogLevelL4N.FATAL, "Unauthorized Request");
            return req.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
        }

        [Route("dashboard/refundOrTransStatus")]
        [HttpPost]
        public HttpResponseMessage RefundOrTransStatus(HttpRequestMessage req, DL_RefundOrTransStatus dL_RefundOrTransStatus)
        {
            try
            {
                User user = new User() { Password = dL_RefundOrTransStatus.Key, UserId = dL_RefundOrTransStatus.UserId };
                Validation.UserCheck(user);
                if (Validation._IsSuccess)
                {

                    if (dL_RefundOrTransStatus != null && !string.IsNullOrEmpty(dL_RefundOrTransStatus.UserId)
                        && (dL_RefundOrTransStatus.TypeId > 0) && !string.IsNullOrEmpty(dL_RefundOrTransStatus.TransId))
                    {
                        DL_RefundOrTransStatusReturn dL_RefundOrTransStatusReturn = dash.RefundorTransStatus(dL_RefundOrTransStatus);
                        if (dash._IsSuccess)
                            return req.CreateResponse<DL_RefundOrTransStatusReturn>(HttpStatusCode.OK, dL_RefundOrTransStatusReturn);
                        else
                            return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                    }
                    Logger.WriteLog(LogLevelL4N.FATAL, "Bad Request");
                    return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Inside the RefundOrTransStatus api | Error : " + ex.Message);
                return req.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
            Logger.WriteLog(LogLevelL4N.FATAL, "Unauthorized Request");

            return req.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
        }//end of Class
    }
}