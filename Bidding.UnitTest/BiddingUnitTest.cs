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
        [TestInitialize]
        public void Init()
        {
            BiddingManager.Initialize();
        }

        [TestMethod]
        public void InitializeDB()
        {
            var users = new List<Bidding.Data.User>() {
                new Bidding.Data.User() { Name = "tzhang",FirstName="Tianyun", LastName="Zhang", Email = "tzhang@asicentral.com", Password = "password1", Groups = "DIST,WESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "mzhang",FirstName="May", LastName="Zhang",  Email = "mzhang@asicentral.com", Password = "password1", Groups = "DIST,WESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "cchen",FirstName="Caroline", LastName="Chen",  Email = "cchen@asicentral.com", Password = "password1", Groups = "SUPP,SESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "yfang",FirstName="Yoyo", LastName="Fang",  Email = "yfang@asicentral.com", Password = "password1", Groups = "SUPP,SESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tbidding1",FirstName="Test1", LastName="Bidding1", Email = "tbidding1@asicentral.com", Password = "password1", Groups = "DIST,WESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tbidding2",FirstName="Test2", LastName="Bidding2", Email = "tbidding2@asicentral.com", Password = "password1", Groups = "SUPP,WESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tbidding3",FirstName="Test3", LastName="Bidding3", Email = "tbidding3@asicentral.com", Password = "password1", Groups = "DIST,WESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tbidding4",FirstName="Test4", LastName="Bidding4", Email = "tbidding4@asicentral.com", Password = "password1", Groups = "SUPP,WESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tbidding5",FirstName="Test5", LastName="Bidding5", Email = "tbidding5@asicentral.com", Password = "password1", Groups = "DIST,WESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tbidding6",FirstName="Test6", LastName="Bidding6", Email = "tbidding6@asicentral.com", Password = "password1", Groups = "SUPP,WESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tbidding7",FirstName="Test7", LastName="Bidding7", Email = "tbidding7@asicentral.com", Password = "password1", Groups = "DIST,WESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tbidding8",FirstName="Test8", LastName="Bidding8", Email = "tbidding8@asicentral.com", Password = "password1", Groups = "SUPP,WESP", CreateDate = DateTime.Now }
            };
            using (var db = new Data.BiddingContext())
            {
                db.Users.AddRange(users);
                db.SaveChanges();
            }

            AddBiddingItem("pen", "blue pen", "tzhang", "tbidding1", "tbidding2");
            AddBiddingItem("shirt", "red shirt", "mzhang", "tbidding3", "tbidding4");
            AddBiddingItem("lanyard", "yellow lanyard", "mzhang", "yfang", "tbidding2");
            AddBiddingItem("usb", "128GB usb", "cchen", "yfang", "tbidding2");
            AddBiddingItem("card", "index card", "yfang", "cchen", "tbidding2");
        }

        private static void AddBiddingItem(string name, string description, string owner, string bidder1, string bidder2)
        {
            var user = UserManager.GetUser(owner);
            var item = new BiddingItem() { Name = name, Description = description, Owner = user, CreateDate = DateTime.Now, Status = BiddingItem.DraftStatus };
            item.Setting = new BiddingSetting()
            {
                MinIncrement = 10,
                StartPrice = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                AcceptPrice = 20,
                Groups = new List<string> { "WESP" }
            };
            BiddingManager.CreateItem(item);
            var obidder1 = UserManager.GetUser(bidder1);
            var obidder2 = UserManager.GetUser(bidder2);
            var action1 = new BiddingAction() { ItemId = item.Id, Bidder = obidder1, Price = 2, ActionTime = DateTime.UtcNow.AddHours(1), Status = BiddingAction.SuccessStatus };
            var action2 = new BiddingAction() { ItemId = item.Id, Bidder = obidder2, Price = 16, ActionTime = DateTime.UtcNow.AddHours(3), Status = BiddingAction.SuccessStatus };
            BiddingManager.AddAction(action1);
            BiddingManager.AddAction(action2);
        }

        [TestMethod]
        public void AddBidding()
        {
            AddBiddingItem("mug", "blue mug","tzhang","tbidding6","tbidding7");
        }

        [TestMethod]
        public void AddAction()
        {
            var action1 = new BiddingAction() {ItemId = 2, Bidder = UserManager.GetUser(9), Price = 25, ActionTime = DateTime.UtcNow.AddHours(7) };
            var ret = BiddingManager.AddAction(action1);
            Console.WriteLine(ret.Message);
        }


        [TestMethod]
        public void AddFailedAction()
        {
            var action1 = new BiddingAction() {ItemId=2, Bidder = UserManager.GetUser(8), Price = 9, ActionTime = DateTime.UtcNow.AddHours(7) };
            var ret = BiddingManager.AddAction(action1);
            Console.WriteLine("First Add: " + ret.Success + " " + ret.Message);
            ret = BiddingManager.AddAction(action1);
            Console.WriteLine("Second Add: " + ret.Success + " " + ret.Message);
        }

        private void PrintBiddingItem(BiddingItem bidItem)
        {
            Console.WriteLine("Item ID: {0} Name: {1} Owner Id: {2} Owner Name {3} Price {4} Status {5} Groups {6}",
                bidItem.Id,
                bidItem.Name,
                bidItem.Owner.Id,
                bidItem.Owner.Name,
                bidItem.Price,
                bidItem.Status,
                bidItem.Setting.Groups?.FirstOrDefault());
            var actions = bidItem.History.OrderBy(a => a.ActionTime);
            if (actions != null)
            {
                foreach (var a in actions)
                {
                    Console.WriteLine("{0} {1} {2:d} {3}", a.Bidder?.Email, a.Price, a.ActionTime, a.Status);
                }
            }
        }
        [TestMethod]
        public void GetItemById()
        {
            var bidItem = Bidding.Bol.BiddingManager.GetItem(2);
            PrintBiddingItem(bidItem);
        }
        [TestMethod]
        public void GetItemByGroup()
        {
            var bidItems = Bidding.Bol.BiddingManager.GetItems("WESP","ACTV,STAG");
            foreach (var bidItem in bidItems)
            {
                PrintBiddingItem(bidItem);
            }
        }
        [TestMethod]
        public void UpdateItem()
        {
            var item = BiddingManager.GetItem(3);
            item.Status = "ACTV";
            BiddingManager.UpdateItem(item);
        }

        [TestMethod]
        public void AddWatcher()
        {
            var w = new Watcher() { UserId = 2, BiddingItemId = 3, IsActive = true };
            BiddingManager.AddOrUpdateWatcher(w);
            w = new Watcher() { UserId = 2, BiddingItemId = 4, IsActive = true };
            BiddingManager.AddOrUpdateWatcher(w);

        }
        [TestMethod]
        public void GetWatchers()
        {
            var ws = BiddingManager.GetWatchers(2);
            foreach (var w in ws)
            {
                Console.WriteLine("{0} {1}", w.UserId, w.BiddingItemId);
            }
        }
        [TestMethod]
        public void GetNotifications()
        {
            var nfs = BiddingManager.GetNotifications(1);
            foreach(var n in nfs)
            {
                Console.WriteLine("{0} {1} {2}", n.BiddingItemId, n.Message, n.EventTime);
            }
        }

        [TestMethod]
        public void GetItemsByOwnerId()
        {
            var items = BiddingManager.GetItems(ownerId:2);
            foreach (var item in items)
            {
                PrintBiddingItem(item);
            }
        }

        [TestMethod]
        public void GetItemsByBidderId()
        {
            var bidder1 = UserManager.GetUser("tbidding1");
            var items = BiddingManager.GetItems(bidderId: bidder1.Id);
            foreach (var item in items)
            {
                PrintBiddingItem(item);
            }
        }
    }
}
