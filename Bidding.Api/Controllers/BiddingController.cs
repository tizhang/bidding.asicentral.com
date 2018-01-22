using System.Web.Http;
using System.Web.Http.Cors;
using Bidding.Bol;
using Microsoft.Web.Http;
using Newtonsoft.Json;

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
            //value = @"{""Id"":0,""Name"":""item post1"",""Description"":""item post 1"",""ImageUrl"":null,""BidTimes"":0,""Price"":1.0,""OwnerId"": 1,""OwnerEmail"":""postItem1@gmail.com"",""Status"":""Initial"",""CreateDate"":""2018-01-20T18:48:35.933"",""Setting"":{""Id"":2,""Groups"":[],""MinIncrement"":10.0,""ShowOwner"":false,""ShowCurrentPrice"":false,""BidTimePerUser"":null,""AcceptMinPrice"":0.0,""StartPrice"":1.0,""EndDate"":""2018-01-23T18:48:36.663"",""StartDate"":""2018-01-20T18:48:36.663""}}";
            var item = JsonConvert.DeserializeObject<BiddingItem>(value);
            if (item != null)
            {
                BiddingManager.CreateItem(item);
            }
            return Ok(item);
        }

        [Route("bidding")]
        [Route("v{version:apiVersion}/bidding")]
        //        [Authorize(Roles = "Administrators")]
        [AllowAnonymous]
        public IHttpActionResult Post([FromBody]BiddingItem item)
        {
            if (item != null)
            {
                BiddingManager.CreateItem(item);
            }
            return Ok(item);
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

        // GET: api/bidding/5
        // GET: api/v1/bidding/5
        [Route("action/{id}")]
        [Route("v{version:apiVersion}/action/{id}")]
        [AllowAnonymous]
        public IHttpActionResult Action(int id)
        {
            var item = BiddingManager.GetAction(id);
            return Ok(item);
        }

        [Route("action")]
        [Route("v{version:apiVersion}/action")]
        //        [Authorize(Roles = "Administrators")]
        [AllowAnonymous]
        public IHttpActionResult PostAction([FromBody]string value)
        {
            var action = JsonConvert.DeserializeObject<BiddingAction>(value);
            if (action != null)
            {
                BiddingManager.AddAction(action);
            }
            return Ok(action);
        }

        [Route("action")]
        [Route("v{version:apiVersion}/action")]
        //        [Authorize(Roles = "Administrators")]
        [AllowAnonymous]
        public IHttpActionResult PostAction([FromBody]BiddingAction action)
        {
            if (action != null)
            {
                BiddingManager.AddAction(action);
            }
            return Ok(action);
        }
    }
}
