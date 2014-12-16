using crebit.retailer.Models.DL.Service;
using Newtonsoft.Json;
using System;
using System.Data;
using com.dhs.webapi.Model.DL.Common;
using api.dhs.Logging;
using System.Data.SqlClient;

namespace crebit.retailer.Models.BL.Service
{
    public class BL_Service
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        private string SpName { get; set; }
        [JsonIgnore]
        public bool _IsSuccess { get; set; }
        public int Status { get; set; }
        public string msg { get; set; }
        public double AvaiBal { get; set; }
        public DL_TorrentPowerReturn torrentPowerReturn;
        DataSet ds = null;
        DataBase db = new DataBase();
        //BL_Service bL_Service; //Commented| Ranjeet| 15-Dec || Not used
        //Used for return Torrent Power Details
        public DL_TorrentPowerReturn GetTorrentPowerDetails(DL_TorrentPower dL_TorrentPower)
        {
            //System.Net.ServicePointManager.CertificatePolicy = new BL_Service();
            string resString = string.Empty;
            this._IsSuccess = true;
            //string strCookie = string.Empty;
            DL_TorrentPowerReturn dL_TorrentPowerReturn = null;
            this.SpName = DL_StoreProcedure.SP_DHS_API_PayElectricity;
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@UserId", dL_TorrentPower.UserId);
                param[1] = new SqlParameter("@key", dL_TorrentPower.Key);
                param[2] = new SqlParameter("@ServiceId", 42);
                param[3] = new SqlParameter("@Amount", dL_TorrentPower.amount);
                param[4] = new SqlParameter("@BU", dL_TorrentPower.Bu);
                param[5] = new SqlParameter("@CusAcc", dL_TorrentPower.CusAcc);
                param[6] = new SqlParameter("@CusMob", dL_TorrentPower.cusMob);
                param[7] = new SqlParameter("@CyDiv", 42);
                param[8] = new SqlParameter("@DueDate", indianTime);
                param[9] = new SqlParameter("@Date", indianTime);
                 //indianTime 


                ds = db.GetDataSet(this.SpName, param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count==1)
                {
                    Logger.WriteLog(LogLevelL4N.INFO, "Got Data from Db.");
                    DataRow dr = ds.Tables[0].Rows[0];
                    dL_TorrentPowerReturn = new DL_TorrentPowerReturn() { Status = Convert.ToInt32(dr.ItemArray[1]), Message = "", AvaiBal=Convert.ToDouble(dr.ItemArray[0]) };
                    //AvaiBal = Convert.ToInt32(dr["AvaiBal"]),Message = "Successfull Transaction" 
                }
            }
            catch (Exception ex) { Logger.WriteLog(LogLevelL4N.INFO, "BL_Service |Torrent : "+ ex.Message); }
            
            //WebRequest request = HttpWebRequest.Create("https://bill.torrentpower.com/viewbill.aspx");
            //request.Proxy = null;
            ////request.Method = "POST";
                
           ////var response = request.UploadValues("https://bill.torrentpower.com/viewbill.aspx", "GET", data);
            
                //using (var wb = new WebClient())
                //{
                    

                //    var data = new NameValueCollection();
                //    System.Net.ServicePointManager.Expect100Continue = false;
                //    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                //    data["ctl00$cph1$drpCity"] = "1";
                //    data["ctl00$cph1$txtServiceNo"] = "449922";
                //    data["ctl00$cph1$btnSubmit"] = "View bill";
                //    wb.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36";
                //    wb.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                //    wb.Headers[HttpRequestHeader.Accept] = "*/*";
                //    wb.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                //    wb.Headers[HttpRequestHeader.AcceptLanguage] = "en-US,en;q=0.8,hi;q=0.6";
                //    var response = wb.UploadValues("https://bill.torrentpower.com/viewbill.aspx", "POST", data);
                //    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                //    byte[] buffer = null;
                //    buffer = new byte[response.Length];
                //    resString = System.Text.Encoding.UTF8.GetString(response);
                //    
            //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                //{
                //    // strCookie = response.Cookies["ASP.NET_SessionId"].ToString();
                //    strCookie = response.Headers["Set-Cookie"].ToString().Split('=')[1].Split(';')[0].Trim();
                //    //ASP.NET_SessionId
                //}


