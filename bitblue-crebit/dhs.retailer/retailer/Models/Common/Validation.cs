using System;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using com.dhs.webapi.Model.DL.Common;
namespace com.dhs.webapi.Model.Common
{

    public class Api_Trans
    {
        public string UserId { get; set; }
        public string UserType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class Api_Balance : Api_Trans
    {
    }

    public class Api_Profit : Api_Trans
    {
    }
    public class Api_MobileReg 
    {
        public string UserId { get; set; }
        public string UserType { get; set; }
        public string Mobile { get; set; }
        public string  Name { get; set; }
    }
    public class Api_transferFund
    {
        public string UserId { get; set; }
        public string UserType { get; set; }
        public string MobileTo { get; set; }
        public string Amount { get; set; }
    }
    public class Api_forgotPass
    {
        public string Mobile { get; set; }
    }

    public class Api_signUp
    {
        public string Name { get; set; }
        public string Pass { get; set; }
        public string Mobile { get; set; }
        public string UserType { get; set; }
    }

    public class Api_changePass
    {
        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string UserType { get; set; }
    }

    public class Api_Services
    {
        public string UserId { get; set; }
        public string UserType { get; set; }
        public string TransactionType { get; set; }
        public string OperatorId { get; set; }
        public string Number { get; set; }
        public double Amount { get; set; }
        public string Source { get; set; } //app || website
        //Electricity
        public string CusAccNum { get; set; }
        public string BU { get; set; }
        public string CySubDivision { get; set; }
        public string DueDate { get; set; }
        //Insurance
        public string InsuranceDob { get; set; }
    }


    public class User
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }

    public class Validation
    {
        public static bool _IsSuccess = false;
        public static string UserCheck(User user)
        {
            string retVal = string.Empty;
            SqlCommand userCmd = new SqlCommand();
            userCmd.Parameters.AddWithValue("@UserId", user.UserId);
            userCmd.Parameters.AddWithValue("@Key", user.Password);
            userCmd.Parameters.AddWithValue("@Version", "1.0");
            userCmd.Parameters.AddWithValue("@UserType", 4);
            userCmd.CommandType = CommandType.StoredProcedure;
            userCmd.CommandText = DL_StoreProcedure.SP_DHS_API_CheckUser;//sp
            DataSet ds_user = (new DataBase()).SelectAdaptQry(userCmd); //validate user 
            if (ds_user != null && ds_user.Tables.Count > 0 && Convert.ToInt64(ds_user.Tables[0].Rows[0]["CountId"]) == 1) //validate user
            {
                retVal = JsonConvert.SerializeObject(ds_user);
                _IsSuccess = true;
            }

            return retVal;

        }

    }
}