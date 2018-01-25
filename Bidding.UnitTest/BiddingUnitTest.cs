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
                new Bidding.Data.User() { Name = "tzhang",FirstName="Tianyun", LastName="Zhang", Email = "tzhang@asicentral.com", Password = "password1", Groups = "WESP,SESP,ADMT", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "mzhang",FirstName="May", LastName="Zhang",  Email = "mzhang@asicentral.com", Password = "password1", Groups = "WESP,SESP,ADMT", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "cchen",FirstName="Caroline", LastName="Chen",  Email = "cchen@asicentral.com", Password = "password1", Groups = "WESP,SESP,ADMT", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "yfang",FirstName="Yoyo", LastName="Fang",  Email = "yfang@asicentral.com", Password = "password1", Groups = "WESP,SESP,ADMT", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "twesp1",FirstName="Test1", LastName="Bidding1", Email = "tbidding1@asicentral.com", Password = "password1", Groups = "WESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tsesp1",FirstName="Test2", LastName="Bidding2", Email = "tbidding2@asicentral.com", Password = "password1", Groups = "SESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tadmt1",FirstName="Test3", LastName="Bidding3", Email = "tbidding3@asicentral.com", Password = "password1", Groups = "ADMT", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "twesp2",FirstName="Test4", LastName="Bidding4", Email = "tbidding4@asicentral.com", Password = "password1", Groups = "WESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tsesp2",FirstName="Test5", LastName="Bidding5", Email = "tbidding5@asicentral.com", Password = "password1", Groups = "SESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tadmt2",FirstName="Test6", LastName="Bidding6", Email = "tbidding6@asicentral.com", Password = "password1", Groups = "ADMT", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "twesp3",FirstName="Test7", LastName="Bidding7", Email = "tbidding7@asicentral.com", Password = "password1", Groups = "WESP", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tsesp3",FirstName="Test8", LastName="Bidding8", Email = "tbidding8@asicentral.com", Password = "password1", Groups = "SESP", CreateDate = DateTime.Now }
            };
            using (var db = new Data.BiddingContext())
            {
                db.Users.AddRange(users);
                db.SaveChanges();
            }

            var item = AddBiddingItem("pen", "1000 blue pen", "http://media.asicdn.com/images/jpgb/20480000/20488855.jpg", "tzhang", 1000, DateTime.Now.AddHours(-1), "WESP", BiddingItem.ActiveStatus);
            AddBiddingActions(item, "twesp1", "twesp2");
            AddBiddingItem("shirt", "1000 Gildan Ladies Heavy Cotton 100% Cotton V-Neck T-Shirt", "http://media.asicdn.com/images/jpgb/81020000/81022564.jpg", "mzhang", 2000, DateTime.Now, "SESP", BiddingItem.DraftStatus, -10);

            item = AddBiddingItem("shirt", "1000 Red House (R) Ladies' Nailhead Non-Iron Button-Down Shirt", "http://media.asicdn.com/images/jpgb/20950000/20956175.jpg", "mzhang", 2000, DateTime.Now, "SESP", BiddingItem.ActiveStatus, -10);
            AddBiddingActions(item, "tsesp1", "tsesp2");

            item = AddBiddingItem("chocolate", "1000 bags of Chocolate Sports Balls Soccer", "http://media.asicdn.com/images/jpgb/21150000/21152160.jpg", "mzhang", 400, DateTime.Now, "SESP", BiddingItem.ActiveStatus, -10);
            AddBiddingActions(item, "tsesp1", "tsesp2");



            AddBiddingItem("lanyard", "1000 Sliding Button Lanyard", "http://media.asicdn.com/images/jpgb/21470000/21477213.jpg", "mzhang", 1000, DateTime.Now.AddMinutes(3), "ADMT", BiddingItem.StagingStatus, 20);
            AddBiddingItem("watch", "1 Citizen Eco-Drive Skyhawk A-T Black Dial Mens Watch", "http://media.asicdn.com/images/jpgb/22290000/22297307.jpg", "mzhang", 100, DateTime.Now.AddMinutes(3), "WESP", BiddingItem.StagingStatus, 20);
            AddBiddingItem("calendar", "3000 Fisherman's Guide appointment calendar", "http://media.asicdn.com/images/jpgb/20770000/20777767.jpg", "mzhang", 2000, DateTime.Now.AddMinutes(3), "WESP", BiddingItem.StagingStatus, 20);
            AddBiddingItem("Hoodie", "100 Heavy Blend (TM) Hood", "http://media.asicdn.com/images/jpgb/7690000/7691233.jpg", "mzhang", 800, DateTime.Now.AddMinutes(3), "WESP", BiddingItem.StagingStatus, 20);
            

            item = AddBiddingItem("lanyard", "1000 Rhinestone Lanyards", "http://media.asicdn.com/images/jpgb/21490000/21496672.jpg", "mzhang", 1000, DateTime.Now.AddMinutes(-30), "ADMT", BiddingItem.ActiveStatus, 20);
            AddBiddingActions(item, "tadmt1", "tadmt2");

            


            item = AddBiddingItem("usb", "100 Puzzle Cube USB drive", "http://media.asicdn.com/images/jpgb/6360000/6360473.jpg", "cchen", 400, DateTime.Now.AddHours(-2), "ADMT", BiddingItem.ActiveStatus, 10);
            AddBiddingActions(item, "tadmt1", "tadmt2");
            item = AddBiddingItem("mug", "200 Sporty Travel Mugs with Handles", "http://media.asicdn.com/images/jpgo/7880000/7887725.jpg", "yfang", 800, DateTime.Now.AddHours(-3),"WESP", BiddingItem.ActiveStatus, 20);
            AddBiddingActions(item, "twesp2", "twesp3");

            item = AddBiddingItem("tool", "1000 7-in-1 Multi-Tool", "http://media.asicdn.com/images/jpgb/7910000/7915335.jpg", "yfang", 3000, DateTime.Now.AddHours(-3), "WESP", BiddingItem.ActiveStatus, 20);
            AddBiddingActions(item, "twesp2", "twesp3");
            item = AddBiddingItem("watches", "5000 Elegant LED Bracelet Watches", "http://media.asicdn.com/images/jpgb/22620000/22620415.jpg", "yfang", 8000, DateTime.Now.AddHours(-3), "WESP", BiddingItem.ActiveStatus, 20);
            AddBiddingActions(item, "twesp2", "twesp3");
            item = AddBiddingItem("apron", "1000 Aprons", "http://media.asicdn.com/images/jpgb/20070000/20070850.jpg", "yfang", 4000, DateTime.Now.AddHours(-3), "WESP", BiddingItem.ActiveStatus, 20);
            AddBiddingActions(item, "twesp2", "twesp3");

        }

        private static BiddingItem AddBiddingItem(string name, string description, string imgUrl, string owner, double acceptPrice, DateTime? startTime, string group, string status = BiddingItem.StagingStatus, int? minIncrement = null)
        {
            var user = UserManager.GetUser(owner);
            var item = new BiddingItem() { Name = name, Description = description, ImageUrl = imgUrl, Owner = user, CreateDate = DateTime.Now, Status = status };
            bool HighWin = minIncrement.HasValue ? minIncrement.Value > 0 : true;
            item.Setting = new BiddingSetting()
            {
                MinIncrement = minIncrement.HasValue? minIncrement.Value : Math.Min(acceptPrice / 10, 50),
                StartPrice = acceptPrice * (HighWin? 0.1 :2.0),
                StartDate = startTime.HasValue?startTime.Value:DateTime.Now,
                EndDate = DateTime.Now.AddHours(2),
                AcceptPrice = acceptPrice,
                Groups = new List<string> { group }
            };
            BiddingManager.CreateItem(item);
            return item;
        }
        private static void AddBiddingActions(BiddingItem item, string bidder1, string bidder2)
        {
            var obidder1 = UserManager.GetUser(bidder1);
            var obidder2 = UserManager.GetUser(bidder2);
            var newPrice = item.Setting.StartPrice + item.Setting.MinIncrement;
            var action1 = new BiddingAction() { ItemId = item.Id, Bidder = obidder1, Price = newPrice, ActionTime = DateTime.Now, Status = BiddingAction.SuccessStatus };
            var action2 = new BiddingAction() { ItemId = item.Id, Bidder = obidder2, Price = newPrice + item.Setting.MinIncrement, ActionTime = DateTime.Now.AddHours(1), Status = BiddingAction.SuccessStatus };
            BiddingManager.AddAction(action1);
            BiddingManager.AddAction(action2);
        }

        [TestMethod]
        public void AddBidding()
        {
            var item = AddBiddingItem("bag", "1000 lunch bag", "http://media.asicdn.com/images/jpgb/6100000/6108942.jpg", "tzhang", 1500, DateTime.Now.AddHours(-2), "WESP", BiddingItem.ActiveStatus);
            AddBiddingActions(item, "cchen", "yfang");
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
            var bidder1 = UserManager.GetUser("twesp1");
            var items = BiddingManager.GetItems(bidderId: bidder1.Id);
            foreach (var item in items)
            {
                PrintBiddingItem(item);
            }
        }
    }
}
