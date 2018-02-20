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
                new Bidding.Data.User() { Name = "tzhang",FirstName="Tianyun", LastName="Zhang", Email = "tzhang@asicentral.com", Password = "password1", Groups = "DIST,SPLR,ADMG", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "mzhang",FirstName="May", LastName="Zhang",  Email = "mzhang@asicentral.com", Password = "password1", Groups = "DIST,SPLR,ADMG", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "cchen",FirstName="Caroline", LastName="Chen",  Email = "cchen@asicentral.com", Password = "password1", Groups = "DIST,SPLR,ADMG", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "yfang",FirstName="Yoyo", LastName="Fang",  Email = "yfang@asicentral.com", Password = "password1", Groups = "DIST,SPLR,ADMG", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tdist1",FirstName="Test1", LastName="Tdist1", Email = "tdist1@asicentral.com", Password = "password1", Groups = "DIST", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tsplr1",FirstName="Test2", LastName="Tsplr1", Email = "tsplr1@asicentral.com", Password = "password1", Groups = "SPLR", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tadmg1",FirstName="Test3", LastName="Tadmg1", Email = "tadmg1@asicentral.com", Password = "password1", Groups = "ADMG", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tdist2",FirstName="Test4", LastName="Tdist2", Email = "tdist2@asicentral.com", Password = "password1", Groups = "DIST", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tsplr2",FirstName="Test5", LastName="Tsplr2", Email = "tsplr2@asicentral.com", Password = "password1", Groups = "SPLR", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tadmg2",FirstName="Test6", LastName="Tadmg2", Email = "tadmg2@asicentral.com", Password = "password1", Groups = "ADMG", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tdist3",FirstName="Test7", LastName="Tdist3", Email = "tdist3@asicentral.com", Password = "password1", Groups = "DIST", CreateDate = DateTime.Now },
                new Bidding.Data.User() { Name = "tsplr3",FirstName="Test8", LastName="Tsplr3", Email = "tsplr3@asicentral.com", Password = "password1", Groups = "SPLR", CreateDate = DateTime.Now }
            };
            using (var db = new Data.BiddingContext())
            {
                db.Users.AddRange(users);
                db.SaveChanges();
            }
            var item = AddBiddingItem("1000 aprons", "1000 Aprons", "http://media.asicdn.com/images/jpgb/20070000/20070850.jpg", "yfang", 4000, DateTime.Now.AddDays(3), "SPLR", BiddingItem.StagingStatus, 20, DateTime.Now.AddDays(10));
            AddBiddingActions(item, new string[] { "tdist1" });


            item = AddBiddingItem("PFP usb", "ESP PFP usb product will be shown as position 1 in search results on our apps", "http://media.asicdn.com/images/jpgb/6360000/6360473.jpg", "cchen", 400, DateTime.Now.AddHours(-2), "ADMG", BiddingItem.ActiveStatus, 10, DateTime.Now.AddDays(10));
            AddBiddingActions(item, new string[] { "tadmg1", "tzhang", "mzhang", "yfang", "tadmg2" });

            item = AddBiddingItem("Category POTD gift baskets", "ESP Category Product of the Day gift baskets product will be shown in search results every Wednesday in March on our apps ", "https://media.asicdn.com/images/jpgo/21660000/21665793.jpg", "cchen", 300, DateTime.Now.AddHours(-2), "ADMG", BiddingItem.ActiveStatus, 10, DateTime.Now.AddDays(10));
            AddBiddingActions(item, new string[] { "tadmg1", "tzhang", "mzhang", "yfang", "tadmg2" });

            item = AddBiddingItem("POTD", "POTD - April - Monday. Your designated product of the day will be shown every Monday in April on our apps homepage.", "http://media.asicdn.com/images/jpgb/21470000/21477213.jpg", "cchen", 500, DateTime.Now.AddDays(30), "ADMG", BiddingItem.StagingStatus, 10, DateTime.Now.AddDays(10));


            item = AddBiddingItem("1000 pen", "1000 blue pens", "http://media.asicdn.com/images/jpgb/20480000/20488855.jpg", "tzhang", 1000, DateTime.Now.AddHours(2), "DIST", BiddingItem.StagingStatus, -20, DateTime.Now.AddMinutes(150));

            AddBiddingItem("1000 shirts", "1000 Gildan Ladies Heavy Cotton 100% Cotton V-Neck T-Shirt", "http://media.asicdn.com/images/jpgb/81020000/81022564.jpg", "mzhang", 2000, DateTime.Now.AddMinutes(1), "DIST", BiddingItem.DraftStatus, -10);

            AddBiddingItem("1 watch", "1 Citizen Eco-Drive Skyhawk A-T Black Dial Mens Watch", "http://media.asicdn.com/images/jpgb/22290000/22297307.jpg", "mzhang", 100, DateTime.Now.AddMinutes(10), "SPLR", BiddingItem.StagingStatus, 20, DateTime.Now.AddMinutes(100));
            AddBiddingItem("3000 calendars", "3000 Fisherman's Guide appointment calendars", "http://media.asicdn.com/images/jpgb/20770000/20777767.jpg", "mzhang", 2000, DateTime.Now.AddMinutes(1), "SPLR", BiddingItem.DraftStatus, 20, DateTime.Now.AddMinutes(5));

            AddBiddingItem("100 Hoodies", "100 Heavy Blend (TM) Hoods", "http://media.asicdn.com/images/jpgb/7690000/7691233.jpg", "mzhang", 500, DateTime.Now.AddMinutes(2), "SPLR", BiddingItem.DraftStatus, 100, DateTime.Now.AddMinutes(5));
            
            item = AddBiddingItem("200 mugs", "200 Sporty Travel Mugs with Handle", "http://media.asicdn.com/images/jpgo/7880000/7887725.jpg", "yfang", 800, DateTime.Now.AddHours(-3),"SPLR", BiddingItem.ActiveStatus, 20);
            AddBiddingActions(item, new string[] { "tdist3", "tzhang", "cchen", "yfang" });

            item = AddBiddingItem("1000 tools", "1000 7-in-1 Multi-Tools", "http://media.asicdn.com/images/jpgb/7910000/7915335.jpg", "yfang", 3000, DateTime.Now.AddHours(-3), "DIST", BiddingItem.ActiveStatus, 100);
            AddBiddingActions(item, new string[] { "tsplr2", "mzhang", "tsplr1", "tsplr3" });

            item = AddBiddingItem("5000 watches", "5000 Elegant LED Bracelet Watches", "http://media.asicdn.com/images/jpgb/22620000/22620415.jpg", "mzhang", 8000, DateTime.Now.AddMinutes(4), "SPLR", BiddingItem.ActiveStatus, 200);
            AddBiddingActions(item, new string[] { "tdist1", "tdist2", "tdist3", "cchen", "yfang", "tdist1", "tdist2" });

            item = AddBiddingItem("1000 shirt", "1000 Red House (R) Ladies' Nailhead Non-Iron Button-Down Shirt", "http://media.asicdn.com/images/jpgb/20950000/20956175.jpg", "mzhang", 2000, DateTime.Now, "DIST", BiddingItem.ActiveStatus, -50, DateTime.Now.AddMinutes(10));
            AddBiddingActions(item, new string[] { "tsplr1", "tsplr2", "tsplr1", "tsplr3", "tsplr2", "tsplr1", "tsplr2", "tsplr3", "tsplr2", "tsplr1" });

            item = AddBiddingItem("1000 bags of chocolate", "1000 bags of Chocolate Sports Balls Soccer", "http://media.asicdn.com/images/jpgb/21150000/21152160.jpg", "mzhang", 400, DateTime.Now, "DIST", BiddingItem.ActiveStatus, -10, DateTime.Now.AddDays(3));
            AddBiddingActions(item, new string[] { "tsplr1", "tsplr2", "tsplr3" });


        }

        private static BiddingItem AddBiddingItem(string name, string description, string imgUrl, string owner, double acceptPrice, DateTime? startTime, string group, string status = BiddingItem.StagingStatus, int? minIncrement = null, DateTime? endTime = null)
        {
            var user = UserManager.GetUser(owner);
            var item = new BiddingItem() { Name = name, Description = description, ImageUrl = imgUrl, Owner = user, CreateDate = DateTime.Now, Status = status };
            bool HighWin = minIncrement.HasValue ? minIncrement.Value > 0 : true;
            item.Setting = new BiddingSetting()
            {
                MinIncrement = minIncrement.HasValue ? minIncrement.Value : Math.Min(acceptPrice / 10, 50),
                StartPrice = acceptPrice * (HighWin ? 0.1 : 1.5),
                StartDate = startTime.HasValue ? startTime.Value : DateTime.Now,
                EndDate = endTime.HasValue ? endTime.Value : DateTime.Now.AddDays(2),
                AcceptPrice = acceptPrice,
                ShowCurrentPrice = true,
                ShowOwner = true,
                Group = group
            };
            BiddingManager.CreateItem(item);
            return item;
        }
        private static void AddBiddingActions(BiddingItem item, string[] bidders)
        {
            var newPrice = item.Setting.StartPrice;
            var rand = new Random();
            foreach (var bidder in bidders)
            {
                var obidder = UserManager.GetUser(bidder);
                newPrice = newPrice + item.Setting.MinIncrement * rand.Next(1,5);
                if (newPrice > 0)
                {
                    var action = new BiddingAction() { ItemId = item.Id, Bidder = obidder, Price = newPrice, ActionTime = DateTime.Now, Status = BiddingAction.SuccessStatus };
                    BiddingManager.AddAction(action);
                }
            }
        }

        [TestMethod]
        public void AddBidding()
        {
            var item = AddBiddingItem("bag", "1000 lunch bag", "http://media.asicdn.com/images/jpgb/6100000/6108942.jpg", "tzhang", 1500, DateTime.Now.AddHours(-2), "DIST", BiddingItem.ActiveStatus);
            AddBiddingActions(item, new string[] { "tdist1", "cchen", "yfang" });
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
                bidItem.Setting.Group);
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
            var bidItems = Bidding.Bol.BiddingManager.GetItems("DIST","ACTV,STAG");
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
            var bidder1 = UserManager.GetUser("tdist1");
            var items = BiddingManager.GetItems(bidderId: bidder1.Id);
            foreach (var item in items)
            {
                PrintBiddingItem(item);
            }
        }

        [TestMethod]
        public void ProcessNotifications()
        {
            BiddingManager.ProcessStatusChange();
        }

    }
}
