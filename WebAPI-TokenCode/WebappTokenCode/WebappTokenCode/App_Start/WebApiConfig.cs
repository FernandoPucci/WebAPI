using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.OData.Builder;
using WebappTokenCode.Models;
using System.Web.OData.Extensions;

namespace WebappTokenCode
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Employee>("Employees");
            builder.EntityType<Employee>().Filter("Salary").Filter("DeptName").Expand().Select().OrderBy().Expand().Count();  //permissao de query por entidade
            config.MapODataServiceRoute(
                routeName: "ODataRoute",                
                routePrefix: "v1/odata",
                model: builder.GetEdmModel()
                );
        }
    }
}
