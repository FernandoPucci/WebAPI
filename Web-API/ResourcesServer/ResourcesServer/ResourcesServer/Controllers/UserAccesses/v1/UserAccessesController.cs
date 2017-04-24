﻿using Microsoft.Web.Http;
using ResourcesServer.Helpers;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ResourcesServer.Controllers.UserAccess
{
    /// <summary>
    /// UserAccessesController v1.0
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/UserAccesses")]
    public class UserAccessesController : UserAccessesBaseController
    {

        #region Constants
        private const string CONTROLLER_V1 = "UserAccesses V1";
        #endregion

        #region Default Verbs

        /// <summary>
        /// <summary> Get a list of All UserAccesses (Need Authentication) (Only for ADMINISTRATOR roles) V1.0
        /// </summary>
        /// <param name="request"></param>
        /// <param name="_offset">Initial offset</param>
        /// <param name="_limit">Maximum results per page</param>
        /// <returns></returns>
        [HttpGet]
        [Route]
        [ResponseType(typeof(IQueryable<Models.UserAccess>))]
        public HttpResponseMessage Get(HttpRequestMessage request, int _offset = 0, int _limit = 5)
        {

            var identity = User.Identity as ClaimsIdentity;

            if (!AuthenticationHelper.GetAdministratorPermission(identity, "GET " + CONTROLLER_V1 + ((_offset != 0 || _limit != 5) ? " Called W/ Pagination: " + _offset + " to " + _limit : "")))
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Necessária Permissão de Administrator.");
            };

            var total = db.UserAccesses.Count();
            // var userAccesses = db.UserAccesses.AsQueryable().OrderByDescending(g => g.AccessDate);
            var userAccesses = db.UserAccesses
                .OrderByDescending(g => g.AccessDate)
                .Skip(_offset)
                .Take(_limit)
                .AsQueryable();

            return request.CreateResponse(HttpStatusCode.OK,
                new
                {
                    Data = userAccesses,
                    Paging = new
                    {
                        Total = total,
                        Limit = _limit,
                        Offset = _offset,
                        Returned = userAccesses.Count()

                    }
                }
                );


        }
        #endregion

        public async Task LogAccess(Models.UserAccess user)
        {
            //log access
            db.UserAccesses.Add(user);
            db.SaveChanges();
        }
    }
}