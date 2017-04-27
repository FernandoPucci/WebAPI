using Microsoft.Web.Http;
using ResourcesServer.Helpers;
using ResourcesServer.Models;
using System.Data.Entity.Core.Objects;
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
    public partial class UserAccessesController : UserAccessesBaseController
    {

        #region Constants

        private const string CONTROLLER_V1 = "UserAccesses V1";

        private const string CONTROLLER_V2 = "UserAccesses V2";

        #endregion

        #region Default Verbs

        /// <summary>
        /// <summary> Get a list of All UserAccesses (Need Authentication) (Only for ADMINISTRATOR roles) V1.0
        /// </summary>
        /// <param name="request">Request Data Paramater</param>
        /// <param name="_offset">Initial offset</param>
        /// <param name="_limit">Maximum results per page</param>
        /// <returns>List of UserAccesses, in descending order and with offset and limit</returns>
        [HttpGet]
        [Route, MapToApiVersion("1.0")]
        [ResponseType(typeof(IQueryable<Models.UserAccess>))]
        public HttpResponseMessage GetV1(HttpRequestMessage request, int _offset = 0, int _limit = 5)
        {
            #region Authorization Validation
            var identity = User.Identity as ClaimsIdentity;

            if (!AuthenticationHelper.GetAdministratorPermission(identity, "GET " + CONTROLLER_V1 + ((_offset != 0 || _limit != 5) ? " Called W/ Pagination: " + _offset + " to " + _limit : "")))
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Necessária Permissão de Administrator.");
            };
            #endregion


            var total = db.UserAccesses.Count();

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