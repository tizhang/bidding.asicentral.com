using System.Web.Http;
using System.Web.Http.Cors;
using ASI.Services.Monitoring;

namespace WebApiTemplate.Controllers.Diagnostics
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/diagnostics/health")]
    public class HealthController : ApiController
    {
        [Route(""), AllowAnonymous]
        public IHttpActionResult Get()
        {
            return Ok(HealthChecks.GetStatus());
        }
    }
}
