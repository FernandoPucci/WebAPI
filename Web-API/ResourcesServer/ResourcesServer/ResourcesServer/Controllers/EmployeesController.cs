﻿using ResourcesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ResourcesServer.Controllers
{
    [Authorize]
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
      
        //[EnableQuery(PageSize = 3])
        public IQueryable<Employee> Get()
        {

            return db.Employees.AsQueryable();

        }


        [HttpGet]
        public SingleResult<Employee> Get(int key)
        {
            IQueryable<Employee> result = db.Employees.Where(p => p.EmpNo == key);
            return SingleResult.Create(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Employees.Add(employee);
            await db.SaveChangesAsync();
            return Created("",employee);
        }
      
    }
}