using School.src.model;
using System;
using System.Web;

namespace Message.School
{
    public partial class Message : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie _mteresa = Request.Cookies["_mteresa"];
            try
            {
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

        protected void btnSendMsg_Click(object sender, EventArgs e)
        {
            try
            {
                string Mobiles = txtAreaMobile.InnerText.Trim(',');
                string Message = txtAreaMessage.InnerText.Trim();
                bool _isSend = SMS.SendSMS(Mobiles, Message);
            }
            catch
                (Exception ex) { Trace.Warn(ex.Message); }
        }


    }
}