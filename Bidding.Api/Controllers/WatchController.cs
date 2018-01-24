using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Bidding.Bol;
using Microsoft.Web.Http;

namespace Bidding.Api.Controllers
{
    //[BasicAuthenticationFilter]
    [EnableCors("*", "*", "*")]
    [ApiVersion("1.0")]
    public class WatchController : ApiController
    {

        [HttpGet]
        [Route("watch/{id}")]
        [Route("v{version:apiVersion}/watch/{id}")]
        [AllowAnonymous]
        public IHttpActionResult Watch(int id)
        {
            var item = BiddingManager.GetWatcher(id);
            return Ok(item);
        }

        [Route("watch")]
        [Route("v{version:apiVersion}/watch")]
        [AllowAnonymous]
        public IHttpActionResult PostWatch([FromBody]List<Watcher> watchers)
        {
            var watchList = new List<Watcher>();
            var error = string.Empty;
            if (watchers != null && watchers.Count > 0)
            {
                for( var i =0; i < watchers.Count; i++) 
                {
                    var watcher = watchers[i];
                    if( watcher.ItemId > 0 && watcher.UserId > 0)
                    {
                        BiddingManager.AddWatcher(watcher);
                    }
                    else
                    {
                        error += string.Format("Item Id or userId is missing for #{0} item", i+1 );
                        break;
                    }
                }
            }
            else
            {
                error = "No watch item is provided!";
            }

            if (!string.IsNullOrEmpty(error))
                return BadRequest(error);

            return Ok(watchList);
        }
    }
}