using System;
using System.Net;
using System.Net.Http;
using ASI.Services.Statistics.Data;
using Microsoft.Web.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Bidding.Api.Models;
using Bidding.Bol;

namespace Bidding.Api.Controllers
{
    [EnableCors("*", "*", "*")]
    [ApiVersion("1.0")]
    public class BiddingController : ApiController
    {

        // GET: api/bidding/5
        // GET: api/v1/bidding/5
        [Route("bidding/{id}")]
        [Route("v{version:apiVersion}/bidding/{id}")]
        [AllowAnonymous]
        public IHttpActionResult GetItem(int id)
        {
            var item = BiddingManager.GetItem(id);
            return Ok(item);
        }

        // POST: api/bidding
        // POST: api/v1/bidding
        [Route("bidding")]
        [Route("v{version:apiVersion}/bidding")]
        //        [Authorize(Roles = "Administrators")]
        [AllowAnonymous]
        public IHttpActionResult Post([FromBody]string value)
        {
            return Ok();
        }



        // PUT: api/bidding/5
        // PUT: api/v1/bidding/5
        [Route("bidding/{id}")]
        [Route("v{version:apiVersion}/bidding/{id}")]
        [Authorize(Roles = "Administrators")]
        public IHttpActionResult Put(int id, [FromBody] string value)
        {
            return Ok();
        }

        // DELETE: api/bidding/5
        // DELETE: api/v1/bidding/5
        [Route("bidding/{id}")]
        [Route("v{version:apiVersion}/bidding/{id}")]
        [Authorize]
        public IHttpActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
