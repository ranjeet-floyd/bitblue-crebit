using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace db
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
            using (SqlConnection conn = new SqlConnection(this.ConnStr))
            {
                SqlCommand cmd = new SqlCommand(commandText, conn);
                if (commandParameters != null)
                {
                    for (int i = 0; i < commandParameters.Length; i++)
                    {
                        cmd.Parameters.Add(commandParameters[i]);
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


    }
}