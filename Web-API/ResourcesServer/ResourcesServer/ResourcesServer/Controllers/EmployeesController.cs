using Microsoft.Web.Http;
using ResourcesServer.Helpers;
using ResourcesServer.Models;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ResourcesServer.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [RoutePrefix("api/v{version:apiVersion}/Employees")]
    public class EmployeesController : ApiController
    {
        EmployeesContext db = new EmployeesContext();
        private bool EmployeesExists(int key)
        {
            return db.Employees.Any(p => p.EmpNo == key);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [HttpGet]
        [Route]
        [ResponseType(typeof(IQueryable<Employee>))]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {

            var identity = User.Identity as ClaimsIdentity;

            if (!AuthenticationHelper.GetAdministratorPermission(identity))
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Necessária Permissão de Administrator.");
            };

            return request.CreateResponse(HttpStatusCode.OK, db.Employees.AsQueryable());

        }

        [HttpGet]
        [Route("{key}")]
        [ResponseType(typeof(Employee))]
        public HttpResponseMessage Get(HttpRequestMessage request, int key)
        {
            IQueryable<Employee> result = db.Employees.Where(p => p.EmpNo == key);

            if (result == null || result.Count() == 0) {
                return request.CreateResponse(HttpStatusCode.NoContent);
            }

            return request.CreateResponse(HttpStatusCode.OK, result.FirstOrDefault());
        }


        //V2
        [HttpGet, MapToApiVersion("2.0")] //Map this method to next version in the same controller     
        [Route("{key}")]
        [ResponseType(typeof(SingleResult<Employee>))]
        public HttpResponseMessage GetV2(HttpRequestMessage request, int key) //Rename the method
        {
            IQueryable<Employee> result = db.Employees.Where(p => p.EmpNo == key);

            if (result == null || result.Count() == 0)
            {
                return request.CreateResponse(HttpStatusCode.NoContent);
            }

            return request.CreateResponse(HttpStatusCode.OK, SingleResult.Create(result)); //difference between both methods,
                                                                                           //only is the type of return 
                                                                                           //(Employee Object @v1 vs. IQeryable @v2)
        }
        
        [HttpPost]
        [Route]
        public async Task<IHttpActionResult> Post(Employee employee)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = User.Identity as ClaimsIdentity;

            //ahother way, throwing an Exception
            if (!AuthenticationHelper.GetAdministratorPermission(identity))
            {
                //using an Exception Text
                //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Necessária Permissão de Administrator." });
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            };

            db.Employees.Add(employee);
            await db.SaveChangesAsync();

            return Created("", employee);
        }

        //TODO:
        [HttpPut]
        [Route("{key}/{value}")]
        public HttpResponseMessage Put(HttpRequestMessage request, int key, string value)
        {
            return request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }

        [HttpDelete]
        [Route("{key}")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int key)
        {
            return request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }

        [HttpPatch]
        [Route("{key}")]
        public HttpResponseMessage Patch(HttpRequestMessage request, int key)
        {
            return request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }

      

    }
}