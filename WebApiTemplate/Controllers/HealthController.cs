using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using ASI.Services.Monitoring;

namespace WebApiTemplate.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/health")]
    public class HealthController : ApiController
    {
        [Route(""), AllowAnonymous]
        public IHttpActionResult Get()
        {
            return Ok(HealthChecks.GetStatus());
        }
    }
}
