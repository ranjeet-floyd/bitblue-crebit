using api.dhs.Logging;
using api.dhs.Models.DL.Common;
using com.dhs.webapi.Model.DL.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace api.dhs.Models.BL.Common
{
    public class BL_Admin
    {
        private string SpName { get; set; }
        [JsonIgnore]
        public bool _IsSuccess { get; set; }
        DataSet ds = null;
        DataBase db = new DataBase();
        public List<DL_PopUpMessage> popUpMessage;
        public List<DL_AdminUpdate> adminUpdate;
        public List<DL_MonthlyRetailerCharges> monthlyCharges;

        //Update message
        public List<DL_AdminUpdate> GetAdminUpdate()
        {
            Logger.WriteLog(LogLevelL4N.INFO, "Inside AdminUpdate Method");
            _IsSuccess = true;
            try
            {
                this.SpName = DL_StoreProcedure.SP_DHS_API_AdminUpdate; //Sp Name
                Logger.WriteLog(LogLevelL4N.INFO, "SpName :" + this.SpName);
                SqlParameter[] param = null;

                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    Logger.WriteLog(LogLevelL4N.INFO, "table count : " + ds.Tables.Count.ToString());
                    //ds.Tables
                    var Json = JsonConvert.SerializeObject(ds.Tables[0]);
                    Logger.WriteLog(LogLevelL4N.INFO, "Json Data : " + Json);
                    //operatorReturn = new List<DL_OperatorReturn>();
                    adminUpdate = JsonConvert.DeserializeObject<List<DL_AdminUpdate>>(Json);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Exception : " + ex.Message);
                _IsSuccess = false;
            }

            return adminUpdate;
        }

        //popup message
        public List<DL_PopUpMessage> GetPopUpMessage()
        {
            Logger.WriteLog(LogLevelL4N.INFO, "Inside PopUpMessage Method");
            _IsSuccess = true;
            try
            {
                this.SpName = DL_StoreProcedure.SP_DHS_API_AdminPopUp; //Sp Name
                Logger.WriteLog(LogLevelL4N.INFO, "SpName :" + this.SpName);
                SqlParameter[] param = null;

                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    Logger.WriteLog(LogLevelL4N.INFO, "table count : " + ds.Tables.Count.ToString());
                    //ds.Tables
                    var Json = JsonConvert.SerializeObject(ds.Tables[0]);
                    Logger.WriteLog(LogLevelL4N.INFO, "Json Data : " + Json);
                    popUpMessage = JsonConvert.DeserializeObject<List<DL_PopUpMessage>>(Json);

                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Exception : " + ex.Message);
                _IsSuccess = false;
            }

            return popUpMessage;
        }

        //crebit montly charges
        public List<DL_MonthlyRetailerCharges> UpdateCrebitMonthlyCharges()
        {
            _IsSuccess = false;
            string msg = " User Rs. 30 has been debit for monthly service charge. Crebit Customer Experience Team.";
            this.SpName = DL_StoreProcedure.SP_DHS_API_DHS_CREBIT_MONTHLYCHARGES; //Sp Name
            SqlParameter[] param = null;
            try
            {
                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    var Json = JsonConvert.SerializeObject(ds.Tables[0]);
                    monthlyCharges = JsonConvert.DeserializeObject<List<DL_MonthlyRetailerCharges>>(Json);
                    _IsSuccess = true;
                    //send sms to each retailer
                    Task t = new Task(() =>
                    {
                        foreach (var item in monthlyCharges)
                        {
                            BL_SMS.SendSMS(item.UserId.Trim(), "Dear " + item.UserId.Trim() + msg);
                        }
                    });
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "@ UpdateCrebitMonthlyCharges Exception : " + ex.Message);
                _IsSuccess = false;
            }
            return monthlyCharges;
        }



    }
}