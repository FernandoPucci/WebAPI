using Microsoft.Web.Http;
using ResourcesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ResourcesServer.Controllers
{
    /// <summary>
    /// Base EmployeesController, inheritance from ApiController and implements Database Context fot Employww
    /// </summary>
    public partial class EmployeesBaseController : ApiController
    {

        #region Database Configurations
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
        #endregion

    }
}