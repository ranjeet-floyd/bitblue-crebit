using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using db;

namespace CrebitAdminPanelNew
{
    public partial class Transaction_Page : System.Web.UI.Page
    {
        public int SuccessCount, FailedCount, PendingCount, InProgressCount, otherCount, RejectCount, ReceivedCount, NotKnownCount, AwaitingCount;
        public float  failed_AmountCount, failed_ElecAmount,failed_FundAmount, failed_MoneyTransferAmount, failed_cyberPlate;
         public float success_AmountCount, success_ElecAmount,success_FundAmount, success_MoneyTransferAmount, success_cyberPlate;
         public float pending_AmountCount, pending_ElecAmount,pending_FundAmount, pending_MoneyTransferAmount, pending_cyberPlate;
         public float InPro_AmountCount, InPro_ElecAmount,InPro_FundAmount, InPro_MoneyTransferAmount, InPro_cyberPlate;
        public int dateAmountAbstractor = 0;
        private string UserId = string.Empty;
        public string QueryString;
        public int type = 0;
        public string value = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            QueryString = Request.QueryString["u"];

            if (string.IsNullOrEmpty(QueryString))
                Server.Transfer("Login.aspx");
            if (!IsPostBack)
            {

                try
                {
                    int Id = 0;
                    string[] qArray = Request.QueryString["u"].ToString().Split('|');
                    string key = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(qArray[1]));
                    string userId = qArray[0].ToString();
                    Handler obj = new Handler();
                    Id = obj.Checker(userId, key);

                    if (Id != 0)
                    {
                        table_data.InnerHtml = GetTransactionData(0, "0");
                    }
                    else
                    {
                        Response.Redirect("Login.aspx");
                    }
                }
                catch (Exception ex)
                {
                    Response.Redirect("Login.aspx");
                }

            }

        }

        
        protected string GetTransactionData(int type, string value)
        {
            string htmlStr = "";
            try
            {
                SuccessCount = FailedCount = PendingCount = InProgressCount = otherCount = RejectCount = ReceivedCount = NotKnownCount = AwaitingCount = 0;
                 failed_AmountCount = failed_ElecAmount = failed_FundAmount = failed_MoneyTransferAmount = failed_cyberPlate=0.0F;
                 success_AmountCount = success_ElecAmount = success_FundAmount = success_MoneyTransferAmount = success_cyberPlate=0.0F;
                 pending_AmountCount = pending_ElecAmount = pending_FundAmount = pending_MoneyTransferAmount = pending_cyberPlate=0.0F;
                 InPro_AmountCount = InPro_ElecAmount = InPro_FundAmount = InPro_MoneyTransferAmount = InPro_cyberPlate=0.0F;
                string ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                SqlConnection thisConnection = new SqlConnection(ConnectionString);
                SqlCommand thisCommand = thisConnection.CreateCommand();
                thisCommand.CommandType = CommandType.StoredProcedure;
                thisCommand.CommandText = "CB_ADMIN_TRANSACTION";
                thisCommand.Parameters.AddWithValue("@Type", type);
                thisCommand.Parameters.AddWithValue("@Value", value);
                DataBase db = new DataBase();
                DataSet ds = db.SelectAdaptQry(thisCommand);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataRowCollection drc = ds.Tables[0].Rows;
                    foreach (DataRow item in drc)
                    {

                        string Id = item["Id"].ToString();
                        string UserName = item["UserName"].ToString();
                        string ApiTransactionId = "" + item["ApiTransactionId"].ToString();
                        string OperaterName = item["OperaterName"].ToString();
                        string Amount = item["Amount"].ToString();
                        string ServiceType = "" + item["ServiceType"].ToString();
                        string CreditAccountNo = "" + item["CreditAccountNo"].ToString();
                        string Date = Convert.ToDateTime(item["Date"]).ToString("d MMM yyyy h:mm tt ");
                        string CyberSessionId = "" + item["CyberSessionId"].ToString();
                        string Source = "" + item["Source"].ToString();
                        int Status = Convert.ToInt32(item["Status"]);
                        string APiId = "" + item["APiId"].ToString();
                        int ServiceId = Convert.ToInt32(item["ServiceId"]);
                        //string statusHtml = "";
                        string statusText = string.Empty;

                        if (dateAmountAbstractor == 0 )
                        {
                            DateTime fromdate = Convert.ToDateTime(Date);
                            string date = fromdate.GetDateTimeFormats('d')[0];
                            DateTime now = DateTime.Now;
                            string nowdate = now.GetDateTimeFormats('d')[0];
                            
                            if (nowdate.Equals(date))
                            {
                                switch (Status)
                                {
                                    case 0:
                                        FailedCount += 1;
                                        failed_AmountCount += float.Parse(Amount);
                                        switch (ServiceId)
                                        {
                                            case 38: failed_ElecAmount += float.Parse(Amount); break;
                                            case 55: failed_FundAmount += float.Parse(Amount); break;
                                            case 57: failed_MoneyTransferAmount += float.Parse(Amount); break;


                                        }
                                        failed_cyberPlate = failed_AmountCount - (failed_ElecAmount + failed_FundAmount + failed_MoneyTransferAmount);
                                        break;

                                    case 1:
                                        SuccessCount += 1;
                                        success_AmountCount += float.Parse(Amount);
                                        switch (ServiceId)
                                        {
                                            case 38: success_ElecAmount += float.Parse(Amount); break;
                                            case 55: success_FundAmount += float.Parse(Amount); break;
                                            case 57: success_MoneyTransferAmount += float.Parse(Amount); break;


                                        }
                                        success_cyberPlate = success_AmountCount - (success_ElecAmount + success_FundAmount + success_MoneyTransferAmount);
                                        break;

                                    case 2:
                                        PendingCount += 1;
                                        pending_AmountCount += float.Parse(Amount);
                                        switch (ServiceId)
                                        {
                                            case 38: pending_ElecAmount += float.Parse(Amount); break;
                                            case 55: pending_FundAmount += float.Parse(Amount); break;
                                            case 57: pending_MoneyTransferAmount += float.Parse(Amount); break;


                                        }
                                        pending_cyberPlate = pending_AmountCount - (pending_ElecAmount + pending_FundAmount + pending_MoneyTransferAmount);
                                        break;
                                    case 3:
                                        InProgressCount += 1;
                                        InPro_AmountCount += float.Parse(Amount);
                                        switch (ServiceId)
                                        {
                                            case 38: InPro_ElecAmount += float.Parse(Amount); break;
                                            case 55: InPro_FundAmount += float.Parse(Amount); break;
                                            case 57: InPro_MoneyTransferAmount += float.Parse(Amount); break;


                                        }
                                        InPro_cyberPlate = InPro_AmountCount - (InPro_ElecAmount + InPro_FundAmount + InPro_MoneyTransferAmount);
                                        break;
                                }
                         }
                        }

                       
                        else
                        {
                            switch (Status)
                            {
                                case 0:
                                    FailedCount += 1;
                                    failed_AmountCount += float.Parse(Amount);
                                    switch (ServiceId)
                                    {
                                        case 38: failed_ElecAmount += float.Parse(Amount); break;
                                        case 55: failed_FundAmount += float.Parse(Amount); break;
                                        case 57: failed_MoneyTransferAmount += float.Parse(Amount); break;


                                    }
                                    failed_cyberPlate = failed_AmountCount - (failed_ElecAmount + failed_FundAmount + failed_MoneyTransferAmount);
                                    break;

                                case 1:
                                    SuccessCount += 1;
                                    success_AmountCount += float.Parse(Amount);
                                    switch (ServiceId)
                                    {
                                        case 38: success_ElecAmount += float.Parse(Amount); break;
                                        case 55: success_FundAmount += float.Parse(Amount); break;
                                        case 57: success_MoneyTransferAmount += float.Parse(Amount); break;


                                    }
                                    success_cyberPlate = success_AmountCount - (success_ElecAmount + success_FundAmount + success_MoneyTransferAmount);
                                    break;

                                case 2:
                                    PendingCount += 1;
                                    pending_AmountCount += float.Parse(Amount);
                                    switch (ServiceId)
                                    {
                                        case 38: pending_ElecAmount += float.Parse(Amount); break;
                                        case 55: pending_FundAmount += float.Parse(Amount); break;
                                        case 57: pending_MoneyTransferAmount += float.Parse(Amount); break;


                                    }
                                    pending_cyberPlate = pending_AmountCount - (pending_ElecAmount + pending_FundAmount + pending_MoneyTransferAmount);
                                    break;
                                case 3:
                                    InProgressCount += 1;
                                    InPro_AmountCount += float.Parse(Amount);
                                    switch (ServiceId)
                                    {
                                        case 38: InPro_ElecAmount += float.Parse(Amount); break;
                                        case 55: InPro_FundAmount += float.Parse(Amount); break;
                                        case 57: InPro_MoneyTransferAmount += float.Parse(Amount); break;


                                    }
                                    InPro_cyberPlate = InPro_AmountCount - (InPro_ElecAmount + InPro_FundAmount + InPro_MoneyTransferAmount);
                                    break;
                            }
                        }



                        switch (Status)
                        {
                            case 1:
                                //statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-success dropdown-toggle' data-toggle='dropdown'>";
                                //SuccessCount += 1;
                                statusText = "Success";
                                break;
                            case 0:
                                //statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-danger dropdown-toggle' data-toggle='dropdown'>";
                                //FailedCount += 1;
                                statusText = "Failed";

                                break;
                            case 2:
                                //statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-warning dropdown-toggle' data-toggle='dropdown'>";
                                //PendingCount += 1;
                                statusText = "Pending";
                                break;
                            case 3:
                                //statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>";
                                //InProgressCount += 1;
                                statusText = "In Progress";
                                break;
                            case 4:
                                //statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-warning dropdown-toggle' data-toggle='dropdown'>";
                                //RejectCount += 1;
                                statusText = "Reject";
                                break;
                            case 5:
                                //statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-primary dropdown-toggle' data-toggle='dropdown'>";
                                //ReceivedCount += 1;
                                statusText = "Received";
                                break;
                            case 7:
                               // statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-warning dropdown-toggle' data-toggle='dropdown'>";
                                //NotKnownCount += 1;
                                statusText = "Not Known";
                                break;
                            case 8:
                               // statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-active  dropdown-toggle' data-toggle='dropdown'>";
                                //AwaitingCount += 1;
                                statusText = "Awaiting";
                                break;
                            case 9:
                                statusText = "Refunded";
                                break;
                            default:
                              //  statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-default dropdown-toggle' data-toggle='dropdown'>";
                                //otherCount += 1;
                                statusText = "Others";
                                break;

                        }               


                            htmlStr += "<tr><td>" + Id + "</td><td>" + UserName + "	</td><td>" + ApiTransactionId + "	</td><td>" + OperaterName + "/" + ServiceType
                               + "</td><td>" + Amount + "</td><td>" + APiId +
                               "</td><td>" + CreditAccountNo +
                           "</td><td>" + CyberSessionId + "</td><td>" + Date + "	</td><td>" + statusText + "</td><td><div class='btn-group'><button type='button' value='Check Status'  class='btn btn-default'  data-toggle='modal' data-target='.status_model' > </button></div></td></tr>";


                    }
                }

            }
            catch (Exception ex)
            {

                error_text.ForeColor = System.Drawing.Color.Red;
                error_text.Text = "Enter Correct Data!";
            }
            return htmlStr;
        }

        // filter Button 
        protected void btnFilter_ServerClick(object sender, EventArgs e)
        {

            dateAmountAbstractor = 1;
            type = Int32.Parse(SeletionList.Value);

            switch (type)
            {
                case 3:
                    value = serviceList.Value;
                    switch (value)
                    {
                        case "1": value = "PostPaid"; break;
                        case "2": value = "PrePaid"; break;
                        case "3": value = "DTH"; break;
                        case "4": value = "Electricity"; break;
                        case "5": value = "Gas Bill"; break;
                        case "6": value = "Insurance"; break;
                        case "7": value = "BroadBand"; break;
                        case "8": value = "Data Card"; break;
                        case "9": value = "Fund Transfer"; break;
                        case "10": value = "Bank Transfer"; break;
                    }
                    break;
                case 4:
                    value = inputtxtDate.Value;
                    break;
                case 6:
                    value = statusList.Value;
                    break;

                case 7:
                    value = operaterName.Value;
                    switch (value)
                    {

                        case "1": value = "Airtel Landline"; break;
                        case "2": value = "Airtel"; break;
                        case "3": value = "Cellone"; break;
                        case "4": value = "Idea"; break;
                        case "5": value = "Loop Mobile"; break;
                        case "6": value = "Reliance"; break;
                        case "7": value = "Tata Docomo"; break;
                        case "8": value = "Tata TeleServices"; break;
                        case "9": value = "Vodafone"; break;
                        case "10": value = "Aircel"; break;
                        case "11": value = "Airtel"; break;
                        case "12": value = "BSNL"; break;
                        case "13": value = "BSNL(Validity/Special)"; break;
                        case "14": value = "Idea"; break;
                        case "15": value = "Loop"; break;
                        case "16": value = "MTNL(TopUp)"; break;
                        case "17": value = "MTNL(Validity)"; break;
                        case "18": value = "MTS"; break;
                        case "19": value = "Reliance(CDMA)"; break;
                        case "20": value = "Reliance(GSM)"; break;
                        case "21": value = "T24(Flexi)	"; break;
                        case "22": value = "T24(Special)"; break;
                        case "23": value = "Tata Docomo(Flexi)	"; break;
                        case "24": value = "Tata Docomo(Special)"; break;
                        case "25": value = "Tata Indicom	"; break;
                        case "26": value = "Uninor"; break;
                        case "27": value = "Videocon"; break;
                        case "28": value = "Virgin(CDMA)"; break;
                        case "29": value = "Virgin(GSM/Flexi)"; break;
                        case "30": value = "Virgin(GSM/Special)"; break;
                        case "31": value = "Vodafone"; break;
                        case "32": value = "Airtel Digital TV"; break;
                        case "33": value = "Big TV"; break;
                        case "34": value = "Dish TV"; break;
                        case "35": value = "Sun Direct	"; break;
                        case "36": value = "Tata Sky(B2C)"; break;
                        case "37": value = "Videocon d2h"; break;
                        case "38": value = "MSEB"; break;
                        case "41": value = "Reliance(Mumbai)"; break;
                        case "42": value = "Mahanagar Gas Limited"; break;
                        case "43": value = "ICICI Pru. Life"; break;
                        case "44": value = "Tata AIG Life"; break;
                        case "45": value = "Tikona Postpaid"; break;
                        case "46": value = "Aircel"; break;
                        case "47": value = "Airtel"; break;
                        case "48": value = "BSNL"; break;
                        case "49": value = "Idea"; break;
                        case "50": value = "MTS"; break;
                        case "51": value = "Reliance"; break;
                        case "52": value = "Tata Docomo"; break;
                        case "53": value = "Tata Indicom"; break;
                        case "55": value = "Crebit Fund Transfer"; break;
                        case "56": value = "Crebit Monthly Charge"; break;
                        case "57": value = "Money Transfer"; break;
                        case "58": value = "Crebit Refund Req."; break;



                    }


                    break;
                default:
                    value = inputControl.Text;
                    break;
            }
            table_data.InnerHtml = GetTransactionData(type, value);
        }
        //protected void btnInsert_ServerClick(object sender, EventArgs e)
        //{
        //    //try
        //    //{
        //    //string tran = inputTransactionToggleForm.Text;
        //    //string comment = inputCommentToggleForm.Text;
        //    //int tblId = Int32.Parse(hdnBtnId.Value);
        //    //int tbstatus = Int32.Parse(hdbBtnLi.Value);
        //    //Handler obj = new Handler();
        //    //obj.AddBankTranCommentData(tblId, tran, comment, tbstatus);
        //    //table_data.InnerHtml = GetBankTransFilterDetails(type, value);

        //    //    }
        //    //catch (Exception ex) { }
        //}

        //protected void btnClose_ServerClick(object sender, EventArgs e)
        //{


        //}
    }
}