using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Microsoft.Web.Http;
using Microsoft.Web.Http.Versioning;
using System.Web.Http.Routing;
using Microsoft.Web.Http.Routing;

namespace ResourcesServer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            //Add Version control system
            config.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0); //Major version, minor version
                o.ApiVersionSelector = new CurrentImplementationApiVersionSelector(o);

            });

            //Routes for versioning
            var constraintResolver = new DefaultInlineConstraintResolver()
            {
                ConstraintMap =
                                {
                                    ["apiVersion"] = typeof( ApiVersionRouteConstraint )
                                }
            };

            // Web API routes
            config.MapHttpAttributeRoutes(constraintResolver);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Swagger Register
            //SwaggerConfig.Register();
        }
    }
}
