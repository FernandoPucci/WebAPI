using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ResourcesServer
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "Employee",
            //    url: "api/v1/{controller}/{action}/{id}",
            //    defaults: new { controller = "Employee"}
            //);
            //routes.MapRoute(
            //    name: "EmployeeV2",
            //    url: "api/v2/{controller}/{action}/{id}",
            //    defaults: new { controller = "EmployeeV2" }
            //);
        }
    }
}
