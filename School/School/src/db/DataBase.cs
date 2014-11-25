using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace School.src.db
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

        public DataSet GetDataSet(string commandText, params MySqlParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            using (MySqlConnection conn = new MySqlConnection(this.ConnStr))
            {
                MySqlCommand cmd = new MySqlCommand(commandText, conn);
                if (commandParameters != null)
                {
                    for (int i = 0; i < commandParameters.Length; i++)
                    {
                        cmd.Parameters.Add(commandParameters[i]);
                    }
                }
                conn.Open();
                cmd.CommandType = CommandType.Text;
                // Create the DataAdapter & DataSet
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    da.Fill(ds);
                    return ds;
                }
            }

        }

        public DataSet SelectAdaptQry(MySqlCommand cmdParam)
        {

            DataSet dataSet = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            using (MySqlConnection con = new MySqlConnection(this.ConnStr))
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