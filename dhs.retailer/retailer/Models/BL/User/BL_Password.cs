using api.dhs.Logging;
using api.dhs.Models.BL.Common;
using com.dhs.webapi.Model.Common;
using com.dhs.webapi.Model.DL.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication1.Models.DL.User;

namespace WebApplication1.Models.BL.User
{
    public class BL_Password
    {
        [JsonIgnore]
        public bool _IsSuccess { get; set; }
        private string SpName { get; set; }
        public DL_ChangePasswordReturn ChangePassReturn = null;
        public DL_ForgotPasswordReturn forgotPassReturn = null;
        DataBase db = new DataBase();
        DataSet ds = null;


        //Check ChangePassword
        public DL_ChangePasswordReturn ChangePassword(DL_ChangePassword changePass)
        {
            ChangePassReturn = new DL_ChangePasswordReturn();
            this.SpName = DL_StoreProcedure.SP_DHS_API_ChangePassword; //Sp Name
            _IsSuccess = true;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@UserId", changePass.UserId);
                param[1] = new SqlParameter("@OldPassword", changePass.OPass);
                param[2] = new SqlParameter("@NewPassword", changePass.NPass);
                param[3] = new SqlParameter("@Key", changePass.Key);
                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    ChangePassReturn.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                    string mobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                    if (ChangePassReturn.Status == "1")
                    {
                        string message = "Dear User, your Password has been changed.Your password is" + changePass.NPass + ". Crebit Customer Experience Team.";
                        //Non Blocking code
                        Task t = new Task(() => BL_SMS.SendSMS(mobile, message));
                        t.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "ChangePassword | Exception : " + ex.Message);
                _IsSuccess = false;
            }
            return ChangePassReturn;
        }

        //Check ForgotPassword
        public DL_ForgotPasswordReturn ForgotPassword(DL_ForgotPassword forgotPass)
        {
            forgotPassReturn = new DL_ForgotPasswordReturn();
            this.SpName = DL_StoreProcedure.SP_DHS_API_ForgotPassword; //Sp Name
            _IsSuccess = true;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Mobile", forgotPass.Mobile);
                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    forgotPassReturn.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                    string password = ds.Tables[0].Rows[0]["Password"].ToString();
                    if (forgotPassReturn.Status == "2")//success
                    {
                        //Dear User, Your password is ##Field## . Crebit Customer Experience Team.
                        string message = "Dear User, Your password is " + password + " . Crebit Customer Experience Team.";
                        //string message = "Dear User, Your password is " + password + ". Crebit Customer Experience Team.";
                        BL_SMS.SendSMS(forgotPass.Mobile, message);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "ForgotPassword | Exception : " + ex.Message);
                _IsSuccess = false;
            }
            return forgotPassReturn;
        }

    }
}