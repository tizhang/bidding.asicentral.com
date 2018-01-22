using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bidding.Bol;
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
            var item = new BiddingItem() { Name = "pen", Description = "blue pen", Owner = new User() { Id = 1, Email = "tzhang@asicentral.com" }, CreateDate = DateTime.Now };
            item.Setting = new BiddingSetting()
            {
                MinIncrement = 10,
                StartPrice = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                AcceptPrice = 20
            };
            var action1 = new BiddingAction() { Bidder = new User() { Id = 5, Email = "pzhang@asicentral.com" }, Price = 2, ActionTime = DateTime.UtcNow.AddHours(1) };
            var action2 = new BiddingAction() { Bidder = new User() { Id = 7, Email = "123@asicentral.com" }, Price = 6, ActionTime = DateTime.UtcNow.AddHours(3) };
            item.History = new List<BiddingAction>();
            item.History.Add(action1);
            item.History.Add(action2);
            BiddingManager.CreateItem(item);
        }

        [TestMethod]
        public void AddAction()
        {
            var action1 = new BiddingAction() { Bidder = new User() { Id = 9, Email = "abcd@asicentral.com" }, Price = 9, ActionTime = DateTime.UtcNow.AddHours(7) };
            BiddingManager.AddAction(2, action1);
        }


        [TestMethod]
        public void AddFailedAction()
        {
            var action1 = new BiddingAction() { Bidder = new User() { Id = 9, Email = "abcd@asicentral.com" }, Price = 9, ActionTime = DateTime.UtcNow.AddHours(7) };
            var ret = BiddingManager.AddAction(2, action1);
            Console.WriteLine("First Add: " + ret.Success + " " + ret.Message);
            ret = BiddingManager.AddAction(2, action1);
            Console.WriteLine("Second Add: " + ret.Success + " " + ret.Message);
        }
        [TestMethod]
        public void RetrieveBidding()
        {
            var bidItem = Bidding.Bol.BiddingManager.GetItem(2);
            Console.WriteLine("Item ID: {0} Name: {1} Owner Id: {2}",
                bidItem.Id,
                bidItem.Name,
                bidItem.Owner?.Id);
            var actions = bidItem.History.OrderBy(a => a.ActionTime);
            if (actions != null)
            {
                foreach (var a in actions)
                {
                    Console.WriteLine("{0} {1} {2:d}", a.Bidder?.Email, a.Price, a.ActionTime);
                }
            }
        }
    }
}
