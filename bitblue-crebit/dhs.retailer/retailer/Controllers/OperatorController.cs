using com.dhs.webapi.Model.Common;
using com.dhs.webapi.Models.DL.Common;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApplication1.Models.BL.Common;

namespace api.dhs.Controllers
{
    public class OperatorController : ApiController
    {
        [Route("dhs/operators")]
        [HttpGet]
        public HttpResponseMessage GetOperators(HttpRequestMessage req)
        {
            BL_Operator blopr = new BL_Operator();
            List<DL_OperatorReturn> oprReturn = blopr.GetOperators(); //Validate Login
            if (blopr._IsSuccess)
                return req.CreateResponse<List<DL_OperatorReturn>>(HttpStatusCode.OK, oprReturn);
            else
                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
        }

        [Route("dhs/operatorMargin")]
        [HttpGet]
        public HttpResponseMessage GetOperatorMargin(HttpRequestMessage req)
        {
            BL_Operator blopr = new BL_Operator();
            List<DL_OperatorMarginReturn> oprReturn = blopr.GetOperatorMargin(); //
            if (blopr._IsSuccess)
                return req.CreateResponse<List<DL_OperatorMarginReturn>>(HttpStatusCode.OK, oprReturn);
            else
                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
        }

        //Modified: Ranjeet | 26-Dec-14 || Changed to HttpGet and Fixed session issue.
        [EnableCors(origins: "*", headers: "Content-Type: application/json; charset=utf-8", methods: "*")]
        [Route("dhs/cyberTransStatus/{TransactionId}")]
        [HttpGet]
        public HttpResponseMessage CheckCyberPlateTransStatus(HttpRequestMessage req, [FromUri] DL_SessionCyberPlateStatus dL_SessionCyberPlateStatus)
        {
            BL_Operator cyberPlateStatus = new BL_Operator();
            DL_SessionCyberPlateStatusReturn cyberPlateStatusReturn = cyberPlateStatus.GetCyberPlateStatus(dL_SessionCyberPlateStatus); //Validate Login
            if (cyberPlateStatus._IsSuccess)
                return req.CreateResponse<DL_SessionCyberPlateStatusReturn>(HttpStatusCode.OK, cyberPlateStatusReturn);
            else
                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
        }

        [Route("dhs/getMSEBCusDetails")]
        [HttpPost]
        public HttpResponseMessage GetMSEBCusDetails(HttpRequestMessage req, MSEBUserDetailsReq mSEBUserDetailsReq)
        {
            if (mSEBUserDetailsReq != null && mSEBUserDetailsReq.BuCode > 1 && !string.IsNullOrEmpty(mSEBUserDetailsReq.ConsumerNo)
                && !string.IsNullOrEmpty(mSEBUserDetailsReq.Key) && !string.IsNullOrEmpty(mSEBUserDetailsReq.UserId))
            {
                User user = new User() { Password = mSEBUserDetailsReq.Key, UserId = mSEBUserDetailsReq.UserId };
                Validation.UserCheck(user);
                if (Validation._IsSuccess)
                {
                    BL_Operator op = new BL_Operator();
                    MSEBUserDetailsReqReturn mSEBUserDetailsReqReturn = op.GetMSEBUserStatus(mSEBUserDetailsReq);
                    if (op._IsSuccess)
                        return req.CreateResponse<MSEBUserDetailsReqReturn>(HttpStatusCode.OK, mSEBUserDetailsReqReturn);
                    else
                        return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                }
                return req.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }
            return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
        }

        [Route("dashboard/deleteRegisteredAccount")]
        [HttpPost]
        public HttpResponseMessage DeleteRegisteredAccount(HttpRequestMessage req, DeleteRegAccount deleteRegAccount)
        {
            User user = new User() { Password = deleteRegAccount.Key, UserId = deleteRegAccount.UserId };
            Validation.UserCheck(user);
            if (Validation._IsSuccess)
            {
                BL_Operator op = new BL_Operator();
                DeleteRegAccountReturn deleteRegAccountReturn = op.DeleteRegAccount(deleteRegAccount);
                if (op._IsSuccess)
                    return req.CreateResponse<DeleteRegAccountReturn>(HttpStatusCode.OK, deleteRegAccountReturn);
                else
                    return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
            }
            return req.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
        }
    }
}