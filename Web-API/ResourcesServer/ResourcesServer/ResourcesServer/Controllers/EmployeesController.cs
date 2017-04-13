using Microsoft.Web.Http;
using ResourcesServer.Models;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace ResourcesServer.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    //[Route("api/v{version:apiVersion}/[controller]")]
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
        public IQueryable<Employee> Get()
        {

            var identity = User.Identity as ClaimsIdentity;

            foreach (Claim c in identity.Claims) {
                Debug.WriteLine(c.Type + " >> " + c.Value);

            }

            return db.Employees.AsQueryable();

        }

        [HttpGet]
        [Route("{key}")]
        public Employee Get(int key)
        {
            IQueryable<Employee> result = db.Employees.Where(p => p.EmpNo == key);
            return result.FirstOrDefault();
        }


        //V2
        [HttpGet, MapToApiVersion("2.0")] //Map this method to next version in the same controller     
        [Route("{key}")]
        public SingleResult<Employee> GetV2(int key) //Rename the method
        {
            IQueryable<Employee> result = db.Employees.Where(p => p.EmpNo == key);
            return SingleResult.Create(result);
        }


        [HttpPost]
        [Route]
        public async Task<IHttpActionResult> Post(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Employees.Add(employee);
            await db.SaveChangesAsync();
            return Created("", employee);
        }

        //TODO:
        public void Put(int id, string value)
        {
        }
        
        public void Delete(int id)
        {
        }

    }
}