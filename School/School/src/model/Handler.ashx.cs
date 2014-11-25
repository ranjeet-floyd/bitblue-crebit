using MySql.Data.MySqlClient;
using School.src.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace School.src.model
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
            string MethodName = context.Request.QueryString["MethodName"].ToString();
            string q = context.Request.QueryString["q"].ToString();

            switch (MethodName)
            {
                case "GetStandard":
                    context.Response.Write(GetStandard(q));
                    break;
                case "GetSection":
                    context.Response.Write(GetSection(q));
                    break;
                default:
                    context.Response.Write("Wrong Method");
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string GetStandard(string q)
        {
            string[] datas = q.Split('|');
            string qtext = "select user_id, username from users";
            MySqlParameter[] mySqlParameter = null; //new MySqlParameter[1];
            //mySqlParameter[0] = new MySqlParameter("@Medium", txtGrNumber.Value);
            DataSet ds = (new DataBase()).GetDataSet(qtext, mySqlParameter);
            return qtext;
        }
        private string GetSection(string q)
        {
            string qtext = "select user_id, username from users";
            MySqlParameter[] mySqlParameter = null; //new MySqlParameter[1];
            //mySqlParameter[0] = new MySqlParameter("@Medium", txtGrNumber.Value);
            DataSet ds = (new DataBase()).GetDataSet(qtext, mySqlParameter);
            return qtext;
        }
    }
}