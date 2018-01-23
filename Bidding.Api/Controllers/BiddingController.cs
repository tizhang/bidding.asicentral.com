﻿using System.Web.Http;
using System.Web.Http.Cors;
using Bidding.Bol;
using Microsoft.Web.Http;

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

        //// PUT: api/bidding/5
        //// PUT: api/v1/bidding/5
        //[Route("bidding/{id}")]
        //[Route("v{version:apiVersion}/bidding/{id}")]
        //[Authorize(Roles = "Administrators")]
        //public IHttpActionResult Put(int id, [FromBody] string value)
        //{
        //    return Ok();
        //}

        // DELETE: api/bidding/5
        // DELETE: api/v1/bidding/5
        [Route("bidding/{id}")]
        [Route("v{version:apiVersion}/bidding/{id}")]
        [Authorize]
        public IHttpActionResult Delete(int id)
        {
            return Ok();
        }

        // GET: api/action/item/5
        // GET: api/v1/bidding/5
        [Route("action/{id}")]
        [Route("v{version:apiVersion}/action/{id}")]
        [AllowAnonymous]
        public IHttpActionResult Action(int id)
        {
            var item = BiddingManager.GetAction(id);

            return Ok(item);
        }

        [Route("action/item/{id}")]
        [Route("v{version:apiVersion}/action/item/{id}")]
        [AllowAnonymous]
        public IHttpActionResult ActionsByItem(int id)
        {
            var item = BiddingManager.GetActions(id, false);
            return Ok(item);
        }

        [Route("action/user/{id}")]
        [Route("v{version:apiVersion}/action/user/{id}")]
        [AllowAnonymous]
        public IHttpActionResult ActionsByUser(int id)
        {
            var item = BiddingManager.GetActions(id, true);
            return Ok(item);
        }

        [Route("action")]
        [Route("v{version:apiVersion}/action")]
        //        [Authorize(Roles = "Administrators")]
        [AllowAnonymous]
        public IHttpActionResult PostAction([FromBody]BiddingAction action)
        {
            var error = "";
            if (action != null && action.ItemId > 0)
            {
                var ret = BiddingManager.AddAction(action);
                if (ret.Success)
                {
                    return Ok(action);
                }
                else
                {
                    error = ret.Message;
                }
            }
            else
            {
                error = "No bidding action or bidding item id is provided!";
            }
            return BadRequest(error);
        }
    }
}
