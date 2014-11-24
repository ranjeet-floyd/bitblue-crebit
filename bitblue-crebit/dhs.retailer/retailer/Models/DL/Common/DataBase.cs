using api.dhs.Logging;
using com.dhs.webapi.Model.Common;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace com.dhs.webapi.Model.DL.Common
{
    public class DataBase
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        private string ConnStr { get; set; }

        //Constructor
        public DataBase()
        {
            this.ConnStr = connectionString;

        }

        public DataSet GetDataSet(string commandText, params SqlParameter[] commandParameters)
        {
           // Logger.WriteLog(LogLevelL4N.INFO, "Inside GetDataSet Method");
          //  Logger.WriteLog(LogLevelL4N.INFO, "ConnStr : "+this.ConnStr);
            using (SqlConnection conn = new SqlConnection(this.ConnStr))
            {
                SqlCommand cmd = new SqlCommand(commandText, conn);
                if (commandParameters != null)
                {
                    for (int i = 0; i < commandParameters.Length; i++)
                    {
                        cmd.Parameters.Add(commandParameters[i]);
                        //Logger.WriteLog(LogLevelL4N.INFO, "Params : " + commandParameters[i] + " value :"+ commandParameters[i].Value);
                    }
                }
                cmd.CommandType = CommandType.StoredProcedure;
                // Create the DataAdapter & DataSet
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                   // Logger.WriteLog(LogLevelL4N.INFO, "Now Fill SqlDataAdapter");
                    da.Fill(ds);
                   // Logger.WriteLog(LogLevelL4N.INFO, "Return Ds");
                    // Return the dataset
                    return ds;
                }
            }

        }
        public  DataSet SelectAdaptQry(SqlCommand cmdParam)
        {

            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            using (SqlConnection con = new SqlConnection(this.ConnStr))
            {
                try
                {
                    cmdParam.Connection = con;
                    adapter.SelectCommand = cmdParam;
                    adapter.Fill(dataSet);
                }
                catch (Exception err)
                {
                    //log here
                }
            }
            return dataSet;
        }

        public  DataSet SelectAdaptQry(SqlCommand cmdParam, User user)
        {
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            using (SqlConnection con = new SqlConnection(this.ConnStr))
            {
                try
                {
                    cmdParam.Connection = con;
                    adapter.SelectCommand = cmdParam;
                    adapter.Fill(dataSet);
                }
                catch (Exception err)
                {
                    //log here
                }
            }
            return dataSet;
        }


    }
}