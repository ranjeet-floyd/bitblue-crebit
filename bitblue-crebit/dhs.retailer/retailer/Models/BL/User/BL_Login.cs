using System;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using com.dhs.webapi.Model.DL.Common;
using com.dhs.webapi.Model.Common;
using System.Collections.Generic;
using WebApplication1.Models.DL.User;
using api.dhs.Logging;
using System.Text;
namespace com.dhs.webapi.Model.BL_User
{
    public class BL_Login
    {
        [JsonIgnore]
        public bool _IsSuccess { get; set; }
        private string SpName { get; set; }
        public List<DL_LoginReturn> loginReturn = null;
        public List<DL_BankDetailsReturn> bankAccounts = null;
        DataBase db = new DataBase();
        DataSet ds = null;


        //Check login
        public List<DL_LoginReturn> CheckLogin(DL_Login login)
        {
            this.SpName = DL_StoreProcedure.SP_DHS_API_Login; //Sp Name
            _IsSuccess = true;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@UserName", login.Mobile);
                param[1] = new SqlParameter("@Password", login.Pass);
                param[2] = new SqlParameter("@Version", login.Version);
                param[3] = new SqlParameter("@Key",GenerateRandomSession());
                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    Logger.WriteLog(LogLevelL4N.INFO, "Got Data from Db.");
                    loginReturn = new List<DL_LoginReturn>();
                    //  var Json = JsonConvert.SerializeObject(ds);
                    //loginReturn = JsonConvert.DeserializeObject<DL_LoginReturn>(Json);
                    loginReturn = SerializeData.SerializeMultiValue<DL_LoginReturn>(ds.Tables[0]);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Exeception : "+ex.Message);
                _IsSuccess = false;
            }
            return loginReturn;
        }

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

        //Get the Registered account for the user.
        public List<DL_BankDetailsReturn> GetBankDetails(User user)
        {
            this.SpName = DL_StoreProcedure.SP_DHS_API_BankAccounts; //Sp Name
            _IsSuccess = true;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@UserId", user.UserId);
                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    bankAccounts = new List<DL_BankDetailsReturn>();
                    bankAccounts = SerializeData.SerializeMultiValue<DL_BankDetailsReturn>(ds.Tables[0]);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, "Exeception : " + ex.Message);
                _IsSuccess = false;
            }
            return bankAccounts;
        }

    }
}
