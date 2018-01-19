using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bidding.Data;
using System.Configuration;

namespace Bidding.UnitTest
{
    [TestClass]
    public class BiddingUnitTest
    {
        [TestMethod]
        public void AddBidding()
        {
            var connectStr = ConfigurationManager.ConnectionStrings["BiddingContext"].ConnectionString;
            using (var db = new BiddingContext(connectStr))
            {
                var item = new BiddingItem() { Name = "mug", Description = "red mug" };
                item.Config = new BiddingConfig()
                {
                    MinIncrement = 10,
                    StartPrice = 1,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(3),
                    Type = BiddingType.HighWin
                };
                var action1 = new BiddingAction() { UserId = 10, UserName = "user1", Price = 2, TimeStamp = DateTime.Now.AddHours(1) };
                var action2 = new BiddingAction() { UserId = 11, UserName = "user2", Price = 6, TimeStamp = DateTime.Now.AddHours(3) };
                item.Actions.Add(action1);
                item.Actions.Add(action2);
                db.BiddingItems.Add(item);
                db.SaveChanges();
            }
        }
    }
}
