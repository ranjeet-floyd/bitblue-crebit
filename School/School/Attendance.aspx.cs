using MySql.Data.MySqlClient;
using School.src.db;
using School.src.model;
using System;
using System.Data;
using System.Web;

namespace School
{
    public partial class Attendance : System.Web.UI.Page
    {
        public int pageNo { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie _mteresa = Request.Cookies["_mteresa"];
            try
            {
                if (!IsPostBack)
                {
                }
                if (_mteresa != null)
                {

                    Validation val = new Validation();
                    int valid = val.ValidateUser(Validation.Base64Decode(_mteresa["UserKey"]), Validation.Base64Decode(_mteresa["Key"]));
                    if (valid == 1)//valid
                    {

                    }
                    else
                        Response.RedirectPermanent("/Login.aspx", false);
                }
                else
                    Response.RedirectPermanent("/Login.aspx", false);
            }
            catch (Exception ex) { Trace.Warn(ex.Message); }
        }

        private void GetDropDownValues()
        {
            string qtext = "";
            MySqlParameter[] mySqlParameter = new MySqlParameter[1];
            mySqlParameter[0] = new MySqlParameter("@Medium", txtGrNumber.Value);
            rptTableData.DataSource = (new DataBase()).GetDataSet(qtext, mySqlParameter);
            rptTableData.DataBind();

        }


        protected void btnSearchGr_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtGrNumber.Value))
                {
                    string qtext = "SELECT  Name ,  Std ,  Medium ,  Section ,  Gr_num , Enroll  FROM user_sch ";
                    qtext += " where Gr_num = @Gr_num";
                    MySqlParameter[] mySqlParameter = new MySqlParameter[1];
                    mySqlParameter[0] = new MySqlParameter("@Gr_num", txtGrNumber.Value);
                      DataSet ds = (new DataBase()).GetDataSet(qtext, mySqlParameter);
                      if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                      {
                          rptTableData.DataSource = ds;//(new DataBase()).GetDataSet(qtext, mySqlParameter);
                          rptTableData.DataBind();
                      }
                   
                }
            }
            catch (Exception ex) { Trace.Warn("btnSearchGr_Click : " + ex.Message); }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(drpMedium.Value) && !string.IsNullOrEmpty(hdnelectedDrpSection.Value) && !string.IsNullOrEmpty(hdnSelectedDrpStandard.Value))
                {
                    MarkAttFilter markAttFilter = new MarkAttFilter() { Medium = drpMedium.Value, Section = hdnelectedDrpSection.Value, Standard = hdnSelectedDrpStandard.Value, PN = pageNo };
                    if (!string.IsNullOrEmpty(markAttFilter.Medium) && !string.IsNullOrEmpty(markAttFilter.Standard) && !string.IsNullOrEmpty(markAttFilter.Section))
                    {
                        string qtext = " SELECT  Name ,  Std ,  Medium ,  Section ,  Gr_num , Enroll  FROM user_sch ";
                        qtext += " where Medium = @Medium AND Std =@Standard AND Section = @Section   Order by Name" ;
                        qtext += " ;SELECT  count(Gr_num) as 'count'  FROM user_sch ";
                        qtext += " where Medium = @Medium AND Std =@Standard AND Section = @Section";
                        MySqlParameter[] mySqlParameter = new MySqlParameter[3];
                        mySqlParameter[0] = new MySqlParameter("@Medium", markAttFilter.Medium);
                        mySqlParameter[1] = new MySqlParameter("@Section", markAttFilter.Section);
                        mySqlParameter[2] = new MySqlParameter("@Standard", markAttFilter.Standard);
                        DataSet ds = (new DataBase()).GetDataSet(qtext, mySqlParameter);
                        if (ds != null && ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)
                        {

                            // int count = Convert.ToInt32(ds.Tables[1].Rows[0]["count"]);

                            // int firstCount = (markAttFilter.PN * 20) < count ? (markAttFilter.PN) * 20 : count;

                            // spnShowCount.InnerText = "" + (markAttFilter.PN - 1) * 20 + " - " + firstCount + " of " + count.ToString();
                            rptTableData.DataSource = ds.Tables[0];
                            rptTableData.DataBind();
                            //string html = "<li id='li_prev' runat='server' ><a onclick='prevPage()'><i class='fa fa-chevron-left'></i></a></li>";
                            //for (int i = 1; i <= (count / 20); i++)
                            //{
                            //    if (i == pageNo)
                            //        html += "<li  class='active'><a  href=?pn=" + i + ">" + i + "</a></li>";
                            //    else
                            //        html += "<li><a  href=?pn=" + i+ " >" + i  + "</a></li>";
                            //}
                            //html += "<li ><a onclick='nextPage()'><i class='fa fa-chevron-right'></i></a></li>";

                            //paginglink.InnerHtml = html;
                        }
                        else
                        {
                            //tabl = "<tr><td><div class='alert alert-danger text-center' style='font-size:18px'>No Data</div></td></tr>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("btnApply_Click" + ex.Message);
            }
        }

    }
}