                ////using (var wb = new WebClient())
                ////{
                ////    var data = new NameValueCollection();
                ////    System.Net.ServicePointManager.Expect100Continue = false;
                ////    data["Cookie"] = strCookie;//"vwc31zq3oelkhh45ocjymg45";
                ////    wb.Headers[HttpRequestHeader.Host] = "bill.torrentpower.com";
                ////    wb.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36";
                ////    wb.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                //    wb.Headers[HttpRequestHeader.Cookie] = "ASP.NET_SessionId=" + strCookie;
                //    wb.Headers[HttpRequestHeader.Accept] = "*/*";
                //    wb.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                //    wb.Headers[HttpRequestHeader.AcceptLanguage] = "en-US,en;q=0.8,hi;q=0.6";
                //    var response = wb.UploadValues("https://bill.torrentpower.com/billdetails.aspx", "GET", data);

                //    byte[] buffer = null;
                //    buffer = new byte[response.Length];
                //    resString = System.Text.Encoding.UTF8.GetString(response);



                //if (!string.IsNullOrEmpty(strCookie))
                //{
                //    WebRequest detailRequest = HttpWebRequest.Create("https://bill.torrentpower.com/billdetails.aspx");
                //    detailRequest.Proxy = null;
                //    detailRequest.ContentType = "application/x-www-form-urlencoded";
                //    detailRequest.Method = "GET";
                //    detailRequest.Headers.Add("Cookie", "ASP.NET_SessionId=" + strCookie);
                //    detailRequest.Headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36");
                //    //byte[] buffer = null;
                //    using (HttpWebResponse response = (HttpWebResponse)detailRequest.GetResponse())
                //    {
                //        buffer = new byte[response.ContentLength];
                //        StreamReader myWebSource = new StreamReader(response.GetResponseStream());
                //        resString = myWebSource.ReadToEnd();


                //        //resString = resString.Replace("&nbsp;", "");
                //        // string s = File.ReadAllText(resString);
                        //long index = LinesCountIndexOf(s);

                        //int numLines = Convert.ToString(resString).Split('\n').Length;
                        //if (true)      //Max Lines
                        //{
                        //  string sp=resString.Remove(0, Convert.ToString(resString).Split('\n').Length);
                        //}

                        //var lines = 2;
                        //var idx = 0;
                        //for (var i = 0; i < lines; i++)
                        //{

                        //    idx = resString.IndexOf("table");
                        //    resString = resString.Substring(idx + 1);
                        //}



                        //HtmlDocument doc = new HtmlDocument();
                        //doc.Load(resString);
                        //foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//table#ctl00_cph1_tblHTML"))
                        //{
                        //    HtmlAttribute att = link.Attributes["/body/form#aspnetForm/table/tr/td/table/tbody/tr/td/table#ctl00_cph1_tblHTML"];
                        //    //att.Value = "http://www.google.com";
                        //}
                        //doc.Save("file.htm");

                        ////Create the XmlDocument.
                        //XmlDocument doc = new XmlDocument();
                        //doc.Load(resString);

                        ////Display all the 
                        //XmlNodeList elemList = doc.GetElementsByTagName("body/form#aspnetForm/table/tr/td/table/tbody/tr/td/table#ctl00_cph1_tblHTML");
                        //for (int i = 0; i < elemList.Count; i++)
                        //{





            return dL_TorrentPowerReturn;
            }
    }

    //public bool CheckValidationResult(ServicePoint srvPoint,
    //            X509Certificate certificate, WebRequest request,
    //                int certificateProblem)
    //{
    //    //Return True to force the certificate to be accepted.
    //    return true;
    //}

    //public void WriteCookie(string strCookieName, string strCookieValue)
    //{
    //    var hcCookie = new HttpCookie(strCookieName, strCookieValue);
    //    HttpContext.Current.Response.Cookies.Set(hcCookie);
    //}

    //}

    //   }
}