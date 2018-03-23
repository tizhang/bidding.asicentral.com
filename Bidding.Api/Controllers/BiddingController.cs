﻿using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Bidding.Api.Filters;
using Bidding.Bol;
using Microsoft.Web.Http;

namespace Bidding.Api.Controllers
{
    //[BasicAuthenticationFilter]
    [EnableCors("*", "*", "*")]
    [ApiVersion("1.0")]
    public class BiddingController : ApiController
    {
        #region BiddingItem
        // GET: api/bidding/5
        // GET: api/v1/bidding/5
        [Route("bidding")]
        //[Route("v{version:apiVersion}/bidding")]
        [AllowAnonymous]
        public IHttpActionResult GetItems(string groups = null, string status = null, int? ownerId = null, int? bidderId = null, bool includeSettings = false, bool includeHistory = false)
        {
            var items = BiddingManager.GetItems(groups, status, ownerId, bidderId);
            if (!includeHistory)
            {
                items.ForEach(i => i.History = null);
            }
            if (!includeSettings)
            {
                items.ForEach(i => i.Setting = null);
            }
            return Ok(items);
        }

        // GET: api/bidding/5
        // GET: api/v1/bidding/5
        [Route("bidding/{id}")]
        //[Route("v{version:apiVersion}/bidding/{id}")]
        [AllowAnonymous]
        public IHttpActionResult GetItem(int id)
        {
            var item = BiddingManager.GetItem(id);
            return Ok(item);
        }

        [Route("bidding")]
        //[Route("v{version:apiVersion}/bidding")]
        //        [Authorize(Roles = "Administrators")]
        [AllowAnonymous]
        public IHttpActionResult PostItem([FromBody]BiddingItem item)
        {
            if (item != null)
            {
                BiddingManager.CreateItem(item);
            }
            return Ok(item);
        }

        // PUT: api/bidding/5
        // PUT: api/v1/bidding/5
        [HttpPut]
        [Route("bidding/{id}")]
        //[Route("v{version:apiVersion}/bidding")]
        //[Authorize(Roles = "Administrators")]
        [AllowAnonymous]
        public IHttpActionResult PutItem(long id, [FromBody]BiddingItem item)
        {
            var error = "";
            if (item != null)
            {
                var ret = BiddingManager.UpdateItem(item);
                if (ret.Success)
                {
                    return Ok(item);
                }
                else
                {
                    error = ret.Message;
                }
            }
            return BadRequest(error);
        }

        //// DELETE: api/bidding/5
        //// DELETE: api/v1/bidding/5
        //[Route("bidding/{id}")]
        ////[Route("v{version:apiVersion}/bidding/{id}")]
        ////[Authorize]
        //[AllowAnonymous]
        //public IHttpActionResult Delete(int id)
        //{
        //    return Ok();
        //}
        #endregion BiddingItem

        #region action
        // GET: api/action/item/5
        // GET: api/v1/bidding/5
        [Route("action/{id}")]
        //[Route("v{version:apiVersion}/action/{id}")]
        [AllowAnonymous]
        public IHttpActionResult GetAction(int id)
        {
            var item = BiddingManager.GetAction(id);

            return Ok(item);
        }

        [Route("action/item/{id}")]
        //[Route("v{version:apiVersion}/action/item/{id}")]
        [AllowAnonymous]
        public IHttpActionResult GetActionsByItem(int id)
        {
            var item = BiddingManager.GetActions(id, false);
            return Ok(item);
        }

        [Route("action/user/{id}")]
        //[Route("v{version:apiVersion}/action/user/{id}")]
        [AllowAnonymous]
        public IHttpActionResult GetActionsByUser(int id)
        {
            var item = BiddingManager.GetActions(id, true);
            return Ok(item);
        }

        [Route("action")]
        //[Route("v{version:apiVersion}/action")]
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
                    var newAction = BiddingManager.GetAction(action.Id);
                    var clients = SignalRHelper.GetClients();
                    if (clients != null)
                        clients.All.updateBiddingItem(action.ItemId);
                    return Ok(newAction);
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
        #endregion action

        #region Notification & Watch
        [Route("notification/{userid}")]
        //[Route("v{version:apiVersion}/action/user/{id}")]
        [AllowAnonymous]
        public IHttpActionResult GetNotificationByUser(int userid)
        {
            var items = BiddingManager.GetNotifications(userid);
            return Ok(items);
        }

        [Route("notificationAck/{userid}")]
        //[Route("v{version:apiVersion}/action/user/{id}")]
        [AllowAnonymous]
        public IHttpActionResult GetNotificationAck(int userid)
        {
            var item = BiddingManager.GetNotificationAck(userid);
            return Ok(item);
        }

        [Route("notificationAck/{userid}")]
        //[Route("v{version:apiVersion}/action/user/{id}")]
        [AllowAnonymous]
        public IHttpActionResult PostNotificationAck(int userid, NotificationAck ack)
        {
            //DateTime lastAccessDate = nack.LastAccessDate;
            //var ack = new NotificationAck() { UserId = userid, LastAccessDate = lastAccessDate };
            ack.LastAccessDate = ack.LastAccessDate.ToLocalTime();
            var ret = BiddingManager.AddOrUpdateNotificationAck(ack);
            if (ret.Success)
            {
                return Ok();
            }
            return BadRequest(ret.Message);
        }

        [Route("watch")]
        //[Route("v{version:apiVersion}/action")]
        //        [Authorize(Roles = "Administrators")]
        [AllowAnonymous]
        public IHttpActionResult PostWatch([FromBody]Watcher watcher)
        {
            var ret = BiddingManager.AddOrUpdateWatcher(watcher);
            if (ret.Success)
            {
                return Ok();
            }
            return BadRequest(ret.Message);
        }

        [Route("watch/{userid}")]
        //[Route("v{version:apiVersion}/action")]
        //        [Authorize(Roles = "Administrators")]
        [AllowAnonymous]
        public IHttpActionResult GetWatch(int userid)
        {
            var watchers = BiddingManager.GetWatchers(userid);
            return Ok(watchers);
        }
        #endregion notification & watch

        #region login/out
        [HttpGet]
        [Route("login")]
        //[Route("v{version:apiVersion}/login")]
        //        [Authorize(Roles = "Administrators")]
        [AllowAnonymous]
        public IHttpActionResult Login(string email, string password)
        {
            var user = BiddingManager.GetUser(email, password);
            return Ok(user);
        }
        #endregion login/out
    }
}
