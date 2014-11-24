using api.dhs.Logging;
using com.dhs.webapi.Model.BL.User;
using com.dhs.webapi.Model.BL_User;
using com.dhs.webapi.Model.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models.BL.User;
using WebApplication1.Models.DL.User;

namespace api.dhs.Controllers
{
    public class UserController : ApiController
    {
        //Login
        [Route("dhs/login")]
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage req, DL_Login login)
        {
            if (!String.IsNullOrEmpty(login.Mobile) && !String.IsNullOrEmpty(login.Pass))
            {
                BL_Login blLogin = new BL_Login();
                List<DL_LoginReturn> dlLoginRet = blLogin.CheckLogin(login); //Validate Login
                if (blLogin._IsSuccess)
                    return req.CreateResponse<List<DL_LoginReturn>>(HttpStatusCode.OK, dlLoginRet);
                else
                    return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
            }

            return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");

        }

        //Sign Up
        [Route("dhs/signUp")]
        [HttpPost]
        public HttpResponseMessage SignUp(HttpRequestMessage req, DL_SignUp signUp)
        {
            if (signUp != null && !String.IsNullOrEmpty(signUp.Mobile) && !String.IsNullOrEmpty(signUp.Pass) && signUp.UserType > 0)
            {
                BL_SignUp blsignUp = new BL_SignUp();
                List<DL_SignUpReturn> signUpReturn = blsignUp.SignUp(signUp); //

                if (blsignUp._IsSuccess)
                    return req.CreateResponse<List<DL_SignUpReturn>>(HttpStatusCode.Created, signUpReturn);

                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
            }

            return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");

        }

        //Change Password
        [Route("dhs/changePassword")]
        [HttpPost]
        public HttpResponseMessage ChangePassword(HttpRequestMessage req, DL_ChangePassword changePas)
        {
            //Logger.WriteLog(LogLevelL4N.INFO, "Called ChangePassword api");
            if (changePas != null && !String.IsNullOrEmpty(changePas.UserId) && !String.IsNullOrEmpty(changePas.Key)
                 && !String.IsNullOrEmpty(changePas.NPass) && !String.IsNullOrEmpty(changePas.OPass))
            {
                BL_Password blchangepass = new BL_Password();
                DL_ChangePasswordReturn changePasswordReturn = blchangepass.ChangePassword(changePas);

                if (blchangepass._IsSuccess)
                    return req.CreateResponse<DL_ChangePasswordReturn>(HttpStatusCode.OK, changePasswordReturn);
                else
                    return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
            }
            else
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
        }

        //Forgot Password
        [Route("dhs/forgotpassword")]
        [HttpPost]
        public HttpResponseMessage ForgotPassword(HttpRequestMessage req, DL_ForgotPassword forgotPas)
        {
            // Logger.WriteLog(LogLevelL4N.INFO, "Called ForgotPassword api");
            if (forgotPas != null && !String.IsNullOrEmpty(forgotPas.Mobile))
            {
                BL_Password blchangepass = new BL_Password();
                DL_ForgotPasswordReturn forgotPasswordReturn = blchangepass.ForgotPassword(forgotPas);

                if (blchangepass._IsSuccess)
                    return req.CreateResponse<DL_ForgotPasswordReturn>(HttpStatusCode.OK, forgotPasswordReturn);

                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
            }

            return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");


        }

        //Get Bank Details
        [Route("dashboard/getRegistratedAccount")]
        [HttpPost]
        public HttpResponseMessage GetBankDetails(HttpRequestMessage req, User user)
        {
            // Logger.WriteLog(LogLevelL4N.INFO, "Called ForgotPassword api");
            if (user != null && !String.IsNullOrEmpty(user.UserId) && !String.IsNullOrEmpty(user.Password))
            {
                Validation.UserCheck(user);
                if (Validation._IsSuccess)
                {
                    BL_Login account = new BL_Login();
                    List<DL_BankDetailsReturn> accountDetailsReturn = account.GetBankDetails(user);

                    if (account._IsSuccess)
                        return req.CreateResponse<List<DL_BankDetailsReturn>>(HttpStatusCode.OK, accountDetailsReturn);

                    return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                }
                return req.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");


        }


    }
}