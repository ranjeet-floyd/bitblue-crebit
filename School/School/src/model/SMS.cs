///Used to send SMS.
using System;
using System.Configuration;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace School.src.model
{
    public static class SMS
    {
        public static bool _IsSMSMisMatch { get; set; }
        /// <summary>
        /// Function called to send SMS
        /// </summary>
        /// <param name="number"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool SendSMS(string mobile, string message)
        {
            bool _isSuccess = false;
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IS_SMS_ON"]))
            {
                _IsSMSMisMatch = false;

                string retVal = "";
                mobile = mobile.TrimEnd(',');
                try
                {
                    // Code for SmsLane
                    string sUserID = "preeti123456";
                    string sPwd = "199278";
                    // string sNumber = mobile;
                    string sSID = "THAKUR";
                    // string sMessage = msg;
                    //string strUrl = "http://smslane.com/vendorsms/pushsms.aspx?user=" + sUserID + "&password=" + sPwd + "&msisdn=" + mobile + "&sid=" + sSID + "&msg=" + message + "&mt=0&fl=0";
                    string strUrl = "http://smslane.com/vendorsms/pushsms.aspx?user=" + sUserID + "&password=" + sPwd + "&msisdn=" + mobile + "&sid=" + sSID + "&msg=" + message + "&mt=0&fl=0&gwid=2";
                    //  string sResponse = GetResponse(sURL);
                    //url tp send SMS through Netcore. This will send SMS even to DND customers.
                    //url = "http://api.mVaayoo.com/mvaayooapi/MessageCompose?user=safdar.khan@volkswagen.co.in:123456&senderID=VWSALE&receipientno=91" + number + "&dcs=0&msgtxt=" + message + "&state=4";
                    // url = "http://bulkpush.mytoday.com/BulkSms/SingleMsgApi?feedid=331928&username=9967335511&password=tdjgd&To=" + number + " &Text=" + message + "&time=&senderid=";
                    //Task t = new Task(() =>
                    //{
                    WebClient webClient = new WebClient();
                    byte[] reqHTML;
                    reqHTML = webClient.DownloadData(strUrl);
                    UTF8Encoding objUTF8 = new UTF8Encoding();
                    retVal = objUTF8.GetString(reqHTML);
                    //});

                    //t.Start();

                    if (retVal.Equals("Invalid template or template mismatch"))
                        _IsSMSMisMatch = true;
                    _isSuccess = true;
                }
                catch (Exception ex)
                {
                    _isSuccess = false;
                }
            }
            return _isSuccess;

        }
    }
}