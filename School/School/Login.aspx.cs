using School.src.model;
using System;
using System.Web;

namespace School
{
    public partial class Login : System.Web.UI.Page
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["_mteresa"] != null)
                Request.Cookies["_mteresa"].Expires = indianTime.AddHours(-1);
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Validation val = new Validation();

            try
            {
                if (!string.IsNullOrEmpty(txtUserName.Value) && !string.IsNullOrEmpty(txtPassword.Value))
                {
                    int valid = val.ValidateUser(txtUserName.Value, txtPassword.Value);
                    if (valid == 1)//valid
                    {

                        //   HttpCookie _mteresa = new HttpCookie("_mteresa");
                        Response.Cookies["_mteresa"]["UserKey"] = Validation.Base64Encode(txtUserName.Value);
                        Response.Cookies["_mteresa"]["Key"] = Validation.Base64Encode(txtPassword.Value);
                        Response.Cookies["_mteresa"].Expires = indianTime.AddHours(1);
                        Response.RedirectPermanent("Attendance.aspx", false);

                    }
                    else
                    {
                        if (valid == 2)
                            lblMesssage.InnerText = "Wrong User Name Or Password";
                        else
                            lblMesssage.InnerText = "User Not Exists";
                    }

                }
                else
                    lblMesssage.InnerText = "Please enter User Name & Password";
            }
            catch (Exception ex) { Trace.Warn(ex.Message); }
        }
    }
}