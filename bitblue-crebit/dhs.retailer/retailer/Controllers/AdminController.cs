using api.dhs.Logging;
using api.dhs.Models.BL.Common;
using api.dhs.Models.DL.Common;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.dhs.Controllers
{
    public class AdminController : ApiController
    {
        [Route("dhs/adminUpdate")]
        [HttpGet]
        public HttpResponseMessage GetAdminUpdate(HttpRequestMessage req)
        {
            // Logger log = new Logger();
            BL_Admin admin = new BL_Admin();
            List<DL_AdminUpdate> adminUpdate = admin.GetAdminUpdate(); //
            if (admin._IsSuccess)
                return req.CreateResponse<List<DL_AdminUpdate>>(HttpStatusCode.OK, adminUpdate);
            else
                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
        }

        [Route("dhs/adminPopup")]
        [HttpGet]
        public HttpResponseMessage GetAdminPopUp(HttpRequestMessage req)
        {
            BL_Admin admin = new BL_Admin();
            List<DL_PopUpMessage> adminPopup = admin.GetPopUpMessage(); //
            if (admin._IsSuccess)
                return req.CreateResponse<List<DL_PopUpMessage>>(HttpStatusCode.OK, adminPopup);
            else
                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
        }

        [Route("dhs/adminMonthly")]
        [HttpGet]
        public HttpResponseMessage GetCrebitMonthlyCharges(HttpRequestMessage req)
        {
            BL_Admin admin = new BL_Admin();
            List<DL_MonthlyRetailerCharges> crebitCharge = admin.UpdateCrebitMonthlyCharges(); //
            if (admin._IsSuccess)
                return req.CreateResponse<List<DL_MonthlyRetailerCharges>>(HttpStatusCode.OK, crebitCharge);
            else
            {
                Logger.WriteLog(LogLevelL4N.WARN, "Server Error");
                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
            }
        }
    }
}
