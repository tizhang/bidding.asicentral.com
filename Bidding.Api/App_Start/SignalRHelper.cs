using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Bidding.Api
{
    public class SignalRHelper
    {

        public static IHubConnectionContext<dynamic> GetClients()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<BiddingHub>();
            if (hubContext != null)
            {
                return hubContext.Clients;
            }
            return null;
        }
    }
}