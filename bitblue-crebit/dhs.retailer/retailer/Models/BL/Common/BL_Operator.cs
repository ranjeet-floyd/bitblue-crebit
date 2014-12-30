using api.dhs.Logging;
using com.dhs.webapi.Model.Common.CyberPlate;
using com.dhs.webapi.Model.DL.Common;
using com.dhs.webapi.Models.DL.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace WebApplication1.Models.BL.Common
{
    public class BL_Operator
    {
        private string SpName { get; set; }
        [JsonIgnore]
        public bool _IsSuccess { get; set; }
        //Added | Ranjeet

        DataSet ds = null;
        DataBase db = new DataBase();
        public List<DL_OperatorReturn> operatorReturn;
        public List<DL_OperatorMarginReturn> operatorMarginReturn;
        public DL_SessionCyberPlateStatusReturn dL_SessionCyberPlateStatusReturn = null;

        //operator Details
        public List<DL_OperatorReturn> GetOperators()
        {
            Logger.WriteLog(LogLevelL4N.INFO, "Inside GetOperators Method");
            _IsSuccess = true;
            try
            {
                this.SpName = DL_StoreProcedure.SP_GetOperaters; //Sp Name
                Logger.WriteLog(LogLevelL4N.INFO, "SpName :" + this.SpName);
                SqlParameter[] param = null;

                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    //Logger.WriteLog(LogLevelL4N.INFO, "table count : " + ds.Tables.Count.ToString());
                    //ds.Tables
                    var Json = JsonConvert.SerializeObject(ds.Tables[0]);
                    //Logger.WriteLog(LogLevelL4N.INFO, "Json Data : " + Json);
                    //operatorReturn = new List<DL_OperatorReturn>();
                    operatorReturn = JsonConvert.DeserializeObject<List<DL_OperatorReturn>>(Json);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Exception : " + ex.Message);
                _IsSuccess = false;
            }

            return operatorReturn;
        }

        public List<DL_OperatorMarginReturn> GetOperatorMargin()
        {
            Logger.WriteLog(LogLevelL4N.INFO, "Inside GetOperatorMargin Method");
            _IsSuccess = true;
            try
            {
                this.SpName = DL_StoreProcedure.SP_GetOperaterMargin; //Sp Name
                Logger.WriteLog(LogLevelL4N.INFO, "SpName :" + this.SpName);
                SqlParameter[] param = null;

                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    Logger.WriteLog(LogLevelL4N.INFO, "table count : " + ds.Tables.Count.ToString());
                    //ds.Tables
                    var Json = JsonConvert.SerializeObject(ds.Tables[0]);
                    //Logger.WriteLog(LogLevelL4N.INFO, "Json Data : " + Json);
                    operatorMarginReturn = JsonConvert.DeserializeObject<List<DL_OperatorMarginReturn>>(Json);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Exception : " + ex.Message);
                _IsSuccess = false;
            }

            return operatorMarginReturn;
        }

        public DL_SessionCyberPlateStatusReturn GetCyberPlateStatus(DL_SessionCyberPlateStatus dL_SessionCyberPlateStatus)
        {

            _IsSuccess = true;
            dL_SessionCyberPlateStatusReturn = new DL_SessionCyberPlateStatusReturn();
            ProcessReqReturn prt = new ProcessReqReturn();
            string TransationId = string.Empty;
            try
            {
                RechargeProcess rechargeProcess = new RechargeProcess();
                CyberPlateStatus cyberPlateStatus = GetSessionId(dL_SessionCyberPlateStatus.TransactionId); //Added | Ranjeet |26-Dec |Get Session and Opid
                if (cyberPlateStatus != null)
                {
                    string strinputMsgStatus = rechargeProcess.GetInputMessageForStatus(cyberPlateStatus.SessionId,
                        cyberPlateStatus.OperatorId, string.Empty);

                    string statusResponse = RechargeProcess.Process(rechargeProcess.Status_OperatorRequestUrl(cyberPlateStatus.OperatorId), strinputMsgStatus);

                    int num = statusResponse.LastIndexOf("TRANSID=");
                    TransationId = statusResponse.Substring(num).Split(new char[1] { '\r' })[0].Split('=')[1];
                    if (statusResponse.IndexOf("ERROR=0") != -1 && statusResponse.IndexOf("RESULT=7") != -1)
                    {
                        dL_SessionCyberPlateStatusReturn.TransId = TransationId;
                        dL_SessionCyberPlateStatusReturn.Status = "1";
                        dL_SessionCyberPlateStatusReturn.Message = "Successfully recharged.";
                        return dL_SessionCyberPlateStatusReturn;
                    }
                    else
                    {
                        dL_SessionCyberPlateStatusReturn.TransId = TransationId;
                        dL_SessionCyberPlateStatusReturn.Status = "2";
                        dL_SessionCyberPlateStatusReturn.Message = rechargeProcess.GetErrorDescription(statusResponse);
                        return dL_SessionCyberPlateStatusReturn;
                    }

                }

                else
                    return null;
            }


            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Exception : " + ex.Message);
                _IsSuccess = false;
            }

            return dL_SessionCyberPlateStatusReturn;
        }

        public MSEBUserDetailsReqReturn GetMSEBUserStatus(MSEBUserDetailsReq mSEBUserDetailsReq)
        {
            MSEBUserDetailsReqReturn mSEBUserDetailsReqReturn = null;
            MSEBapiReq _msebapiReq = new MSEBapiReq { BuNumber = mSEBUserDetailsReq.BuCode, ConsumerNo = mSEBUserDetailsReq.ConsumerNo };
            try
            {

                string json = this.PostWebApi(_msebapiReq);
                if (!json.Contains("error"))
                    mSEBUserDetailsReqReturn = JsonConvert.DeserializeObject<MSEBUserDetailsReqReturn>(json);

                this._IsSuccess = true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "GetMSEBUserStatus Exception : " + ex.Message);
                this._IsSuccess = false;
            }
            return mSEBUserDetailsReqReturn;
        }

        public DeleteRegAccountReturn DeleteRegAccount(DeleteRegAccount deleteRegAccount)
        {
            DeleteRegAccountReturn deleteRegAccountReturn = new DeleteRegAccountReturn();
            try
            {
                this._IsSuccess = true;
                this.SpName = DL_StoreProcedure.SP_DHS_DeleteRegAccount; //Sp Name
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@UserId", deleteRegAccount.UserId);
                param[1] = new SqlParameter("@TblId", deleteRegAccount.Id);

                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    //ds.Tables
                    //var Json = JsonConvert.SerializeObject(ds.Tables[0].Rows[0]);
                    deleteRegAccountReturn = new DeleteRegAccountReturn() { Status = ds.Tables[0].Rows[0]["Status"].ToString() };
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Exception : " + ex.Message);
                _IsSuccess = false;
            }
            return deleteRegAccountReturn;

        }

        //POST APi
        private string PostWebApi(MSEBapiReq data)
        {
            string jsonMessage = string.Empty;
            string MSEBapiUrl = "http://wss.mahadiscom.in/wss/wss?uiActionName=postViewPayBill&IsAjax=true";
            HttpClient client = new HttpClient();
            var body = String.Format("BuNumber=" + data.BuNumber + "&ConsumerNo=" + data.ConsumerNo);
            StringContent theContent = new StringContent(body, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            try
            {
                HttpResponseMessage aResponse = client.PostAsync(new Uri(MSEBapiUrl), theContent).Result;

                using (Stream responseStream = aResponse.Content.ReadAsStreamAsync().Result)
                {
                    jsonMessage = new StreamReader(responseStream).ReadToEnd();
                }
            }
            catch (Exception ex)
            { Logger.WriteLog(LogLevelL4N.ERROR, "Exception : " + ex.Message); }
            return jsonMessage;
        }

        //Modified: Ranjeet || 26-Dec || return type CyberPlateStatus
        public CyberPlateStatus GetSessionId(string transId)
        {
            CyberPlateStatus cyberPlateStatus = null;
            //string sessionNum = string.Empty;
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@TransId", transId);
            string spName = DL_StoreProcedure.SP_DHS_GetSessionId; //Sp Name"GetSessionId";
            ds = db.GetDataSet(spName, param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                //Modified: Ranjeet || Get cyber session id
                //sessionNum = "" + dr["CSessionId"];
                cyberPlateStatus = new CyberPlateStatus() { SessionId = "" + dr["CSessionId"], OperatorId = Convert.ToInt32(dr["OpId"]) };
            }
            return cyberPlateStatus;
        }
    }
}