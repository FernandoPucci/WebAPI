using Microsoft.Web.Http;
using ResourcesServer.Controllers.UserAccess;
using ResourcesServer.Helpers;
using ResourcesServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace ResourcesServer.Controllers.UserAccess
{
    /// <summary>
    /// UserAccessesController v2.0
    /// </summary>
    [Authorize]
    [ApiVersion("2.0")]
    public partial class UserAccessesController : UserAccessesBaseController
    {
        #region Default Verbs
        /// <summary>
        /// UserAccesses returned with Paged Results (Need Authentication) (Only for ADMINISTRATOR roles) V2.0
        /// </summary>
        /// <param name="request">Request Data Paramater</param>
        /// <param name="_pageNo">Page</param>
        /// <param name="_pageSize">Registers Per Page</param>
        /// <returns>Paged Results of UserAccesses</returns>
        [HttpGet]
        [Route, MapToApiVersion("2.0")]
        [ResponseType(typeof(IQueryable<PagedResult<Models.UserAccess>>))]
        public HttpResponseMessage GetV2(HttpRequestMessage request, int _pageNo = 1, int _pageSize = 5)
        {
            #region Authorization Validation
            var identity = User.Identity as ClaimsIdentity;

            if (!AuthenticationHelper.GetAdministratorPermission(identity, "GET " + CONTROLLER_V2 + ((_pageNo != 1 || _pageSize != 5) ? " Called W/ Paged Results: " + _pageNo + " to " + _pageSize : "")))
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Necessária Permissão de Administrator.");
            };
            #endregion

            // Determine the number of records to skip
            int skip = (_pageNo - 1) * _pageSize;

            // Get total number of records
            int total = db.UserAccesses.Count();

            // Select the userAccesses based on paging parameters
            var userAccesses = db.UserAccesses
                .OrderByDescending(g => g.AccessDate)
                .Skip(skip)
                .Take(_pageSize)
                .AsQueryable();

            // Return the list of userAccesses
            return request.CreateResponse(HttpStatusCode.OK, new PagedResult<Models.UserAccess>(userAccesses, _pageNo, _pageSize, total));
        }
        #endregion
    }
}