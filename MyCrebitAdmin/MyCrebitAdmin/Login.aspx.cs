using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Security.Cryptography;


namespace CrebitAdminPanelNew
{
    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {         
        }        
             
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
            Handler obj = new Handler();
            int Id = obj.Checker(UserId.Text, Password.Text);
            if (Id != 0)
            {
                try
                {
                    HttpCookie cookies = new HttpCookie("UserInfo");
                    byte[] pwdBytes = System.Text.Encoding.ASCII.GetBytes(Password.Text);
                    string password = Convert.ToBase64String(pwdBytes);
                    cookies.Value = UserId.Text + "|" + password;
                    cookies.Expires = DateTime.Now.AddHours(1);
                    Response.Cookies.Add(cookies);
                    Random rand = new Random(9999);

                    Label3.Text = "Welcome";
                    Response.Redirect("Electricity_page.aspx?u=" + UserId.Text + "|" + password + "&uniq=" + rand.Next(9999, 99999).ToString());
                }
                catch (Exception ex){ };
    
            }
            else
            {
                UserId.Text = "";
                Password.Text = "";
                Label2.Text = "";
                Label3.ForeColor = System.Drawing.Color.Red;
                Label3.Text = "Please Enter Correct Username and Pasword !";

            }
        } catch (Exception ex)
            {
                Label3.Text = "Login_Status";
                Label2.ForeColor = System.Drawing.Color.Red;
                Label2.Text = "Please Enter Correct Data !";
            }
        
        }

    }
}