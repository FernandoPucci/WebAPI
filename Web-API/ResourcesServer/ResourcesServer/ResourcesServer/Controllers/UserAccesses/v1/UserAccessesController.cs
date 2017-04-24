using Microsoft.Web.Http;
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
        /// Get a list of All UserAccesses (Need Authentication) (Only for ADMINISTRATOR roles) V1.0
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list o Accesses</returns>
        [HttpGet]
        [Route]
        [ResponseType(typeof(IQueryable<Models.UserAccess>))]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {

            var identity = User.Identity as ClaimsIdentity;

            if (!AuthenticationHelper.GetAdministratorPermission(identity, "GET " + CONTROLLER_V1))
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Necessária Permissão de Administrator.");
            };

            return request.CreateResponse(HttpStatusCode.OK, db.UserAccesses.AsQueryable().OrderByDescending(g => g.AccessDate));


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