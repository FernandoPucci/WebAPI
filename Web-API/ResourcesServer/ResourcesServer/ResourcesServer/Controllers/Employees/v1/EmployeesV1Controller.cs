﻿using Microsoft.Web.Http;
using ResourcesServer.Helpers;
using ResourcesServer.Models;
using System;
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
    /// Employees Controller - Version 1.0
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/Employees")]
    public partial class EmployeesController : EmployeesBaseController
    {

        #region Default Verbs

        /// <summary>
        /// Get a list of Employees (Need Authentication) (Only for ADMINISTRATOR roles) V1.0
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list o Empoyees</returns>
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

        /// <summary>
        /// Get a Employee that correspond to a Key (Need Authentication) V1.0
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key">Identifier Parameter</param>
        /// <returns>Employee Object</returns>
        [HttpGet]
        [Route("{key}"), MapToApiVersion("1.0")]
        [ResponseType(typeof(Employee))]
        public HttpResponseMessage GetV1(HttpRequestMessage request, int key)
        {
            IQueryable<Employee> result = db.Employees.Where(p => p.EmpNo == key);

            if (result == null || result.Count() == 0)
            {
                return request.CreateResponse(HttpStatusCode.NoContent);
            }

            return request.CreateResponse(HttpStatusCode.OK, result.FirstOrDefault());
        }

        /// <summary>
        /// Save a new Employee or Update an existent V1.0
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
        #endregion

        #region Verbs not implementad yet

        //TODO:
        /// <summary>
        /// Update an attribute of an Employee V1.0
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
        /// Remove a Employee from Database V1.0
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
        /// Update/Overwrite a Employee V1.0
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

        #endregion

    }
}