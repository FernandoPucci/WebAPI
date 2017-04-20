﻿using Microsoft.Web.Http;
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

namespace ResourcesServer.Controllers.Employees
{
    /// <summary>
    /// Employees Controller - Version 2.0
    /// </summary>
    [Authorize]
    [ApiVersion("2.0")]
    public partial class EmployeesController : EmployeesBaseController
    {

        #region  Verbs        
        /// <summary>
        /// Get a Employee that correspond to a Key - version 2.0 (Need Authentication) V2.0
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key">Identifier Parameter</param>
        /// <returns>Employee IQueryable Object</returns>
        [HttpGet] //Map this method to next version in the same controller     
        [Route("{key}"), MapToApiVersion("2.0")]
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

        #endregion

    }
}