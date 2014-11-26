using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Services;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using db;
namespace CrebitAdminPanelNew
{
    public partial class RefundRequest : System.Web.UI.Page
    {
        public int SuccessCount, FailedCount, PendingCount, InProgressCount, otherCount, RejectCount, ReceivedCount, NotKnownCount, AwaitingCount;
        private string UserId = string.Empty;
        public string QueryString;
        public int type = 0;
        public string value = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            QueryString = Request.QueryString["u"];
            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(QueryString))
                    Server.Transfer("Login.aspx");
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
                        table_data.InnerHtml = getRefundRequestFilterData(type, value);
                    }
                    else
                    { Response.Redirect("Login.aspx"); }
                }
                catch (Exception ex) { Trace.Warn(ex.Message); Response.Redirect("Login.aspx"); }
            }

        }
        public string getRefundRequestFilterData(int type, string value)
        {
            string htmlStr = "";
            try
            {
                SuccessCount = FailedCount = PendingCount = InProgressCount = otherCount = RejectCount = ReceivedCount = NotKnownCount = AwaitingCount = 0;

                string ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

                SqlConnection thisConnection = new SqlConnection(ConnectionString);
                SqlCommand thisCommand = thisConnection.CreateCommand();
                thisCommand.CommandType = CommandType.StoredProcedure;
                thisCommand.CommandText = "CB_ADMIN_REFUND_NEW_REQUEST";
                thisCommand.Parameters.AddWithValue("@Type", type);
                thisCommand.Parameters.AddWithValue("@Value", value);
                DataBase db = new DataBase();
                DataSet ds = db.SelectAdaptQry(thisCommand);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataRowCollection drc = ds.Tables[0].Rows;
                    foreach (DataRow item in drc)
                    {

                        string Id = "" + item["Id"].ToString();
                        string UserId = "" + item["UserName"].ToString();
                        string Service = "" + item["OperaterName"].ToString();
                        string Type = "" + item["ServiceType"].ToString();
                        string AccountNo = "" + item["CreditAccountNo"].ToString();
                        string ReqDate = Convert.ToDateTime(item["ReqDate"]).ToString("d MMM yyyy h:mm tt ");
                        string TransactionId = "" + item["TransactionId"].ToString();
                        //string RefundTransactionId = "" + item["RefundTransId"].ToString();
                        String Comments = "" + item["Comment"].ToString();
                        int Status = Convert.ToInt32(item["Status"]);
                        string statusHtml = "";
                        string statusText = string.Empty;

                        switch (Status)
                        {
                            case 9:
                                statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-success dropdown-toggle' data-toggle='dropdown' disabled>";
                                SuccessCount += 1;
                                statusText = "Refunded";
                                //statusHtml += "<span class='caret'></span><span class='sr-only'>Toggle Dropdown</span>  </button> ";
                                //statusHtml += "<ul id='selectionToggle' class='dropdown-menu' role='menu'>";
                                //statusHtml += "<li ><a id='atag_" + Id + "_3' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>In Progress</a></li>";
                                //statusHtml += "<li ><a id='atag_" + Id + "_4' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Reject</a></li></ul>";
                                break;

                            case 3:
                                statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>";
                                InProgressCount += 1;
                                statusText = "In Progress";
                                statusHtml += "<span class='caret'></span><span class='sr-only'>Toggle Dropdown</span>  </button> ";
                                statusHtml += "<ul id='selectionToggle' class='dropdown-menu' role='menu'><li ><a id='atag_" + Id + "_9' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Refunded</a></li>";
                                statusHtml += "<li ><a id='atag_" + Id + "_4' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Reject</a></li></ul>";
                                break;
                            case 4:
                                statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-danger dropdown-toggle' data-toggle='dropdown' disabled>";
                                RejectCount += 1;
                                statusText = "Reject";
                                //statusHtml += "<span class='caret'></span><span class='sr-only'>Toggle Dropdown</span>  </button> ";
                                //statusHtml += "<ul id='selectionToggle' class='dropdown-menu' role='menu'><li ><a id='atag_" + Id + "_9' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Refunded</a></li>";
                                //statusHtml += "<li ><a id='atag_" + Id + "_3' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>In Progress</a></li>";
                                //statusHtml += "</ul>";

                                break;

                            default:
                                statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-default dropdown-toggle' data-toggle='dropdown'>";
                                otherCount += 1;
                                statusText = "Others";
                                statusHtml += "<span class='caret'></span><span class='sr-only'>Toggle Dropdown</span>  </button> ";
                                statusHtml += "<ul id='selectionToggle' class='dropdown-menu' role='menu'>";
                                statusHtml += "<li ><a id='atag_" + Id + "_3' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>In Progress</a></li>";
                                statusHtml += "</ul>";
                                break;

                        }

                        htmlStr += "<tr><td>" + Id + "</td><td>" + UserId + "</td><td>" + Service + "</td><td>" +
                        Type + "</td><td>" + AccountNo + "</td><td>" + ReqDate + "</td><td>" + TransactionId +
                        "</td><td><textarea>" + Comments + "</textarea></td><td>" + statusText + "<td><div class='btn-group dropup'>" + statusHtml;
                        htmlStr += "</div></td><td></tr>";
                    }
                }
            }
            catch (Exception ex)
            {
                error_text.ForeColor = System.Drawing.Color.Red;
                error_text.Text = "Enter Correct Data!";
                Trace.Warn(ex.Message);
            }
            return htmlStr;
        }

        protected void btnFilter_ServerClick(object sender, EventArgs e)
        {
            try
            {
                type = Int32.Parse(SeletionList.Value);
                switch (type)
                {
                    
                       case 2:

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
                            case "11": value = "Crebit Admin"; break;
                            case "12": value = "Crebit Monthly Charge"; break;
                            case "13": value = "Money Transfer"; break;
                            case "14": value = "Crebit Refund"; break;
                        }
                        break;


                    case 5:
                        value = inputtxtDate.Value.ToString();
                        break;

                    case 8:
                        value = statusList.Value;
                        break;
                    default:
                        value = inputControl.Text;
                        break;
                }
                table_data.InnerHtml = getRefundRequestFilterData(type, value);
            }
            catch (Exception ex) { Trace.Warn(ex.Message); }
        }

        protected void btnInsert_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string comment = inputCommentToggleForm.Text;
                int tblId = Int32.Parse(hdnBtnId.Value);
                int tbstatus = Int32.Parse(hdbBtnLi.Value);
                Handler obj = new Handler();
                obj.AddRefundTranCommentData(tblId, comment, tbstatus);
                table_data.InnerHtml = getRefundRequestFilterData(0, "0");
            }
            catch (Exception ex) { Trace.Warn(ex.Message); }

        }

        protected void btnClose_ServerClick(object sender, EventArgs e)
        {


        }
    }
}








