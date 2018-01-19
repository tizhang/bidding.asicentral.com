using System;
using System.Net;
using System.Net.Http;
using ASI.Services.Statistics.Data;
using Microsoft.Web.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Bidding.Api.Models;
using Bidding.Data;

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
        public IHttpActionResult Get(int id)
        {
            var result = "value";
            return Ok(result);
        }

        // POST: api/bidding
        // POST: api/v1/bidding
        [Route("bidding")]
        [Route("v{version:apiVersion}/bidding")]
        //        [Authorize(Roles = "Administrators")]
        [AllowAnonymous]
        public IHttpActionResult Post([FromBody]string value)
        {
            using (var db = new BiddingContext())
            {
                var item = new BiddingItem() { Name = value };
                //item.Config = new BiddingConfig()
                //{
                //    MinIncrement = 10,
                //    StartPrice = 1,
                //    StartDate = DateTime.Now,
                //    EndDate = DateTime.Now.AddDays(3),
                //    Type = BiddingType.HighWin
                //};
                //var action1 = new BiddingAction() { UserId = 10, UserName = "user1", Price = 2, TimeStamp = DateTime.Now.AddHours(1) };
                //var action2 = new BiddingAction() { UserId = 11, UserName = "user2", Price = 6, TimeStamp = DateTime.Now.AddHours(3) };
                //item.Actions.Add(action1);
                //item.Actions.Add(action2);
                db.BiddingItems.Add(item);
                db.SaveChanges();
            }
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
