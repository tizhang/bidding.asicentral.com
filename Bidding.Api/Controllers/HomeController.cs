﻿using System;
using System.Net;
using System.Net.Http;
using ASI.Services.Statistics.Data;
using Microsoft.Web.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Bidding.Api.Models;

namespace Bidding.Api.Controllers
{
    [EnableCors("*", "*", "*")]
    [ApiVersion("1.0")]
    public class HomeController : ApiController
    {
        // GET: api/home
        // GET: api/v1/home
        [Route("home")]
        [Route("v{version:apiVersion}/home")]
        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            var result = new[] { "value1", "value2" };
            return Ok(result);
        }

        // GET: api/home/5
        // GET: api/v1/home/5
        [Route("home/{id}")]
        [Route("v{version:apiVersion}/home/{id}")]
        [AllowAnonymous]
        public IHttpActionResult Get(int id)
        {
            var result = "value";
            return Ok(result);
        }

        // POST: api/home
        // POST: api/v1/home
        [Route("home")]
        [Route("v{version:apiVersion}/home")]
        [Authorize(Roles = "Administrators")]
        public IHttpActionResult Post([FromBody]string value)
        {
            return Ok();
        }

        // PUT: api/home/5
        // PUT: api/v1/home/5
        [Route("home/{id}")]
        [Route("v{version:apiVersion}/home/{id}")]
        [Authorize(Roles = "Administrators")]
        public IHttpActionResult Put(int id, [FromBody] string value)
        {
            return Ok();
        }

        // DELETE: api/home/5
        // DELETE: api/v1/home/5
        [Route("home/{id}")]
        [Route("v{version:apiVersion}/home/{id}")]
        [Authorize]
        public IHttpActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
