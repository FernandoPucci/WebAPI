using Microsoft.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ResourcesServer.Controllers.Health
{
    //[Authorize]
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/Health")]
    public class HealthController : ApiController

    {
        [HttpGet]
        [Route]
        [ResponseType(typeof(string))]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            

            return request.CreateResponse(HttpStatusCode.OK, "Health check OK!");

        }
    }
}