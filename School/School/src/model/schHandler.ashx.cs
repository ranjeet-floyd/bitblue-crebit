using MySql.Data.MySqlClient;
using School.src.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
namespace School.src.model
{
    /// <summary>
    /// Summary description for schHandler
    /// </summary>
    public class schHandler : IHttpHandler
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

        public void ProcessRequest(HttpContext context)
        {
            string MethodName = string.Empty; string q = string.Empty;
            try
            {
                var content = context.Request.Form;
                context.Response.ContentType = "application/json";
                if (context.Request.QueryString["MethodName"] != null)
                    MethodName = context.Request.QueryString["MethodName"].ToString();
                else
                {
                    if (content["methodName"] != null)
                        MethodName = content["methodName"].ToString();
                }

                if (context.Request.QueryString["q"] != null)
                    q = context.Request.QueryString["q"].ToString();

                //string bodys = context.Request.Files[0];

                switch (MethodName)
                {
                    case "GetStandard":
                        context.Response.Write(this.GetStandard(q));
                        break;
                    case "GetSection":
                        context.Response.Write(this.GetSection(q));
                        break;
                    case "SubmitAttendance":
                        context.Response.Write(this.SubmitAttendance(q));
                        break;
                    case "GetAttendance":
                        context.Response.Write(this.GetAttendance(q));
                        break;
                    case "GetSMSNumbers":
                        context.Response.Write(this.GetSMSNumbers(q));
                        break;
                    case "SendSMS":
                        context.Response.Write(this.SendSMS(content));
                        break;
                    case "GetMobileByGRNumber":
                        context.Response.Write(this.GetMobileByGRNumber(q));
                        break;
                    default:
                        context.Response.Write("Wrong Method");
                        break;
                }
            }
            catch (Exception ex) { context.Response.Write("Exception" + ex.Message); }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        //On Drop Down
        private string GetStandard(string q)
        {
            string _error = "";
            try
            {
                string Medium = q.Split('|')[0];
                string qtext = "SELECT  Std  FROM sch_class where Medium = @Medium  Order by Std";
                MySqlParameter[] mySqlParameter = new MySqlParameter[1];
                mySqlParameter[0] = new MySqlParameter("@Medium", Medium);
                return JsonConvert.SerializeObject((new DataBase()).GetDataSet(qtext, mySqlParameter), Formatting.None);
            }
            catch (Exception ex) { _error = "Exception" + ex.Message; }
            return _error;
        }

        //On Drop Down Section
        private string GetSection(string q)
        {
            string _error = "";
            try
            {
                string[] datas = q.Split('|');
                string qtext = "SELECT  no_of_div As 'div'  FROM sch_class where Medium = @Medium AND Std = @Std ";
                MySqlParameter[] mySqlParameter = new MySqlParameter[2];
                mySqlParameter[0] = new MySqlParameter("@Medium", datas[0]);
                mySqlParameter[1] = new MySqlParameter("@Std", datas[1]);
                return JsonConvert.SerializeObject((new DataBase()).GetDataSet(qtext, mySqlParameter), Formatting.None);
            }
            catch (Exception ex) { _error = "Exception" + ex.Message; }
            return _error;
        }

        private string SubmitAttendance(string q)
        {
            string retVal = "";
            string qtext = "";
            try
            {
                string[] grNums = q.Trim('_').Split('_');
                string grNumumers = "";
                foreach (string item in grNums)
                {
                    grNumumers += "'" + item + "',";
                    qtext += "Insert into  sch_attendance (grNum,date,isActive,presentDays,absentDays) values ('" + item + "','" + indianTime.ToString("yyyy-MM-dd") + "',1,0,0) ; ";
                }
                qtext += " select 1 As Status; SELECT  sch_details.cont_num As 'mobile'  FROM sch_details where Gr_num in (" + grNumumers.Trim(',') + ")";

                MySqlParameter[] mySqlParameter = null;

                DataSet ds = (new DataBase()).GetDataSet(qtext, mySqlParameter);
                if (ds != null && ds.Tables.Count > 1)
                {
                    //send sms
                    string mobiles = string.Empty;
                    Task t = new Task(() =>
                    {
                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            mobiles += "+91" + row["mobile"] + ";";
                        }
                        string message = ConfigurationManager.AppSettings["absentSMS"].ToString();

                        SMS.SendSMS(mobiles, message);

                    });
                    t.Start();//start parallel task

                    return JsonConvert.SerializeObject(ds.Tables[0], Formatting.None);
                }
            }
            catch (Exception ex) { retVal = "Exception" + ex.Message; }
            return retVal;

        }

        private string GetAttendance(string q)
        {
            string qtext = "";
            string retVal = "";
            try
            {
                qtext = "select DISTINCT  date as AbsentDate FROM  sch_attendance  WHERE grNum = '" + q.Trim() + "' AND isActive = 1; ";

                MySqlParameter[] mySqlParameter = null;
                return JsonConvert.SerializeObject((new DataBase()).GetDataSet(qtext, mySqlParameter).Tables[0], Formatting.None);
            }
            catch (Exception ex) { retVal = "Exception" + ex.Message; }
            return retVal;

        }

        private string GetSMSNumbers(string q)
        {
            string[] datas = q.Split('|');

            string qtext = "";
            string retVal = "";
            try
            {
                qtext = "SELECT  sch_details.cont_num As 'mobile'  FROM sch_details "
                + " Inner Join user_sch ON sch_details.Gr_num = user_sch.Gr_num where user_sch.Medium = @Medium AND user_sch.Std = @Std  and user_sch.Section = @Section ";
                MySqlParameter[] mySqlParameter = new MySqlParameter[3];
                mySqlParameter[0] = new MySqlParameter("@Medium", datas[0]);
                mySqlParameter[1] = new MySqlParameter("@Std", datas[2]);
                mySqlParameter[2] = new MySqlParameter("@Section", datas[1]);
                return JsonConvert.SerializeObject((new DataBase()).GetDataSet(qtext, mySqlParameter), Formatting.None);
            }
            catch (Exception ex) { retVal = "Exception" + ex.Message; }
            return retVal;
        }

        private bool SendSMS(NameValueCollection datas)
        {
            //send sms
            StringWriter mobilesData = new StringWriter();
            StringWriter messageData = new StringWriter();
            HttpUtility.HtmlDecode(datas["mobiles"], mobilesData);
            HttpUtility.HtmlDecode(datas["message"], messageData);

            string[] mobiles = mobilesData.ToString().Trim(',').Split(',');
            string smsMobiles = "";
            Task t = new Task(() =>
            {
                foreach (string mobile in mobiles)
                {
                    smsMobiles += "91" + mobile + ",";
                }

                SMS.SendSMS(smsMobiles, messageData.ToString());

            });
            t.Start();//start parallel task
            return true;
        }

        private string GetMobileByGRNumber(string q)
        {
            string mobile = "0";
            if (!string.IsNullOrEmpty(q))
            {
                string qtext = "SELECT  sch_details.cont_num As 'mobile'  FROM sch_details";
                qtext += " where Gr_num = @Gr_num";
                MySqlParameter[] mySqlParameter = new MySqlParameter[1];
                mySqlParameter[0] = new MySqlParameter("@Gr_num", q.Trim());
                DataSet ds = (new DataBase()).GetDataSet(qtext, mySqlParameter);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    mobile = ds.Tables[0].Rows[0]["mobile"].ToString();
                }

            }
            return mobile;
        }

    }
}