using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using api.dhs.Logging;

namespace retailer.Handler
{
    public class DataBase
    {
        private static String strConn = strConn = ConfigurationManager.ConnectionStrings["connString"].ToString();

        public static DataSet SelectAdaptQry(SqlCommand cmdParam)
        {

            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            using (SqlConnection con = new SqlConnection(strConn))
            {
                try
                {
                    cmdParam.Connection = con;
                    adapter.SelectCommand = cmdParam;
                    adapter.Fill(dataSet);
                }
                catch (Exception err)
                {
                    Logger.WriteLog(LogLevelL4N.FATAL, err.Message);
                }
            }
            return dataSet;
        }

        public static DataSet SelectAdaptQry(SqlCommand cmdParam, User user)
        {
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            using (SqlConnection con = new SqlConnection(strConn))
            {
                try
                {
                    cmdParam.Connection = con;
                    adapter.SelectCommand = cmdParam;
                    adapter.Fill(dataSet);
                }
                catch (Exception err)
                {
                    Logger.WriteLog(LogLevelL4N.FATAL, err.Message);
                }
            }
            return dataSet;
        }

    }
}
