using System.Web.Http;
using WebActivatorEx;
using ResourcesServer;
using Swashbuckle.Application;
using System;
using System.Xml.XPath;
using ResourcesServer.Filters;
using ResourcesServer.Helpers;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace ResourcesServer
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;




            GlobalConfiguration.Configuration                
                .EnableSwagger("docs/{apiVersion}/swagger", c =>
                    {
                        // By default, the service root url is inferred from the request used to access the docs.
                        // However, there may be situations (e.g. proxy and load-balanced environments) where this does not
                        // resolve correctly. You can workaround this by providing your own code to determine the root URL.
                        //
                        //  c.RootUrl(req => GetRootUrl());

                        // If schemes are not explicitly provided in a Swagger 2.0 document, then the scheme used to access
                        // the docs is taken as the default. If your API supports multiple schemes and you want to be explicit
                        // about them, you can use the "Schemes" option as shown below.
                        //
                        //c.Schemes(new[] { "http", "https" });

                        // Use "SingleApiVersion" to describe a single version API. Swagger 2.0 includes an "Info" object to
                        // hold additional metadata for an API. Version and title are required but you can also provide
                        // additional fields by chaining methods off SingleApiVersion.
                        //
                        c.SingleApiVersion("v1", "Resources Server");
                        c.OperationFilter<MultipleOperationsWithSameVerbFilter>();
                        c.IncludeXmlComments(GetXmlCommentsPath());
                        // If your API has multiple versions, use "MultipleApiVersions" instead of "SingleApiVersion".
                        // In this case, you must provide a lambda that tells Swashbuckle which actions should be
                        // included in the docs for a given API version. Like "SingleApiVersion", each call to "Version"
                        // returns an "Info" builder so you can provide additional metadata per API version.
                        //
                        //c.MultipleApiVersions(
                        //    (apiDesc, targetApiVersion) => ResolveVersionSupportByRouteConstraint(apiDesc, targetApiVersion),
                        //    (vc) =>
                        //    {
                        //        vc.Version("v2", "Resources Server API V2");
                        //        vc.Version("v1", "Resources Server API V1");
                        //    });


                        //c.OAuth2("oauth2")
                        //    .Description("Oauth2 implicit grant")
                        //    .Flow("implicit")
                        //    .AuthorizationUrl(AuthenticationServerEnvironmentHelper.GetAuthenticationURI(null))
                        //    //.tokenurl("https://tempuri.org/token")
                        //    .Scopes(scopes =>
                        //    {
                        //        scopes.Add("UsuarioSwagger", "Granted User to Swagger Documentation");
                        //        //scopes.add("write", "write access to protected resources");
                        //    });



                        //c.OperationFilter<AssignOAuth2SecurityRequirements>();
                    })
                .EnableSwaggerUi("documentation/ui/{*assetPath}",c =>
                    {
                        
                        // If your API supports the OAuth2 Implicit flow, and you've described it correctly, according to
                        // the Swagger 2.0 specification, you can enable UI support as shown below.
                        //
                        c.EnableOAuth2Support("UsuarioSwagger", "samplerealm", "Swagger UI");
                        //c.EnableOAuth2Support(
                        //    clientId: "test-client-id",
                        //    clientSecret: null,
                        //    realm: "test-realm",
                        //    appName: "Swagger UI"
                        //    //additionalQueryStringParams: new Dictionary<string, string>() { { "foo", "bar" } }
                        //);
                        
                    });
        }

        private static string GetRootUrl()
        {
            var request = System.Web.HttpContext.Current.Request;
            var rootURL = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "help/doc";
            return rootURL;

        }

        protected static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\Swagger.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
