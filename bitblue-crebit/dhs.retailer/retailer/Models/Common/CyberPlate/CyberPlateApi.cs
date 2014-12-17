/*Author: Ranjeet kumar
 * 
 * Date- 03-July-14
 * Used for Cyber plate transaction process
 * Step-1 ....Validate the Number /Request
 * Step-2 .... Go for payment
 * Step-3 ...Check for payment status
 */
using api.dhs.Logging;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace com.dhs.webapi.Model.Common.CyberPlate
{
    #region Response Json
    public class ProcessReqReturn
    {
        public string SessionId { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
    #endregion

    public class RechargeProcess
    {
        #region Memeber variables

        private static readonly object LockObject = new object();
        //  private string _responseCode = "0";
        //  private string _responseDescription = "Ok";
        public string TransationId { get; set; }
        public string CustomerMobile { get; set; }
        //public string SubscriberMobile { get; set; }
        public string Amount { get; set; }
        // public string Account { get; set; }
        private string SD { get; set; }
        private string AP { get; set; }
        private string OP { get; set; }
        private string TERM_ID { get; set; }
        private string KeyPath { get; set; }
        private string Key { get; set; }
        // private string SubscriberMobile { get; set; }
        private string Account { get; set; }
        #endregion

        public RechargeProcess()
        {
            #region App Config Values

            NameValueCollection appSettingColl = ConfigurationManager.AppSettings;
            SD = appSettingColl["SD"];
            AP = appSettingColl["AP"];
            OP = appSettingColl["OP"];
            TERM_ID = appSettingColl["TERM_ID"];//added By Ranjeet || for Airtel Operators only
            KeyPath = appSettingColl["keyPath"];
            Key = appSettingColl["key"];
            Account = appSettingColl["Account"];
            #endregion
        }
        #region Process Transaction
        /// <summary>
        /// Send the request for to cyber plate and check the status .
        /// return on  success :  Transactionid and status code
        /// </summary>
        /// <param name="productId">used for url</param>
        /// <param name="mobileNumber">Customer Mobile number</param>
        /// <param name="amount">amount to be recharge</param>
        /// <param name="account">account registe with cyber plate</param>
        /// <returns></returns>
        public ProcessReqReturn ProcessRequest(int operatorId, string mobileNumber, double amount, string account)
        {
            try
            {
                ProcessReqReturn prt = new ProcessReqReturn();
                string session = GenerateRandomSession();//get the Unique random seesion number
                prt.SessionId = session;//store session ||Added by Ranjeet ||01-Nov || To check pay status from cyber plate


                //Get the input message for Cyber plate
                string inputMessageforValidateNumber = GetInputMessage(mobileNumber, amount, session, operatorId, account);

                //validate the request for number|| Subscriber
                string validateResponse = RechargeProcess.Process(Verification_OperatorRequestUrl(operatorId), inputMessageforValidateNumber);

                //error
                if (validateResponse.IndexOf("ERROR=0") == -1 || validateResponse.IndexOf("RESULT=0") == -1)
                {
                    prt.Message = "Validation Error- " + this.GetErrorDescription(validateResponse);
                    prt.StatusCode = -1;
                    //Added: Ranjeet | 16-Dec || For logging 
                    Logger.WriteLog(LogLevelL4N.WARN, "cyber-plate error : " + prt.Message);
                    Logger.WriteLog(LogLevelL4N.WARN, "Validation Error : " + inputMessageforValidateNumber);
                    return prt;
                }
                //success----go for payment.
                else
                {
                    string strinputMsgPayment = GetInputMessageForPayment(mobileNumber, amount, session, operatorId, Account);
                    string paymentResponse = RechargeProcess.Process(Payment_OperatorRequestUrl(operatorId), strinputMsgPayment);
                    if (paymentResponse.IndexOf("ERROR=0") == -1 || paymentResponse.IndexOf("RESULT=0") == -1)
                    {
                        prt.Message = "Payment Error- " + this.GetErrorDescription(paymentResponse);
                        prt.StatusCode = -1;
                        //Added: Ranjeet | 16-Dec || For logging 
                        Logger.WriteLog(LogLevelL4N.WARN, "cyberplate-error  : " + prt.Message);
                        Logger.WriteLog(LogLevelL4N.WARN, "Payment Error : " + strinputMsgPayment);
                        return prt;
                    }
                    else //success---Check transaction status.
                    {
                        int num = paymentResponse.LastIndexOf("TRANSID=");
                        TransationId = paymentResponse.Substring(num).Split(new char[1] { '\r' })[0].Split('=')[1];
                        string strinputMsgStatus = GetInputMessageForStatus(session, operatorId, Account);
                        string statusResponse = RechargeProcess.Process(Status_OperatorRequestUrl(operatorId), strinputMsgStatus);
                        if (statusResponse.IndexOf("ERROR=0") != -1 && statusResponse.IndexOf("RESULT=7") != -1)
                        {
                            prt.TransactionId = TransationId;
                            prt.StatusCode = 1;
                            prt.Message = "Successfully recharged.";
                            return prt;
                        }
                        else
                        {
                            //Added: Ranjeet | 16-Dec || For logging 
                            Logger.WriteLog(LogLevelL4N.WARN, "Payment Status || Not Success|| : " + strinputMsgStatus);
                            for (int i = 1; i < 7; i++)
                            {
                                if (statusResponse.IndexOf("ERROR=0") != -1 && statusResponse.IndexOf("RESULT=" + i) != -1)
                                {
                                    prt.TransactionId = TransationId;
                                    prt.StatusCode = 1;
                                    prt.Message = "Payment has been transferred to the Service Provider (the transaction is in pending).TransactionId : " + TransationId;
                                    Logger.WriteLog(LogLevelL4N.WARN, "cyber-plate  :prt.Message :" + this.GetErrorDescription(statusResponse));
                                    return prt;
                                }
                            }
                            if (statusResponse.IndexOf("ERROR=0") != -1 && statusResponse.IndexOf("RESULT=1") != -1 || statusResponse.IndexOf("RESULT=0") != -1)
                            {
                                prt.TransactionId = TransationId;
                                prt.Message = "Please try again.";
                                prt.StatusCode = -1; //Send status -1 as payment is not successfull.
                                Logger.WriteLog(LogLevelL4N.WARN, "cyber-plate status error :1 :" + this.GetErrorDescription(statusResponse));
                                return prt;
                            }

                            else
                            {
                                prt.TransactionId = TransationId;
                                prt.Message = "Status : ||  " + this.GetErrorDescription(statusResponse);
                                prt.StatusCode = -1; //Send status 1 as payment is not successfull.
                                Logger.WriteLog(LogLevelL4N.WARN, "cyber-plate status error : " + this.GetErrorDescription(statusResponse));
                                return prt;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { Logger.WriteLog(LogLevelL4N.ERROR, "Error @ProcessRequest : : " + ex.Message); }
            return null;

        }
        #endregion

        #region Commented Code

        #region Coomented |Used for  Test || Request url ( 1 -SubsCriber Number Validation, 2- Payment | 3- Status check for the transaction)
        ////Get the url for the Oprator || 1 -SubsCriber Number Validation, 2- Payment | 3- Status check for the transaction
        //private static string GetRequestUrl(int operatorId)
        //{

        //    string str = string.Empty;
        //    switch (operatorId)
        //    {
        //        case 1:
        //            {
        //                //for Number validation:
        //                str = "http://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay_check.cgi";
        //                break;
        //            }
        //        case 2:
        //            {
        //                //Payment:
        //                str = "http://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay.cgi";
        //                break;
        //            }
        //        case 3:
        //            {
        //                //Status:
        //                str = "http://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay_status.cgi";
        //                break;
        //            }
        //        default:
        //            {
        //                str = "http://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay_check.cgi";
        //                break;
        //            }

        //    }
        //    #region Commented Code for operator url || India
        //    //switch (operatorId)
        //    //{
        //    //    case 0:
        //    //        str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_check.cgi/205";
        //    //        break;
        //    //    case 1:
        //    //        str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay_check.cgi";
        //    //        break;
        //    //    case 2:
        //    //        str = "http://ru-demo.cyberplat.com/cgi-bin/DealerSertification/de_pay_check.cgi";///"https://in.cyberplat.com/cgi-bin/ac/ac_pay_check.cgi/1";
        //    //        break;
        //    //    case 3:
        //    //        str = "https://in.cyberplat.com/cgi-bin/mt/mt_pay_check.cgi";
        //    //        break;
        //    //    case 4:
        //    //        str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay_check.cgi";
        //    //        break;
        //    //    case 5:
        //    //        str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_check.cgi";
        //    //        break;
        //    //    case 6:
        //    //        str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_check.cgi";
        //    //        break;
        //    //    case 7:
        //    //        str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_check.cgi/219";
        //    //        break;
        //    //    case 8:
        //    //        str = "https://in.cyberplat.com/cgi-bin/vd/vd_pay_check.cgi";
        //    //        break;
        //    //    case 9:
        //    //        str = "https://in.cyberplat.com/cgi-bin/un/un_pay_check.cgi";
        //    //        break;
        //    //    case 10:
        //    //        str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_check.cgi/212";
        //    //        break;
        //    //    case 11:
        //    //        str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_check.cgi/215";
        //    //        break;
        //    //    case 12:
        //    //        str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay_check.cgi";
        //    //        break;
        //    //    case 13:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bt/bt_pay_check.cgi";
        //    //        break;
        //    //    case 14:
        //    //        str = "https://in.cyberplat.com/cgi-bin/dt/dt_pay_check.cgi";
        //    //        break;
        //    //    case 15:
        //    //        str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_check.cgi/213";
        //    //        break;
        //    //    case 16:
        //    //        str = "https://in.cyberplat.com/cgi-bin/vc/vc_pay_check.cgi";
        //    //        break;
        //    //    case 17:
        //    //        str = "https://in.cyberplat.com/cgi-bin/ts/ts_pay_check.cgi";
        //    //        break;
        //    //    case 18:
        //    //        str = "https://in.cyberplat.com/cgi-bin/id/id_pay_check.cgi";
        //    //        break;
        //    //    case 19:
        //    //        str = "https://in.cyberplat.com/cgi-bin/at/at_pay_check.cgi/209";
        //    //        break;
        //    //    case 20:
        //    //        str = "https://in.cyberplat.com/cgi-bin/ad/ad_pay_check.cgi";
        //    //        break;
        //    //    case 21:
        //    //        str = "https://in.cyberplat.com/cgi-bin/un/un_pay_check.cgi";
        //    //        break;
        //    //    case 22:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/228";
        //    //        break;
        //    //    case 23:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/230";
        //    //        break;
        //    //    case 24:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/233";
        //    //        break;
        //    //    case 25:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/231";
        //    //        break;
        //    //    case 26:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/232";
        //    //        break;
        //    //    case 27:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/234";
        //    //        break;
        //    //    case 28:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/239";
        //    //        break;
        //    //    case 29:
        //    //        str = "https://in.cyberplat.com/cgi-bin/vm/vm_pay_check.cgi";
        //    //        break;
        //    //    case 30:
        //    //        str = "https://in.cyberplat.com/cgi-bin/vm/vm_pay_check.cgi";
        //    //        break;
        //    //    case 31:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/235";
        //    //        break;
        //    //    case 32:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/236";
        //    //        break;
        //    //    case 33:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/237";
        //    //        break;
        //    //    case 34:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/238";
        //    //        break;
        //    //    case 35:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/241";
        //    //        break;
        //    //    case 36:
        //    //        str = "https://in.cyberplat.com/cgi-bin/lm/lm_pay_check.cgi";
        //    //        break;
        //    //    case 37:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/242";
        //    //        break;
        //    //    case 38:
        //    //        str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/243";
        //    //        break;
        //    //    case 40:
        //    //        str = "https://in.cyberplat.com/cgi-bin/ad/ad_pay_check.cgi/225";
        //    //        break;
        //    //    case 41:
        //    //        str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_check.cgi";
        //    //        break;
        //    //    case 42:
        //    //        str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_check.cgi";
        //    //        break;
        //    //    default:
        //    //        Console.WriteLine("Invalid Operator");
        //    //        break; 

        //    //}
        //    #endregion
        //    Logger.WriteLog(LogLevelL4N.INFO, "requestUrl : " + str);
        //    return str;
        //}
        #endregion

        #region Commented code for  GetPaymentRequestUrl
        //private static string GetPaymentRequestUrl(int operatorId)
        //{
        //    string str = string.Empty;
        //    switch (operatorId)
        //    {
        //        case 0:
        //            str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay.cgi/205";
        //            break;
        //        case 1:
        //            str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay.cgi";
        //            break;
        //        case 2:
        //            str = "https://in.cyberplat.com/cgi-bin/ac/ac_pay.cgi/1";
        //            break;
        //        case 3:
        //            str = "https://in.cyberplat.com/cgi-bin/mt/mt_pay.cgi";
        //            break;
        //        case 4:
        //            str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay.cgi";
        //            break;
        //        case 5:
        //            str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay.cgi";
        //            break;
        //        case 6:
        //            str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay.cgi";
        //            break;
        //        case 7:
        //            str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay.cgi/219";
        //            break;
        //        case 8:
        //            str = "https://in.cyberplat.com/cgi-bin/vd/vd_pay.cgi";
        //            break;
        //        case 9:
        //            str = "https://in.cyberplat.com/cgi-bin/un/un_pay.cgi";
        //            break;
        //        case 10:
        //            str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay.cgi/212";
        //            break;
        //        case 11:
        //            str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay.cgi/215";
        //            break;
        //        case 12:
        //            str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay.cgi";
        //            break;
        //        case 13:
        //            str = "https://in.cyberplat.com/cgi-bin/bt/bt_pay.cgi";
        //            break;
        //        case 14:
        //            str = "https://in.cyberplat.com/cgi-bin/dt/dt_pay.cgi";
        //            break;
        //        case 15:
        //            str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay.cgi/213";
        //            break;
        //        case 16:
        //            str = "https://in.cyberplat.com/cgi-bin/vc/vc_pay.cgi";
        //            break;
        //        case 17:
        //            str = "https://in.cyberplat.com/cgi-bin/ts/ts_pay.cgi";
        //            break;
        //        case 18:
        //            str = "https://in.cyberplat.com/cgi-bin/id/id_pay.cgi";
        //            break;
        //        case 19:
        //            str = "https://in.cyberplat.com/cgi-bin/at/at_pay.cgi/209";
        //            break;
        //        case 20:
        //            str = "https://in.cyberplat.com/cgi-bin/ad/ad_pay.cgi";
        //            break;
        //        case 21:
        //            str = "https://in.cyberplat.com/cgi-bin/un/un_pay.cgi";
        //            break;
        //        case 22:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/228";
        //            break;
        //        case 23:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/230";
        //            break;
        //        case 24:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/233";
        //            break;
        //        case 25:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/231";
        //            break;
        //        case 26:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/232";
        //            break;
        //        case 27:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/234";
        //            break;
        //        case 28:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/239";
        //            break;
        //        case 29:
        //            str = "https://in.cyberplat.com/cgi-bin/vm/vm_pay.cgi";
        //            break;
        //        case 30:
        //            str = "https://in.cyberplat.com/cgi-bin/vm/vm_pay.cgi";
        //            break;
        //        case 31:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/235";
        //            break;
        //        case 32:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/236";
        //            break;
        //        case 33:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/237";
        //            break;
        //        case 34:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/238";
        //            break;
        //        case 35:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/241";
        //            break;
        //        case 36:
        //            str = "https://in.cyberplat.com/cgi-bin/lm/lm_pay.cgi";
        //            break;
        //        case 37:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/242";
        //            break;
        //        case 38:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 40:
        //            str = "https://in.cyberplat.com/cgi-bin/ad/ad_pay.cgi/225";
        //            break;
        //        case 41:
        //            str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay.cgi";
        //            break;
        //        case 42:
        //            str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay.cgi";
        //            break;
        //        default:
        //            Console.WriteLine("Invalid Operator");
        //            break;
        //    }
        //    return str;
        //} 
        #endregion

        #region commented code for GetStatusCheckUrl
        //private string GetStatusCheckUrl(int operatorId)
        //{
        //    string str = string.Empty;
        //    switch (operatorId)
        //    {
        //        case 0:
        //            str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_status.cgi";
        //            break;
        //        case 1:
        //            str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay_status.cgi";
        //            break;
        //        case 2:
        //            str = "https://in.cyberplat.com/cgi-bin/ac/ac_pay_status.cgi";
        //            break;
        //        case 3:
        //            str = "https://in.cyberplat.com/cgi-bin/mt/mt_pay_status.cgi";
        //            break;
        //        case 4:
        //            str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay_status.cgi";
        //            break;
        //        case 5:
        //            str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_status.cgi";
        //            break;
        //        case 6:
        //            str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_status.cgi";
        //            break;
        //        case 7:
        //            str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_status.cgi";
        //            break;
        //        case 8:
        //            str = "https://in.cyberplat.com/cgi-bin/vd/vd_pay_status.cgi";
        //            break;
        //        case 9:
        //            str = "https://in.cyberplat.com/cgi-bin/un/un_pay_status.cgi";
        //            break;
        //        case 10:
        //            str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_status.cgi";
        //            break;
        //        case 11:
        //            str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_status.cgi";
        //            break;
        //        case 12:
        //            str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay_status.cgi";
        //            break;
        //        case 13:
        //            str = "https://in.cyberplat.com/cgi-bin/bt/bt_pay_status.cgi";
        //            break;
        //        case 14:
        //            str = "https://in.cyberplat.com/cgi-bin/dt/dt_pay_status.cgi";
        //            break;
        //        case 15:
        //            str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_status.cgi";
        //            break;
        //        case 16:
        //            str = "https://in.cyberplat.com/cgi-bin/vc/vc_pay_status.cgi";
        //            break;
        //        case 17:
        //            str = "https://in.cyberplat.com/cgi-bin/ts/ts_pay_status.cgi";
        //            break;
        //        case 18:
        //            str = "https://in.cyberplat.com/cgi-bin/id/id_pay_status.cgi";
        //            break;
        //        case 19:
        //            str = "https://in.cyberplat.com/cgi-bin/at/at_pay_status.cgi";
        //            break;
        //        case 20:
        //            str = "https://in.cyberplat.com/cgi-bin/ad/ad_pay_status.cgi";
        //            break;
        //        case 21:
        //            str = "https://in.cyberplat.com/cgi-bin/un/un_pay_status.cgi";
        //            break;
        //        case 22:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 23:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 24:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 25:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 26:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 27:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 28:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 29:
        //            str = "https://in.cyberplat.com/cgi-bin/vm/vm_pay_status.cgi";
        //            break;
        //        case 30:
        //            str = "https://in.cyberplat.com/cgi-bin/vm/vm_pay_status.cgi";
        //            break;
        //        case 31:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 32:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 33:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 34:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 35:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 36:
        //            str = "  https://in.cyberplat.com/cgi-bin/lm/lm_pay_status.cgi";
        //            break;
        //        case 37:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 38:
        //            str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";
        //            break;
        //        case 40:
        //            str = "https://in.cyberplat.com/cgi-bin/ad/ad_pay_status.cgi/225";
        //            break;
        //        case 41:
        //            str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_status.cgi";
        //            break;
        //        case 42:
        //            str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_status.cgi";
        //            break;
        //        default:
        //            Console.WriteLine("Invalid Operator");
        //            break;
        //    }
        //    return str;
        //} 
        #endregion
        #endregion

        #region Post Request method
        public static string Process(string url, string message)
        {
            string str = string.Empty;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "POST";
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = (long)bytes.Length;
                Stream requestStream = ((WebRequest)httpWebRequest).GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                WebResponse response = httpWebRequest.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                str = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                response.Close();
            }
            catch (IPrivException ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "IPrivException : " + ex.Message);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Exception : " + ex.Message);
            }
            return str;
        }
        #endregion

        #region Input Message for different request.

        private string GetInputMessage(string number, double amount, string session, int operatorId, string acount)
        {
            //bool _isAccount = false;
            //if (operatorId == 235 || operatorId == 243 || operatorId == 242) //Reliance (1) | Life Insurance(2)
            //_isAccount = true;
            try
            {
                lock (RechargeProcess.LockObject)
                {
                    StringBuilder strRequest = new StringBuilder();
                    strRequest.Append("SD=" + SD + Environment.NewLine);
                    strRequest.Append("AP=" + AP + Environment.NewLine);
                    strRequest.Append("OP=" + OP + Environment.NewLine);
                    //if (operatorId == 35 || operatorId == 1 || operatorId == 111 || operatorId == 41)//Airtel FTT | postpaid mobile \Landline | DTH
                    strRequest.Append("TERM_ID=" + TERM_ID + Environment.NewLine);
                    strRequest.Append("SESSION=" + session + Environment.NewLine);
                    strRequest.Append("NUMBER=" + number + Environment.NewLine);//MobileNo. ||Consumer Number || Policy Number
                    //if (_isAccount)
                    strRequest.Append("ACCOUNT=" + acount + Environment.NewLine);

                    strRequest.Append("AMOUNT=" + (object)amount + Environment.NewLine);
                    strRequest.Append("AMOUNT_ALL=" + (object)amount + Environment.NewLine);
                    strRequest.Append("COMMENT=" + Environment.NewLine);
                    IPriv.Initialize();
                    ///App_Data/CyberPlateKeys/secret.key || 33333333
                    IPrivKey iPrivKey = IPriv.openSecretKey(HttpContext.Current.Server.MapPath(KeyPath), Key);
                    string reqStr = "inputmessage=" + HttpUtility.UrlEncode(iPrivKey.signText((strRequest).ToString()));
                    if (iPrivKey != null)
                        iPrivKey.closeKey();
                    IPriv.Done();
                    return reqStr;
                }
            }
            catch (Exception ex)
            {
                string responseDescription = "Error while generating input message for cyberplat";
                Logger.WriteLog(LogLevelL4N.ERROR, "Exception  : " + responseDescription + "| Ex Message : " + ex.Message);
                return string.Empty;
            }
        }


        private string GetInputMessageForPayment(string mobileNo, double amount, string session, int operatorId, string account)
        {
            //bool _isAccount = false;
            //if (operatorId == 9 || operatorId == 12 || operatorId == 13) //Reliance (1) | Life Insurance(2)
            //_isAccount = true;
            string reqStr = string.Empty;
            try
            {
                lock (RechargeProcess.LockObject)
                {
                    StringBuilder inputMsg = new StringBuilder();
                    inputMsg.Append("SD=" + SD + Environment.NewLine);
                    inputMsg.Append("AP=" + AP + Environment.NewLine);
                    inputMsg.Append("OP=" + OP + Environment.NewLine);
                    inputMsg.Append("SESSION=" + session + Environment.NewLine);
                    inputMsg.Append("NUMBER=" + mobileNo + Environment.NewLine);
                    //for airtel
                    //   if (operatorId == 35 || operatorId == 1 || operatorId == 111 || operatorId == 41)//Airtel FTT | postpaid mobile \Landline | DTH
                    inputMsg.Append("TERM_ID=" + TERM_ID + Environment.NewLine);

                    //if (_isAccount)
                    //inputMsg.Append("ACCOUNT=" + account + Environment.NewLine);
                    inputMsg.Append("AMOUNT=" + amount + Environment.NewLine);
                    inputMsg.Append("AMOUNT_ALL=" + amount + Environment.NewLine);

                    // inputMsg.Append("COMMENT=" + Environment.NewLine);
                    IPriv.Initialize();
                    //"/App_Data/CyberPlateKeys/secret.key" |"3333333333"
                    IPrivKey iPrivKey = IPriv.openSecretKey(HttpContext.Current.Server.MapPath(KeyPath), Key);
                    reqStr = "inputmessage=" + HttpUtility.UrlEncode(iPrivKey.signText((inputMsg).ToString()));
                    if (iPrivKey != null)
                        iPrivKey.closeKey();
                    IPriv.Done();

                }
            }
            catch (Exception ex)
            {
                reqStr = string.Empty;
                Logger.WriteLog(LogLevelL4N.ERROR, "GetInputMessageForPayment | Exception : " + ex.Message);
            }
            return reqStr;
        }

        public string GetInputMessageForStatus(string session, int productCode, string acount)
        {
            string reqStr = string.Empty;
            try
            {
                lock (RechargeProcess.LockObject)
                {
                    StringBuilder inputMsg = new StringBuilder();
                    inputMsg.Append("SESSION=" + session + Environment.NewLine);
                    IPriv.Initialize();
                    //"/App_Data/CyberPlateKeys/secret.key"|"3333333333"
                    IPrivKey iPrivKey = IPriv.openSecretKey(HttpContext.Current.Server.MapPath(KeyPath), Key);//add key and key path
                    reqStr = "inputmessage=" + HttpUtility.UrlEncode(iPrivKey.signText((inputMsg).ToString()));
                    if (iPrivKey != null)
                        iPrivKey.closeKey();
                    IPriv.Done();
                }
            }
            catch (Exception ex)
            {
                reqStr = string.Empty;
                Logger.WriteLog(LogLevelL4N.ERROR, "GetInputMessageForStatus | Exception : " + ex.Message);
            }
            return reqStr;
        }

        #endregion

        #region Error Code for Cyber plate

        public string GetErrorDescription(string response)
        {
            string[] strArray;
            try
            {
                int num = response.LastIndexOf("ERROR=");
                strArray = response.Substring(num + 6, 5).Split(new char[1]
        {
          '\r'
        });
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Exception : " + ex.Message);
                return " Error :";
            }
            string str;
            switch (int.Parse(((object)strArray[0]).ToString()))
            {
                case 1:
                    str = "Session with this number already exists.";
                    break;
                case 2:
                    str = "Invalid Dealer code (SD).";
                    break;
                case 3:
                    str = "Invalid acceptance outlet code (AP).";
                    break;
                case 4:
                    str = "Invalid Operator code (OP).";
                    break;
                case 5:
                    str = "Invalid session code format.";
                    break;
                case 6:
                    str = "Invalid EDS.";
                    break;
                case 7:
                    str = "Invalid amount";
                    break;
                case 8:
                    str = "Invalid phone number format.";
                    break;
                case 9:
                    str = "Invalid format of personal account number.";
                    break;
                case 10:
                    str = "Invalid request message format.";
                    break;
                case 11:
                    str = "Session with such a number does not exist.";
                    break;
                case 12:
                    str = "The request is made from an unregistered IP.";
                    break;
                case 13:
                    str = "The outlet is not registered with the Service Provider.";
                    break;
                case 15:
                    str = "Payments to the benefit of this operator are not supported by the system.";
                    break;
                case 17:
                    str = "The phone number does not match the previously entered number.";
                    break;
                case 18:
                    str = "The payment amount does not match the previously entered amount.";
                    break;
                case 19:
                    str = "The account (contract) number does not match the previously entered number.";
                    break;
                case 20:
                    str = "The payment is being completed.";
                    break;
                case 21:
                    str = "Try after some time";
                    break;
                case 22:
                    str = "The payment has not been accepted. Funds transfer error.";
                    break;
                case 23:
                    str = "Invalid Operator Number";
                    break;
                case 24:
                    str = "Error of connection with the provider’s server or a routine break in CyberPlat®.";
                    break;
                case 25:
                    str = "Effecting of this type of payments is suspended.";
                    break;
                case 26:
                    str = "Payments of this Dealer are temporarily blocked";
                    break;
                case 27:
                    str = "Operations with this account are suspended";
                    break;
                case 30:
                    str = "General system failure.";
                    break;
                case 31:
                    str = "Exceeded number of simultaneously processed requests (CyberPlat®).";
                    break;
                case 32:
                    str = "Try after one hour";
                    break;
                case 33:
                    str = "Exceeded the maximum interval between number verification and payment (24 hours).";
                    break;
                case 34:
                    str = "Transaction with such number could not be found.";
                    break;
                case 35:
                    str = "Payment state alteration error.";
                    break;
                case 36:
                    str = "Invalid payment status.";
                    break;
                case 37:
                    str = "An attempt of referring to the gateway that is different from the gateway at the previous stage.";
                    break;
                case 38:
                    str = "Invalid date. The effective period of the payment may have expired.";
                    break;
                case 39:
                    str = "Invalid account number.";
                    break;
                case 40:
                    str = "The card of the specified value is not registered in the system";
                    break;
                case 41:
                    str = "Error while saving the payment in the system.";
                    break;
                case 42:
                    str = "Error while saving the receipt to the database.";
                    break;
                case 43:
                    str = "Invalid Dealer code (SD).";
                    break;
                case 44:
                    str = "The Client cannot operate on this trading server.";
                    break;
                case 45:
                    str = "No license is available for accepting payments to the benefit of this operator.";
                    break;
                case 46:
                    str = "Could not complete the erroneous payment.";
                    break;
                case 47:
                    str = "Time limitation of access rights has been activated.";
                    break;
                case 48:
                    str = "Error in saving the session data to the database.";
                    break;
                case 50:
                    str = "Effecting payments in the system is temporarily impossible.";
                    break;
                case 51:
                    str = "Data are not found in the system.";
                    break;
                case 52:
                    str = "The Dealer may be blocked. The Dealer’s current status does not allow effecting payments.";
                    break;
                case 53:
                    str = "The Acceptance Outlet may be blocked. The Acceptance Outlet’s current status does not allow effecting payments.";
                    break;
                case 54:
                    str = "The Operator may be blocked. The Operator’s current status does not allow effecting payments.";
                    break;
                case 55:
                    str = "The Dealer Type does not allow effecting payments.";
                    break;
                case 56:
                    str = "An Acceptance Outlet of another type was expected. This Acceptance Outlet type does not allow effecting payments.";
                    break;
                case 57:
                    str = "Another type of Operator was expected. This Operator type does not allow effecting payments.";
                    break;
                case 81:
                    str = "Exceeded the maximum payment amount.";
                    break;
                case 82:
                    str = "Daily debit amount has been exceeded.";
                    break;
                case 223:
                    str = "Not appropriate subscriber contract for top-up";
                    break;
                default:
                    str = "UnKnown Error";
                    break;
            }
            Logger.WriteLog(LogLevelL4N.ERROR, "Error Message  : " + str);
            return str;
        }

        #endregion

        #region Random Numer Generator  for  Cyber Plate Session
        //Generate random number
        private string GenerateRandomSession()
        {
            string t = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJLKMNOPQRTUVWXYZ0123456789";
            char[] chars = t.ToCharArray();
            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                char c = chars[random.Next(chars.Length)];
                sb.Append(c);
            }
            return sb.ToString();
        }
        #endregion

        #region Operators Url

        /// <summary>
        ///  request url.
        /// </summary>
        /// <param name="opId"></param>
        /// <returns>Url</returns>
        private string Verification_OperatorRequestUrl(int opId)
        {
            string str = string.Empty;
            switch (opId)
            {
                #region ****PostPaid -1***************************
                case 10: //225:
                    str = "https://in.cyberplat.com/cgi-bin/ad/ad_pay_check.cgi/225"; // Airtel Landline Postpaid --
                    break;
                case 11: //225:
                    str = "https://in.cyberplat.com/cgi-bin/ad/ad_pay_check.cgi/225"; //Airtel Mobile Pospaid |--
                    break;
                case 12://231:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/231";//Cellone PostPaid --
                    break;
                case 13://232:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/232";//IDEA Postpaid --
                    break;
                case 14://230:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/230";//Loop Mobile PostPaid --
                    break;
                case 15://251:
                    str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay_check.cgi/251";//Reliance Postpaid --
                    break;
                case 16://228:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/228";//Docomo Postpaid --
                    break;
                case 17://233:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/233";//Tata TeleServices PostPaid --
                    break;
                case 18://234:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/234";//Vodafone Postpaid --
                    break;

                #endregion

                #region ****PrePaid - 2 ******************
                case 20://1:
                    str = "https://in.cyberplat.com/cgi-bin/ac/ac_pay_check.cgi/1";//Aircel *--
                    break;
                case 21://15:
                    str = "https://in.cyberplat.com/cgi-bin/at/at_pay_check.cgi/209"; //Airtel FTT PrePaid*//"https://in.cyberplat.com/cgi-bin/at/at_pay_check.cgi";//
                    break;
                case 22://8:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_check.cgi/205";//BSNL Top up *--
                    break;
                case 23://9:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_check.cgi/219";//BSNL (Validity / Special)--***************
                    break;
                case 24://17:
                    str = "https://in.cyberplat.com/cgi-bin/id/id_pay_check.cgi";//Idea  Prepaid *-
                    break;
                case 25://2:
                    str = "https://in.cyberplat.com/cgi-bin/lm/lm_pay_check.cgi";//Loop Mobile -*
                    break;
                case 26://212:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_check.cgi/212";//MTNL Topup*
                    break;
                case 27://215:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_check.cgi/215";//MTNL Validity-------------------------
                    break;
                case 28://3:
                    str = "https://in.cyberplat.com/cgi-bin/mt/mt_pay_check.cgi";//MTS *--
                    break;
                case 29://4:
                    str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay_check.cgi";//Reliance CDMA* --
                    break;
                case 200://4:
                    str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay_check.cgi";//Reliance GSM*--
                    break;
                case 201://19:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_check.cgi";//T24 (Flexi)--------------
                    break;
                case 202://20:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_check.cgi";//T24 (special)-------------------
                    break;
                case 203://6:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_check.cgi";//TATA Docomo flexi *--
                    break;
                case 204://7:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_check.cgi";//TATA Docomo special ----------
                    break;
                case 205://5:
                    str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay_check.cgi";//Tata Indicom *--
                    break;

                case 206://18:
                    str = "https://in.cyberplat.com/cgi-bin/un/un_pay_check.cgi"; //Uninor *-----
                    break;
                case 207://24:
                    str = "https://in.cyberplat.com/cgi-bin/vm/vm_pay_check.cgi";//Videocon Mobile (TopUp /Special)*
                    break;
                case 208://21:
                    str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay_check.cgi";//Virgin CDMA*
                    break;
                case 209://22:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_check.cgi";//Virgin GSM Flexi*
                    break;
                case 210://23:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_check.cgi";//Virgin GSM Special----------------
                    break;
                case 211://10:
                    str = "https://in.cyberplat.com/cgi-bin/vd/vd_pay_check.cgi";//Vodafone *-
                    break;
                #endregion

                #region ****DTH**************************

                case 30://16:
                    str = "https://in.cyberplat.com/cgi-bin/ad/ad_pay_check.cgi";//Airtel DTH *
                    break;
                case 31://11:
                    str = "https://in.cyberplat.com/cgi-bin/bt/bt_pay_check.cgi";//BIG TV *--
                    break;
                case 32://12:
                    str = "https://in.cyberplat.com/cgi-bin/dt/dt_pay_check.cgi"; //DISH TV --*
                    break;
                case 33://213:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_check.cgi/213";//SUN TV*--
                    break;
                case 34://14:
                    str = "https://in.cyberplat.com/cgi-bin/ts/ts_pay_check.cgi";//"https://in.cyberplat.com/cgi-bin/ts/ts_pay_check.cgi?t=249";////TATA SKY *-- (B2c data)
                    break;
                case 35://13:
                    str = "https://in.cyberplat.com/cgi-bin/vc/vc_pay_check.cgi"; //VideoconD2h *--
                    break;

                #endregion

                #region ****Electricity*************

                case 41://235:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/235";//Reliance Energy (Mumbai) -
                    break;
                #endregion

                #region *****Gas Bill******************

                case 50://241:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/241";//Mahanagar Gas Limited - 
                    break;

                #endregion

                #region ****Insurance -6**********************
                /*Policy Number -> NUMBER (10 symbols)
                  Date of Birth - ACCOUNT (DD-MM-YYYY) */
                case 60://243:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/243";//ICICI Pru Life --
                    break;
                case 61://242:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/242";//Tata AIG Life -- 
                    break;

                #endregion

                #region ****BroadBand - 7 ***********************

                case 70://261:
                    str = "https://in.cyberplat.com/cgi-bin/tk/tk_pay_check.cgi/261";//Tikona Postpaid | broadband --
                    break;

                #endregion

                #region ****Data Card - 8 ****************************
                // Missing : Vodafone \ IDEA
                case 80://42:
                    str = "https://in.cyberplat.com/cgi-bin/ac/ac_pay_check.cgi/1";//Aircel*--
                    break;
                case 81://40:
                    str = "https://in.cyberplat.com/cgi-bin/at/at_pay_check.cgi"; //Airtel DataCard--
                    break;
                case 82://205:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_check.cgi/205";//BSNL *--
                    break;
                case 83://41:
                    str = "https://in.cyberplat.com/cgi-bin/id/id_pay_check.cgi";//Idea DataCard--
                    break;
                case 84://43:
                    str = "https://in.cyberplat.com/cgi-bin/mt/mt_pay_check.cgi";//MTS--
                    break;
                case 85://44:
                    str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay_check.cgi";//Reliance-- 
                    break;
                case 86://46:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_check.cgi";//TATA Docomo*--
                    break;
                case 87://45:
                    str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay_check.cgi";//Tata Indicom*--
                    break;

                #endregion

                #region ******others -Check once************
                //Landline |5
                case 1000://240:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_check.cgi/240";//MTNL Delhi - 
                    break;
                #endregion

                default:
                    str = string.Empty;
                    break;
            }
            return str;
        }

        private string Payment_OperatorRequestUrl(int opId)
        {
            string str = string.Empty;
            switch (opId)
            {
                #region ****PostPaid**********
                case 10:
                    str = "https://in.cyberplat.com/cgi-bin/ad/ad_pay.cgi/225"; //Airtel Landline 
                    break;
                case 11:
                    str = "https://in.cyberplat.com/cgi-bin/ad/ad_pay.cgi/225"; //Airtel Mobile Pospaid 
                    break;
                case 12://231:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/231";//Cellone PostPaid 
                    break;
                case 13://232:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/232";//IDEA Postpaid
                    break;
                case 14://230:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/230";//Loop Mobile PostPaid 
                    break;
                case 15://251:
                    str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay.cgi/251";//Reliance Postpaid * 
                    break;
                case 16://228:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/228";//Docomo Postpaid
                    break;
                case 17://233:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/233";//Tata TeleServices PostPaid 
                    break;
                case 18://234:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/234";//Vodafone Postpaid 
                    break;
                #endregion

                #region ****Prepaid**************
                case 20://1:
                    str = "https://in.cyberplat.com/cgi-bin/ac/ac_pay.cgi/1";//Aircel
                    break;
                case 21://15:
                    str = "https://in.cyberplat.com/cgi-bin/at/at_pay.cgi/209"; //Airtel FTT//"https://in.cyberplat.com/cgi-bin/at/at_pay.cgi";//
                    break;
                case 22://8:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay.cgi/205";//BSNL Top up
                    break;
                case 23://9:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay.cgi/219";//BSNL (Validity / Special)
                    break;
                case 24://17:
                    str = "https://in.cyberplat.com/cgi-bin/id/id_pay.cgi";//Idea  Prepaid
                    break;
                case 25://2:
                    str = "https://in.cyberplat.com/cgi-bin/lm/lm_pay.cgi";//Loop Mobile 
                    break;
                case 26://212:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay.cgi/212";//MTNL Topup
                    break;
                case 27://215:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay.cgi/215";//MTNL Validity
                    break;
                case 28://3:
                    str = "https://in.cyberplat.com/cgi-bin/lm/lm_pay.cgi";//MTS
                    break;
                case 29://4:
                    str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay.cgi";//Reliance CDMA 
                    break;
                case 200://4:
                    str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay.cgi";//Reliance GSM
                    break;
                case 201://19:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay.cgi";//T24 (Flexi)
                    break;
                case 202://20:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay.cgi";//T24 (special)
                    break;
                case 203://6:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay.cgi";//TATA Docomo flexi 
                    break;

                case 204://7:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay.cgi";//TATA Docomo special
                    break;
                case 205://5:
                    str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay.cgi";//Tata Indicom 
                    break;
                case 206://18:
                    str = "https://in.cyberplat.com/cgi-bin/un/un_pay.cgi";//"https://in.cyberplat.com/cgi-bin/un/un_pay_check.cgi"; //Uninor
                    break;
                case 207://24:
                    str = "https://in.cyberplat.com/cgi-bin/vm/vm_pay.cgi";//Videocon Mobile (TopUp /Special)
                    break;
                case 208://21:
                    str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay.cgi";//Virgin CDMA
                    break;
                case 209://22:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay.cgi";//Virgin GSM Flexi
                    break;
                case 210://23:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay.cgi";//Virgin GSM Special
                    break;
                case 211://10:
                    str = "https://in.cyberplat.com/cgi-bin/vd/vd_pay.cgi";//Vodafone
                    break;

                #endregion

                #region ****DTH***************************
                case 30://16:
                    str = "https://in.cyberplat.com/cgi-bin/ad/ad_pay.cgi";//Airtel DTH 
                    break;
                case 31://11:
                    str = "https://in.cyberplat.com/cgi-bin/bt/bt_pay.cgi";//BIG TV
                    break;
                case 32://12:
                    str = "https://in.cyberplat.com/cgi-bin/dt/dt_pay.cgi"; //DISH TV
                    break;
                case 33://213:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay.cgi/213";//SUN TV
                    break;
                case 34://14:
                    str = "https://in.cyberplat.com/cgi-bin/ts/ts_pay.cgi";//"https://in.cyberplat.com/cgi-bin/ts/ts_pay.cgi?t=249";////TATA SKY 
                    break;
                case 35://13:
                    str = "https://in.cyberplat.com/cgi-bin/vc/vc_pay.cgi"; //VideoconD2h
                    break;

                #endregion

                #region ****Electricity - 4 *****************

                case 41://235:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/235";//Reliance Energy (Mumbai) 
                    break;
                #endregion

                #region ****Gas Bill - 5 ***********************

                case 50://241:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/241";//Mahanagar Gas Limited 
                    break;
                #endregion

                #region ****Insurance - 6 *****************************

                case 60://243:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/243";//ICICI Pru Life 
                    break;
                case 61://242:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/242";//Tata AIG Life 
                    break;
                #endregion

                #region ****Broadband - 7 **************************
                case 70://261:
                    str = "https://in.cyberplat.com/cgi-bin/tk/tk_pay.cgi/261";//Tikona Postpaid | broadband
                    break;
                #endregion

                #region ****DataCard -  8 *************************************
                case 80://42:
                    str = "https://in.cyberplat.com/cgi-bin/ac/ac_pay.cgi/1";//Aircel
                    break;
                case 81://40:
                    str = "https://in.cyberplat.com/cgi-bin/at/at_pay.cgi"; //Airtel DataCard
                    break;
                case 82://205:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay.cgi/205";//BSNL 
                    break;
                case 83://41:
                    str = "https://in.cyberplat.com/cgi-bin/id/id_pay.cgi";//Idea DataCard
                    break;
                case 84://43:
                    str = "https://in.cyberplat.com/cgi-bin/mt/mt_pay.cgi";//MTS
                    break;
                case 85://44:
                    str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay.cgi";//Reliance 
                    break;
                case 86://46:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay.cgi";//TATA Docomo
                    break;
                case 87://45:
                    str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay.cgi";//Tata Indicom
                    break;

                #endregion

                #region ****Others *****************************
                case 1000://240:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay.cgi/240";//MTNL Delhi
                    break;
                #endregion
                default:
                    str = string.Empty;
                    break;
            }
            return str;
        }

        /// <summary>
        /// Get the Url for status check
        /// </summary>
        /// <param name="opId">id for Operator</param>
        /// <returns>Status check Urk</returns>
        public string Status_OperatorRequestUrl(int opId)
        {
            string str = string.Empty;
            switch (opId)
            {
                #region ****PostPaid -1 ******************************************
                case 10:
                    str = "https://in.cyberplat.com/cgi-bin/ad/ad_pay_status.cgi/225"; //Airtel Landline Pospaid 
                    break;
                case 11:
                    str = "https://in.cyberplat.com/cgi-bin/ad/ad_pay_status.cgi/225"; // Airtel Mobile Postpaid
                    break;
                case 12:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";//Cellone PostPaid 
                    break;
                case 13:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";//IDEA Postpaid -
                    break;
                case 14:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";//Loop Mobile PostPaid 
                    break;
                case 15:
                    str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay_status.cgi";//Reliance Postpaid * 
                    break;
                case 16:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";//Docomo Postpaid
                    break;
                case 17:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";//Tata TeleServices PostPaid 
                    break;
                case 18:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";//Vodafone Postpaid 
                    break;

                #endregion

                #region ****Prepaid - 2 *************************
                case 20:
                    str = "https://in.cyberplat.com/cgi-bin/ac/ac_pay_status.cgi";//Aircel
                    break;
                case 21:
                    str = "https://in.cyberplat.com/cgi-bin/at/at_pay_status.cgi";//Airtel FTT 
                    break;
                case 22:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_status.cgi";//BSNL Top up
                    break;
                case 23:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_status.cgi";//BSNL (Validity / Special)
                    break;
                case 24:
                    str = "https://in.cyberplat.com/cgi-bin/id/id_pay_status.cgi";//Idea  Prepaid
                    break;
                case 25:
                    str = "https://in.cyberplat.com/cgi-bin/lm/lm_pay_status.cgi";//Loop Mobile 
                    break;
                case 26:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_status.cgi";//MTNL Topup
                    break;
                case 27:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_status.cgi";//MTNL Validity
                    break;
                case 28:
                    str = "https://in.cyberplat.com/cgi-bin/mt/mt_pay_status.cgi";//MTS
                    break;
                case 29:
                    str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay_status.cgi";//Reliance CDMA 
                    break;
                case 200:
                    str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay_status.cgi";//Reliance GSM
                    break;
                case 201:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_status.cgi";//T24 (Flexi)
                    break;
                case 202:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_status.cgi";//T24 (special)
                    break;
                case 203:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_status.cgi";//TATA Docomo flexi 
                    break;
                case 204:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_status.cgi";//TATA Docomo special
                    break;
                case 205:
                    str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay_status.cgi";//Tata Indicom 
                    break;
                case 206:
                    str = "https://in.cyberplat.com/cgi-bin/un/un_pay_status.cgi"; //Uninor
                    break;
                case 207:
                    str = "https://in.cyberplat.com/cgi-bin/vm/vm_pay_status.cgi";//Videocon Mobile (TopUp /Special)
                    break;
                case 208:
                    str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay_status.cgi";//Virgin CDMA
                    break;
                case 209:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_status.cgi";//Virgin GSM Flexi
                    break;
                case 210:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_status.cgi";//Virgin GSM Special
                    break;
                case 211:
                    str = "https://in.cyberplat.com/cgi-bin/vd/vd_pay_status.cgi";//Vodafone
                    break;

                #endregion

                #region ****DTH - 3 ********************************************
                case 30:
                    str = "https://in.cyberplat.com/cgi-bin/at/at_pay_status.cgi"; //Airtel FTT
                    break;
                case 31:
                    str = "https://in.cyberplat.com/cgi-bin/bt/bt_pay_status.cgi";//BIG TV
                    break;
                case 32:
                    str = "https://in.cyberplat.com/cgi-bin/dt/dt_pay_status.cgi"; //DISH TV
                    break;
                case 33:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_status.cgi";//SUN TV
                    break;
                case 34:
                    str = "https://in.cyberplat.com/cgi-bin/ts/ts_pay_status.cgi";//"https://in.cyberplat.com/cgi-bin/ts/ts_pay_status.cgi";//TATA SKY 
                    break;
                case 35:
                    str = "https://in.cyberplat.com/cgi-bin/vc/vc_pay_status.cgi"; //VideoconD2h
                    break;

                #endregion

                #region **********Electricity - 4 ********************

                case 41:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";//Reliance Energy (Mumbai) 
                    break;
                #endregion

                #region ****Gas Pay - 5 ****************************

                case 50:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";//Mahanagar Gas Limited 
                    break;

                #endregion

                #region ****Insurance - 6 *****************
                case 60:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";//ICICI Pru Life 
                    break;
                case 61:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";//Tata AIG Life 
                    break;
                #endregion

                #region ****BroadBand - 7 ********************************
                case 70:
                    str = "https://in.cyberplat.com/cgi-bin/tk/tk_pay_status.cgi"; //Tikona Postpaid | broadband
                    break;

                #endregion

                #region ****Data Card - 8 **************************************

                case 80:
                    str = "https://in.cyberplat.com/cgi-bin/ac/ac_pay_status.cgi";//Aircel
                    break;
                case 81:
                    str = "https://in.cyberplat.com/cgi-bin/at/at_pay_status.cgi"; //Airtel DataCard
                    break;
                case 82:
                    str = "https://in.cyberplat.com/cgi-bin/mm/mm_pay_status.cgi";//BSNL 
                    break;
                case 83:
                    str = "https://in.cyberplat.com/cgi-bin/id/id_pay_status.cgi";//Idea DataCard
                    break;
                case 84:
                    str = "https://in.cyberplat.com/cgi-bin/mt/mt_pay_status.cgi";//MTS
                    break;
                case 85:
                    str = "https://in.cyberplat.com/cgi-bin/rl/rl_pay_status.cgi";//Reliance 
                    break;
                case 86:
                    str = "https://in.cyberplat.com/cgi-bin/dc/dc_pay_status.cgi";//TATA Docomo
                    break;
                case 87:
                    str = "https://in.cyberplat.com/cgi-bin/tt/tt_pay_status.cgi";//Tata Indicom
                    break;

                #endregion

                #region ****Others**************************8

                case 1000:
                    str = "https://in.cyberplat.com/cgi-bin/bu/bu_pay_status.cgi";//MTNL Delhi
                    break;
                #endregion

                default:
                    str = string.Empty;
                    break;
            }
            return str;
        }

        #endregion
    }
}
