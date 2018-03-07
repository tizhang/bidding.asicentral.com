using ASI.Contracts.Messages.Scheduling;
using ASI.Services.Messaging;
using Bidding.Bol;
using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Bidding.Api
{
    public class BiddingJobScheduler
    {
        private readonly static Lazy<BiddingJobScheduler> _instance = new Lazy<BiddingJobScheduler>(() => new BiddingJobScheduler());
        private readonly Timer _timer;

        public static BiddingJobScheduler Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private BiddingJobScheduler()
        {
            _timer = new Timer(ProcessJob, null, 10000, 5000); 
        }

        private void ProcessJob(object state)
        {
            BiddingManager.ProcessStatusChange();
            var clients = SignalRHelper.GetClients();
            if (clients != null)
                clients.All.notify("test");
        }
    }
}