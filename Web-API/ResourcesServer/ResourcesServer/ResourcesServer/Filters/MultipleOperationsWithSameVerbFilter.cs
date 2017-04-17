using Swashbuckle.Swagger;
using System.Web.Http.Description;

namespace ResourcesServer.Filters
{
    /// <summary>
    /// Filter to set same names (verbs) at API 
    /// Available documentation in https://docs.microsoft.com/pt-br/azure/app-service-api/app-service-api-dotnet-swashbuckle-customize
    /// </summary>
    public class MultipleOperationsWithSameVerbFilter : IOperationFilter
    {
        public void Apply(
            Operation operation,
            SchemaRegistry schemaRegistry,
            ApiDescription apiDescription)
        {
            if (operation.parameters != null)
            {
                operation.operationId += "By";
                foreach (var parm in operation.parameters)
                {
                    operation.operationId += string.Format("{0}", parm.name);
                }
            }
        }
    }
}