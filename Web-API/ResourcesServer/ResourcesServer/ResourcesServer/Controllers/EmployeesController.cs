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
    public abstract class EmployeesController : ApiController
    {
        protected EmployeesContext db = new EmployeesContext();
        private bool EmployeesExists(int key)
        {
            return db.Employees.Any(p => p.EmpNo == key);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Get a list of Employees (Need Authentication) (Only for ADMINISTRATOR roles)
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list o Empoyees</returns>
        [HttpGet]
        [Route]
        [ResponseType(typeof(IQueryable<Employee>))]        
        public  HttpResponseMessage Get(HttpRequestMessage request)
        {

            var identity = User.Identity as ClaimsIdentity;

            if (!AuthenticationHelper.GetAdministratorPermission(identity))
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Necessária Permissão de Administrator.");
            };

            return request.CreateResponse(HttpStatusCode.OK, db.Employees.AsQueryable());

        }

        /// <summary>
        /// Get a Employee that correspond to a Key (Need Authentication) 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key">Identifier Parameter</param>
        /// <returns>Employee Object</returns>
        [HttpGet]
        [Route("{key}")]
        [ResponseType(typeof(Employee))]
        public  HttpResponseMessage Get(HttpRequestMessage request, int key)
        {
            IQueryable<Employee> result = db.Employees.Where(p => p.EmpNo == key);

            if (result == null || result.Count() == 0) {
                return request.CreateResponse(HttpStatusCode.NoContent);
            }

            return request.CreateResponse(HttpStatusCode.OK, result.FirstOrDefault());
        }


        /// <summary>
        /// Get a Employee that correspond to a Key - version 2.0 (Need Authentication) 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key">Identifier Parameter</param>
        /// <returns>Employee IQueryable Object</returns>
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

        /// <summary>
        /// Save a new Employee or Update an existent
        /// </summary>
        /// <param name="employee">JSON Object representative of an Employee</param>
        /// <returns></returns>
        /// <responsecode>200 - Ok</responsecode>
        /// <responsecode>400 - Bad Request</responsecode>
        /// <responsecode>401 - Unauthorized</responsecode>
        /// <responsecode>500 - Internal Server Error</responsecode>
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
        /// <summary>
        /// Update an attribute of an Employee
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key">Identifier Parameter</param>
        /// <param name="value">Attribute</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{key}/{value}")]
        public HttpResponseMessage Put(HttpRequestMessage request, int key, string value)
        {
            return request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }

        /// <summary>
        /// Remove a Employee from Database
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key">Identifier Parameter</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{key}")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int key)
        {
            return request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }


        /// <summary>
        /// Update/Overwrite a Employee
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key">Identifier Parameter</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{key}")]
        public HttpResponseMessage Patch(HttpRequestMessage request, int key)
        {
            return request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }

      

    }
}