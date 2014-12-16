using api.dhs.Logging;
using api.dhs.Models.BL.Common;
using com.dhs.webapi.Model.Common;
using com.dhs.webapi.Model.DL.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApplication1.Models.DL.User;

namespace com.dhs.webapi.Model.BL.User
{
    public class BL_SignUp
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        [JsonIgnore]
        public bool _IsSuccess { get; set; }
        private string SpName { get; set; }
        public List<DL_SignUpReturn> signUpReturn = null;
        DataBase db = new DataBase();
        DataSet ds = null;


        //Check login
        public List<DL_SignUpReturn> SignUp(DL_SignUp signUp)
        {
            this.SpName = DL_StoreProcedure.SP_DHS_API_SignUp; //Sp Name
            _IsSuccess = true;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@Name", signUp.Name);
                param[1] = new SqlParameter("@Password", signUp.Pass);
                param[2] = new SqlParameter("@Mobile", signUp.Mobile);
                param[3] = new SqlParameter("@UserType", signUp.UserType);
                param[4] = new SqlParameter("@Date", indianTime);

                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    signUpReturn = SerializeData.SerializeMultiValue<DL_SignUpReturn>(ds.Tables[0]);
                    //send message
                    foreach (var t in signUpReturn)
                    {
                        if (t.Status == "1")//suceess
                        {
                            Task task = new Task(() =>
                            {
                                string message = "Dear " + signUp.Name + " , your UserName is " + signUp.Mobile + " and password is " + signUp.Pass + " . Crebit Customer Experience Team.";
                                BL_SMS.SendSMS(signUp.Mobile, message);
                            });

                            task.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _IsSuccess = false;
                Logger.WriteLog(LogLevelL4N.ERROR, "SignUp | Exception : " + ex.Message);
            }
            return signUpReturn;
        }
    }
}