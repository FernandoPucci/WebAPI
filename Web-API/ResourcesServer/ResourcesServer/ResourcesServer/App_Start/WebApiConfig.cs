
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Web.Http;
using System.Web.Http.Routing;
using Microsoft.Web.Http.Routing;
using Microsoft.Web.Http.Versioning;

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

                o.DefaultApiVersion = new ApiVersion(2, 0); //Major Version  / Minor version
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ApiVersionReader = new MediaTypeApiVersionReader();
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
