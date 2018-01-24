using ASI.Contracts.Messages.Scheduling;
using ASI.Services.Messaging;
using Bidding.Bol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bidding.Api
{
    public class ScheduleConfig
    {
        private static Consumer consumer = new Consumer();

        public static void Setup()
        {
            var request = new ScheduleRequest { Name = "BindingApiSchedule", Command = ScheduleRequest.CommandAdd, Interval = TimeSpan.FromSeconds(30), RepeatForever = true };
            request.TalkAndWait<ScheduleRequest, ScheduleResponse>(response =>
           {
               
           });

            consumer.Subscribe(new[] { "BindingApiSchedule" });
        }

        public static void Cleanup()
        {
            consumer.Unsubscribe();
        }
    }

    public class Consumer : IConsumer<ScheduledEventMessage>
    {
        public Configuration Configuration => Configuration.Default;

        public string Id
        {
            get { return "binding_api_consumer"; }
            set { return; }
        }

        public void Consume(ScheduledEventMessage message)
        {
            if (message.Event != "BindingApiSchedule") return;
            BiddingManager.ProcessStatusChange();

        }
    }
}