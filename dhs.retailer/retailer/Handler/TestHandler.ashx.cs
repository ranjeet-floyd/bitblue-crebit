using System.Web;

namespace retailer.Handler
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class TestHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}