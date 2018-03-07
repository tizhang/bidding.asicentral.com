using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Bidding.Bol;

namespace Bidding.Api
{
    [HubName("biddingApi")]
    public class BiddingHub : Hub
    {
        public void Notify(string message)
        {
            Clients.All.notify(message);
        }

    }
}