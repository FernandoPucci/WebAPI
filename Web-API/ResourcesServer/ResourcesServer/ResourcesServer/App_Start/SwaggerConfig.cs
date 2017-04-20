using System.Web.Http;
using WebActivatorEx;
using Swashbuckle.Application;
using Microsoft.Web.Http;
using Swashbuckle.Swagger;
using System.Linq;
using ResourcesServer;
using System.Web.Http.Description;
using ResourcesServer.Filters;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace ResourcesServer
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                 .EnableSwagger("docs/{apiVersion}/help",c =>
                    {
                        //OLD IMPLEMENTATIONS
                        c.OperationFilter<MultipleOperationsWithSameVerbFilter>();
                        c.GroupActionsBy(GroupByControllerName);
                        c.IncludeXmlComments(GetXmlCommentsPath());
                        c.OperationFilter<RemoveControllerNameInOperationIdFilter>();

                        //////
                        c.MultipleApiVersions(
                            (apiDesc, version) =>
                            {
                                // get the versions specified by the controller
                                var controllerVersions = apiDesc.GetControllerAndActionAttributes<ApiVersionAttribute>()
                                    .SelectMany(attr => attr.Versions); ;
                                // get the versions specified by the action. these take precedence
                                var actionVersions = apiDesc.GetControllerAndActionAttributes<MapToApiVersionAttribute>()
                                    .SelectMany(attr => attr.Versions);

                                version = version.Replace("_", ".");

                                // if there are any action versions that match the current swagger version, use them
                                if (actionVersions.Any())
                                {
                                    return actionVersions.Any(v => $"v{v.ToString()}" == version);
                                }

                                // else use any controller versions that match
                                return controllerVersions.Any(v => $"v{v.ToString()}" == version);
                            },
                            (vc) =>
                            {
                                vc.Version("v1.0", "API v1.0");
                                vc.Version("v2.0", "API v2.0");
                            });
                        c.DocumentFilter<VersionHeaderDocumentFilter>();
                    })
                .EnableSwaggerUi("documentation/ui/{*assetPath}", c =>
                    {

                        //OLD IMPLEMENTATIONS
                        c.DocExpansion(DocExpansion.List);
                        //////

                        c.EnableDiscoveryUrlSelector();
                    });
        }

        protected static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\Swagger.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }

        private static string GroupByControllerName(ApiDescription desc)
        {
            return desc.ActionDescriptor.ControllerDescriptor.ControllerName.Contains("_") ? desc.ActionDescriptor.ControllerDescriptor.ControllerName.Split('_').First() : desc.ActionDescriptor.ControllerDescriptor.ControllerName;
        }
    }
    public class VersionHeaderDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            // get the version that this swagger doc represents
            var currentVersion = swaggerDoc.info.version.Replace("v", "").Replace("_", ".");
            // iterate through all the paths in the swagger doc
            foreach (var path in swaggerDoc.paths.Values)
            {
                // for every action on that path, add the version to the media type
                UpdateOperationVersionInfo(apiExplorer, currentVersion, path.get);
                UpdateOperationVersionInfo(apiExplorer, currentVersion, path.post);
                UpdateOperationVersionInfo(apiExplorer, currentVersion, path.head);
                UpdateOperationVersionInfo(apiExplorer, currentVersion, path.options);
                UpdateOperationVersionInfo(apiExplorer, currentVersion, path.put);
                UpdateOperationVersionInfo(apiExplorer, currentVersion, path.delete);
                UpdateOperationVersionInfo(apiExplorer, currentVersion, path.patch);
            }
        }

        private static void UpdateOperationVersionInfo(IApiExplorer apiExplorer,
            string currentVersion,
            Operation operation)
        {
            if (operation != null)
            {
                var currentVersionMediaType = $"v={currentVersion}";
                var apiDesc = apiExplorer.ApiDescriptions
                    .FirstOrDefault(a => operation.operationId.StartsWith($"{a.ActionDescriptor.ControllerDescriptor.ControllerName}_{a.ActionDescriptor.ActionName}"));
                var version = apiDesc?.GetControllerAndActionAttributes<ApiVersionAttribute>()
                    .FirstOrDefault(attr => attr.Versions.Select(v => v.ToString()).Contains(currentVersion));
                operation.deprecated = version?.Deprecated ?? false;
                operation.produces = operation.produces.Select(p => $"{p};{currentVersionMediaType}").ToList();
            }
        }
    }
    public class RemoveControllerNameInOperationIdFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (!string.IsNullOrEmpty(operation.operationId))
            {
                operation.operationId = operation.operationId.Split('_').Last();
            }
        }
    }
}
