using Microsoft.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ResourcesServer.Controllers.Health
{
    //[Authorize]
    /// <summary>
    /// Health Check Controller
    /// </summary>
    /// <param name="version">versao</param>
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/Health")]
    public class HealthController : ApiController

    {
        /// <summary>
        /// System Health Check
        /// </summary>
        /// <param name="request">Request Data Paramater</param>
        /// <param name="version">Your description here</param>
        /// <returns>Status of System Health Check</returns>
        [HttpGet]
        [Route]
        [ResponseType(typeof(string))]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            

            return request.CreateResponse(HttpStatusCode.OK, "Health check OK!");

        }
    }
}