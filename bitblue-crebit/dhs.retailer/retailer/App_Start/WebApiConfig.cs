using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace api.dhs
{
    // Web API configuration and services
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.EnableCors();
            // Web API routes
            // Attribute routing.
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                  routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional  }
            );
        }
    }
}
