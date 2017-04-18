using Microsoft.Web.Http;
using ResourcesServer.Helpers;
using ResourcesServer.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

namespace ResourcesServer.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("3.0")]
    [RoutePrefix("api/v{version:apiVersion}/Employees")]
    public class EmployeesV3Controller : EmployeesController
    {

        [HttpGet, MapToApiVersion("3.0")] 
        [Route("{key}")]
        public  HttpResponseMessage Get(HttpRequestMessage request, int key) 
        {

            IQueryable<Employee> result = db.Employees.Where(p => p.EmpNo == key);

            var identity = User.Identity as ClaimsIdentity;

            
            if (!AuthenticationHelper.GetAdministratorPermission(identity))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Usuário não autorizado: [" + identity.FindFirst("userName").Value + "]." });
            };

            if (result == null || result.Count() == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NoContent) { ReasonPhrase = "Nenhum Employee cadastrado para o Id: ["  + key + "]."});
            }

            return request.CreateResponse(HttpStatusCode.OK, SingleResult.Create(result)); 
        }

        /// <summary>
        /// Override the Get from V1
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route]
        //[ResponseType(typeof(IQueryable<Employee>))]
        //public HttpResponseMessage Get(HttpRequestMessage request)
        //{
        //    return base.Get(request);

        //}
    }
}