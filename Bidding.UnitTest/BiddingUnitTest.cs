using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bidding.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Bidding.UnitTest
{
    [TestClass]
    public class BiddingUnitTest
    {
        [TestMethod]
        public void AddBidding()
        {
            using (var db = new BiddingContext())
            {
                var item = new BiddingItem() { GroupName="WESP.DIST", Name = "mug", Description = "red mug", OwnerId = 1 };
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
                item.Actions = new List<BiddingAction>();
                item.Actions.Add(action1);
                item.Actions.Add(action2);
                db.BiddingItems.Add(item);
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void RetrieveBidding()
        {
            using (var context = new BiddingContext())
            {
                var bidItems = context.BiddingItems
                     .Include("Config")
                     .Include("Actions")
                     .Where(i => i.OwnerId == 1);

                foreach (var bidItem in bidItems)
                {
                    Console.WriteLine("Item ID: {0} Name: {1} Owner Id: {2}",
                        bidItem.BiddingItemId,
                        bidItem.Name,
                        bidItem.OwnerId);
                    var actions = bidItem.Actions.OrderBy(a => a.TimeStamp);
                    if (actions != null)
                    {
                        foreach (var a in actions)
                        {
                            Console.WriteLine("{0} {1} {2:d}", a.UserName, a.Price, a.TimeStamp);
                        }
                    }
                }

            }
        }
    }
}
