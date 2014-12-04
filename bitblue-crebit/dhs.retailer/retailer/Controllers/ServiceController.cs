using com.dhs.webapi.Model.Common;
using crebit.retailer.Models.BL.Service;
using crebit.retailer.Models.DL.Service;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace crebit.retailer.Controllers
{
    public class ServiceController : ApiController
    {

        [Route("dhs/torrentPower")]
        [HttpPost]
        public HttpResponseMessage GettorrentPowerCusDetails(HttpRequestMessage req, DL_TorrentPower dL_TorrentPower)
        {
            //bool _IsSuccess;
            if (dL_TorrentPower != null && dL_TorrentPower.Bu > 0 && !string.IsNullOrEmpty(dL_TorrentPower.CusAcc)
                && !string.IsNullOrEmpty(dL_TorrentPower.Key) && !string.IsNullOrEmpty(dL_TorrentPower.UserId))
            {
                User user = new User() { Password = dL_TorrentPower.Key,  UserId = dL_TorrentPower.UserId };
                Validation.UserCheck(user);
                if (Validation._IsSuccess)
                {
                    BL_Service bL_Service = new BL_Service();
                    DL_TorrentPowerReturn dL_TorrentPowerReturn = bL_Service.GetTorrentPowerDetails(dL_TorrentPower);
                    if (bL_Service._IsSuccess)
                        return req.CreateResponse<DL_TorrentPowerReturn>(HttpStatusCode.OK, dL_TorrentPowerReturn);
                    else
                        return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "ServerError");
                }
                return req.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }
            return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
        }
    }
}