using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using WebappTokenCode.Models;

namespace WebappTokenCode.Controllers
{
    [Authorize]
    public class EmployeesController : ODataController
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
        [EnableQuery]
        //[EnableQuery(PageSize = 3])
        public IQueryable<Employee> Get()
        {

            return db.Employees.AsQueryable();

        }


        [EnableQuery]
        public SingleResult<Employee> Get([FromODataUri] int key)
        {
            IQueryable<Employee> result = db.Employees.Where(p => p.EmpNo == key);
            return SingleResult.Create(result);
        }
        
        public async Task<IHttpActionResult> Post(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Employees.Add(employee);
            await db.SaveChangesAsync();
            return Created(employee);
        }

        [EnableQuery]
        public IQueryable<Employee> Get([FromODataUri] string deptName)
        {
            IQueryable<Employee> result = db.Employees.Where(p => p.DeptName == deptName);
            return result;
        }

    }
}