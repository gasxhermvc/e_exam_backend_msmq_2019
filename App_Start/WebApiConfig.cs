using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace e_exam_backend_msmq_2019
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {   
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
