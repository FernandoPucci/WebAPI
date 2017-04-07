using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebappTokenCode.Models;

namespace WebappTokenCode.Controllers
{
    [Authorize]
    public class EmployeeAPIController : ApiController
    {
        public List<Models.Employee> Get()
        {
            return new EmployeeDatabase();
        }
    }
}