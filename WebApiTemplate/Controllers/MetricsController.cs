using ASI.Services.Statistics.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiTemplate.Models;

namespace WebApiTemplate.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/metrics")]
    public class MetricsController : ApiController
    {
        private readonly IQuery _store;

        public MetricsController(IQuery store)
        {
            _store = store;
        }

        [Route("home"), AllowAnonymous]
        public async Task<IHttpActionResult> Get()
        {
            var result = (await _store.Filter<ExecutionTimeRecord>(typeof(ExecutionTimeRecord).Name, r => r.Method.EndsWith("/home"))).OrderByDescending(r => r.Observation.TimeStamp);
            return Ok(result.ToArray());
        }
    }
}
