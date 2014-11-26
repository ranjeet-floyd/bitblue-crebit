using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using db;

namespace CrebitAdminPanelNew
{
    public partial class Electricity_page : System.Web.UI.Page
    {
        public int SuccessCount, FailedCount, PendingCount, InProgressCount, otherCount, RejectCount, ReceivedCount, NotKnownCount, AwaitingCount;
        public float totalAmount;
        public int dateAmountAbstractor = 0;
        private string UserId = string.Empty;
        public string QueryString;
        public int type = 0;
        public string value = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            QueryString = Request.QueryString["u"];
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(QueryString))
                {
                    Response.Redirect("Login.aspx");
                }
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
                        table_data.InnerHtml = getElectricityFilterData(0, null);
                    }
                    else
                    {
                        Response.Redirect("Login.aspx");

                    }
                }
                catch (Exception ex) { Trace.Warn(ex.Message); Response.Redirect("Login.aspx"); }
            }

        }


        public string getElectricityFilterData(int type, string value)
        {
            string htmlStr = "";
            string optionHtml = "";
            try
            {
                SuccessCount = FailedCount = PendingCount = InProgressCount = otherCount = RejectCount = ReceivedCount = NotKnownCount = AwaitingCount = 0;
                totalAmount = 0.0F;
                string ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                SqlConnection thisConnection = new SqlConnection(ConnectionString);
                SqlCommand thisCommand = thisConnection.CreateCommand();
                thisCommand.CommandType = CommandType.StoredProcedure;
                thisCommand.CommandText = "CB_ADMIN_ELECTRICITY_REQUEST";
                thisCommand.Parameters.AddWithValue("@Type", type);
                thisCommand.Parameters.AddWithValue("@Value", value);
                DataBase db = new DataBase();
                DataSet ds = db.SelectAdaptQry(thisCommand);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRowCollection drc = ds.Tables[0].Rows;
                    foreach (DataRow item in drc)
                    {

                        string Id = item["Id"].ToString();
                        string UserID = item["UserName"].ToString();
                        string Amount = item["Amount"].ToString();
                        string BUId = item["BUId"].ToString();
                        string CusAcc = item["CusAcc"].ToString();
                        string CusMob = item["CusMob"].ToString();
                        string DueDate = item["DueDate"].ToString();
                        string ReqDate = Convert.ToDateTime(item["ReqDate"]).ToString("d MMM yyyy h:mm tt ");
                        string TransactionId = item["TransactionId"].ToString();
                        string RefundTransactionId = "" + item["MESBTransactionId"].ToString();
                        String Comments = "" + item["Comment"].ToString();
                        int Status = Convert.ToInt32(item["Status"]);
                        string statusHtml = "";
                        //string commentNull = "";
                        string statusText = string.Empty;

                        if (Status == 1)
                        {
                            if (dateAmountAbstractor == 0)
                            {
                                DateTime fromdate = Convert.ToDateTime(ReqDate);
                                string date = fromdate.GetDateTimeFormats('d')[0];
                                DateTime now = DateTime.Now;
                                string nowdate = now.GetDateTimeFormats('d')[0];
                                if (nowdate.Equals(date))
                                {totalAmount += float.Parse(Amount); }
                            }
                            else
                            {
                                totalAmount += float.Parse(Amount);
                            }
                        }

                        optionHtml = "";
                        switch (Status)
                        {
                            case 1:
                                statusHtml = "<button type='button' id='btn_" + Id + "' disabled class='btn btn-success dropdown-toggle' data-toggle='dropdown'>";
                                SuccessCount += 1;
                                statusText = "Success";
                                //optionHtml += "<li ><a id='atag_" + Id + "_1' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Success</a></li>";
                                optionHtml += "<li><a id='atag_" + Id + "_0' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Failed</a></li>";
                                optionHtml += "<li ><a id='atag_" + Id + "_3' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>In Progress</a></li>";
                                optionHtml += "<li ><a id='atag_" + Id + "_4' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Reject</a></li>";
                                break;
                            case 0:
                                statusHtml = "<button type='button' id='btn_" + Id + "' disabled class='btn btn-danger dropdown-toggle' data-toggle='dropdown'>";
                                FailedCount += 1;
                                statusText = "Failed";
                                optionHtml += "<li ><a id='atag_" + Id + "_1' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Success</a></li>";
                                //optionHtml += "<li><a id='atag_" + Id + "_0' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Failed</a></li>";
                                optionHtml += "<li ><a id='atag_" + Id + "_3' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>In Progress</a></li>";
                                optionHtml += "<li ><a id='atag_" + Id + "_4' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Reject</a></li>";
                                break;
                            //case 2:
                            //    statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-warning dropdown-toggle' data-toggle='dropdown'>";
                            //    PendingCount += 1;
                            //    statusText = "Pending";
                            //    break;
                            case 3:
                                statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>";
                                InProgressCount += 1;
                                statusText = "In Progress";
                                optionHtml += "<li ><a id='atag_" + Id + "_1' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Success</a></li>";
                                optionHtml += "<li><a id='atag_" + Id + "_0' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Failed</a></li>  ";
                                //optionHtml += "<li ><a id='atag_" + Id + "_3' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>In Progress</a></li>";
                                optionHtml += "<li ><a id='atag_" + Id + "_4' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Reject</a></li>";
                                break;
                            case 4:
                                statusHtml = "<button type='button' id='btn_" + Id + "' disabled class='btn btn-warning dropdown-toggle' data-toggle='dropdown'>";
                                RejectCount += 1;
                                statusText = "Reject";
                                optionHtml += "<li ><a id='atag_" + Id + "_1' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Success</a></li>";
                                optionHtml += "<li><a id='atag_" + Id + "_0' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Failed</a></li> ";
                                optionHtml += "<li ><a id='atag_" + Id + "_3' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>In Progress</a></li>";
                                //optionHtml += "<li ><a id='atag_" + Id + "_4' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Reject</a></li>";
                                break;
                            //case 5:
                            //    statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-primary dropdown-toggle' data-toggle='dropdown'>";
                            //    ReceivedCount += 1;
                            //    statusText = "Received";
                            //    break;
                            //case 7:
                            //    statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-warning dropdown-toggle' data-toggle='dropdown'>";
                            //    NotKnownCount += 1;
                            //    statusText = "Not Known";
                            //    break;
                            //case 8:
                            //    statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-active  dropdown-toggle' data-toggle='dropdown'>";
                            //    AwaitingCount += 1;
                            //    statusText = "Awaiting";
                            //    break;
                            default:
                                statusHtml = "<button type='button' id='btn_" + Id + "' class='btn btn-default dropdown-toggle' data-toggle='dropdown'>";
                                otherCount += 1;
                                statusText = "Others";
                                optionHtml += "<li ><a id='atag_" + Id + "_1' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Success</a></li>";
                                optionHtml += "<li><a id='atag_" + Id + "_0' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Failed</a></li> ";
                                optionHtml += "<li ><a id='atag_" + Id + "_3' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>In Progress</a></li>";
                                optionHtml += "<li ><a id='atag_" + Id + "_4' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Reject</a></li>";
                                break;

                        }

                        htmlStr += "<tr><td>" + Id + "</td><td>" + UserID + "</td><td>" + Amount + "</td><td>" + BUId +
                            "</td><td>" + CusAcc + "</td><td>" + CusMob + "</td><td>" + DueDate + "</td><td>" + TransactionId +
                            "</td><td>" + ReqDate + "</td><td><textarea>" + Comments + "</textarea></td><td>" + statusText + "<td><div class='btn-group dropup'>" + statusHtml;
                        htmlStr += "<span class='caret'></span><span class='sr-only'>Toggle Dropdown</span>  </button> ";
                        htmlStr += "<ul id='selectionToggle' class='dropdown-menu' role='menu'>" + optionHtml;
                        //  htmlStr +=  "<li ><a id='atag_" + Id + "_1' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Success</a></li>";
                        //htmlStr += "<li><a id='atag_" + Id + "_0' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Failed</a></li>  <li ><a id='atag_" + Id + "_2' data-toggle='modal'  data-target='.status_model' onclick='setModelHiddenValu(this)'>Pending</a></li>";
                        //htmlStr += "<li ><a id='atag_" + Id + "_3' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>In Progress</a></li>";
                        //htmlStr += "<li ><a id='atag_" + Id + "_4' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Reject</a></li>";
                        //htmlStr += "<li ><a id='atag_" + Id + "_5' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Received</a></li>";
                        //htmlStr += "<li ><a id='atag_" + Id + "_6' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Others</a></li>";
                        //htmlStr += "<li ><a id='atag_" + Id + "_7' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Not Known</a></li>";
                        //htmlStr += "<li ><a id='atag_" + Id + "_8' data-toggle='modal' data-target='.status_model' onclick='setModelHiddenValu(this)'>Awaiting</a></li>";
                        htmlStr += "</ul></div></td><td></tr>";
                    }


                }
                else
                    htmlStr += "<tr><td  class='textcenter'>No Data</td><tr>";
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                error_text.ForeColor = System.Drawing.Color.Red;
                error_text.Text = "Enter Correct Data!";


            }
            return htmlStr;
        }



        protected void btnFilter_ServerClick(object sender, EventArgs e)
        {
            dateAmountAbstractor = 1;
            try
            {
                type = Int32.Parse(SeletionList.Value);
                switch (type)
                {
                    case 4:
                        value = inputtxtDate.Value.ToString();
                        break;
                    case 6:
                        value = inputtxtDate.Value.ToString();
                        break;
                    case 7:
                        value = statusList.Value;
                        break;
                    default:
                        value = inputControl.Text;
                        break;

                }
                table_data.InnerHtml = getElectricityFilterData(type, value);
            }
            catch (Exception ex) { Trace.Warn(ex.Message); }
        }

        protected void btnInsert_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string tran = inputTransactionToggleForm.Text;
                string comment = inputCommentToggleForm.Text;
                int tblId = Int32.Parse(hdnBtnId.Value);
                int tbstatus = Int32.Parse(hdbBtnLi.Value);
                Handler obj = new Handler();
                obj.AddTranCommentData(tblId, tran, comment, tbstatus);
                table_data.InnerHtml = getElectricityFilterData(0, "0");

            }
            catch (Exception ex) { Trace.Warn(ex.Message); }
        }
        protected void btnClose_ServerClick(object sender, EventArgs e)
        {


        }



    }
}