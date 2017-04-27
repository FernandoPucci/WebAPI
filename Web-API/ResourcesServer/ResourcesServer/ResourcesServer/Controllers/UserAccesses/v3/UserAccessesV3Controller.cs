using Microsoft.Web.Http;
using ResourcesServer.Controllers.UserAccess;
using ResourcesServer.Helpers;
using ResourcesServer.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http.Description;
using System.Web.Mvc;
namespace ResourcesServer.Controllers.UserAccess
{
    /// <summary>
    /// UserAccessesController v3.0 - Page data on Response Header 
    /// </summary>
    [Authorize]
    [ApiVersion("3.0")]
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
        [Route, MapToApiVersion("3.0")]
        [ResponseType(typeof(IQueryable<PagedResult<Models.UserAccess>>))]
        public HttpResponseMessage GetV3(HttpRequestMessage request, int _pageNo = 1, int _pageSize = 5)
        {
            #region Authorization Validation
            var identity = User.Identity as ClaimsIdentity;

            if (!AuthenticationHelper.GetAdministratorPermission(identity, "GET " + CONTROLLER_V3 + ((_pageNo != 1 || _pageSize != 5) ? " Called W/ Paged Results: " + _pageNo + " to " + _pageSize : "")))
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Necessária Permissão de Administrator.");
            };
            #endregion

            // Determine the number of records to skip
            int skip = (_pageNo - 1) * _pageSize;

            // Get total number of records
            int total = db.UserAccesses.Count();

            // Determine page count
            int pageCount = total > 0
                ? (int)Math.Ceiling(total / (double)_pageSize)
                : 0;

            // Select the userAccesses based on paging parameters
            var userAccesses = db.UserAccesses
                .OrderByDescending(g => g.AccessDate)
                .Skip(skip)
                .Take(_pageSize)
                .AsQueryable();

            // Return the list of userAccesses
            // Create the response
            var response = Request.CreateResponse(HttpStatusCode.OK, userAccesses);

            // Set headers for paging
            response.Headers.Add("X-Paging-PageNo", _pageNo.ToString());
            response.Headers.Add("X-Paging-PageSize", _pageSize.ToString());
            response.Headers.Add("X-Paging-PageCount", pageCount.ToString());
            response.Headers.Add("X-Paging-TotalRecordCount", total.ToString());

            response.Headers.Reverse();

            // Return the response
            return response;
        }
        #endregion
    }
}