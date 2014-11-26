using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Security.Cryptography;

namespace CrebitAdminPanelNew
{
    //public static class StoreProc
    //{
    //    public const string CB_BankTransferTable_FILTER = "CB_BankTransferTable_FILTER";
    //    public const string CB_ADMIN_REFUND_REQUEST = "CB_ADMIN_REFUND_REQUEST";
    //}

    public class Handler
    {
        public int Checker(String UserId, String Password)
        {
            int Id = 0;

            string constr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {

                    cmd.Connection = con;
                    cmd.CommandText = "CB_ADMIN_LOGIN";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Connection = con;
                    con.Open();
                    Id = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }

            }


            return Id;
        }


        public int AddTranCommentData(int Id, string transactionId, string comment, int status)
        {

            string constr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "Cb_ElectricityBillRequests_StatusChange";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@MSEBTransactionId", transactionId);
                    cmd.Parameters.AddWithValue("@Comments", comment);
                    cmd.Parameters.AddWithValue("@Status", status);
                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    con.Close();
                }
            } return Id;
        }
        public int AddBankTranCommentData(int Id, string transactionId, string comment, int status)
        {

            string constr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "CB_BankTransferRequest_StatusChange";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@BankTransactionId", transactionId);
                    cmd.Parameters.AddWithValue("@Comments", comment);
                    cmd.Parameters.AddWithValue("@Status", status);
                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    con.Close();
                }
            } return Id;
        }
        public int AddRefundTranCommentData(int Id, string comment, int status)
        {

            string constr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "RefundRequest_Trans_Comment";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Comments", comment);
                    cmd.Parameters.AddWithValue("@Status", status);
                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    con.Close();
                }
            } return Id;
        }
    }
}
