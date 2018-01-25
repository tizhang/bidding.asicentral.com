using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Bol
{
    public class BiddingManager
    {

        public static void Initialize()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Data.BiddingItem, Bol.BiddingItem>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.BiddingItemId))
                .ForMember(dest => dest.Owner, opts => opts.MapFrom(src => UserManager.GetUser(src.OwnerId)))
                .ForMember(dest => dest.History, opts => opts.MapFrom(src => src.Actions));

                cfg.CreateMap<Bol.BiddingItem, Data.BiddingItem>()
                .ForMember(dest => dest.BiddingItemId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.OwnerId, opts => opts.MapFrom(src => src.Owner != null ? src.Owner.Id : 0))
                .ForMember(dest => dest.Actions, opts => opts.MapFrom(src => src.History));


                cfg.CreateMap<Data.BiddingSetting, Bol.BiddingSetting>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.BiddingSettingId))
                .ForMember(dest => dest.Groups, opts => opts.MapFrom(src => !string.IsNullOrEmpty(src.GroupNames) ? src.GroupNames.Split(',').ToList() : null));

                cfg.CreateMap<Bol.BiddingSetting, Data.BiddingSetting>()
                .ForMember(dest => dest.BiddingSettingId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.GroupNames, opts => opts.MapFrom(src => src.Groups != null ? string.Join(",", src.Groups) : null))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => src.MinIncrement > 0 ? Data.BiddingType.HighWin : Data.BiddingType.LowWin));


                cfg.CreateMap<Data.BiddingAction, Bol.BiddingAction>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.BiddingActionId))
                .ForMember(dest => dest.ActionTime, opts => opts.MapFrom(src => src.TimeStamp))
                .ForMember(dest => dest.Bidder, opts => opts.MapFrom(src => UserManager.GetUser(src.BidderId)));

                cfg.CreateMap<Bol.BiddingAction, Data.BiddingAction>()
                .ForMember(dest => dest.BiddingActionId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.TimeStamp, opts => opts.MapFrom(src => src.ActionTime))
                .ForMember(dest => dest.BidderId, opts => opts.MapFrom(src => src.Bidder != null ? src.Bidder.Id : 0));

                cfg.CreateMap<Data.User, Bol.User>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Groups, opts => opts.MapFrom(src => !string.IsNullOrEmpty(src.Groups) ? src.Groups.Split(',').ToList() : null));

                cfg.CreateMap<Bol.User, Data.User>()
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Groups, opts => opts.MapFrom(src => src.Groups != null ? string.Join(",", src.Groups) : null));

                cfg.CreateMap<Data.Notification, Bol.Notification>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.NotificationId));

                cfg.CreateMap<Data.Watcher, Bol.Watcher>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.WatcherId));

                cfg.CreateMap<Bol.Watcher, Data.Watcher>()
                .ForMember(dest => dest.WatcherId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreateDate, opts => opts.Ignore())
                .ForMember(dest => dest.UpdateDate, opts => opts.Ignore());

                cfg.CreateMap<Data.NotificationAck, Bol.NotificationAck>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.NotificationAckId));

                cfg.CreateMap<Bol.NotificationAck, Data.NotificationAck>()
                .ForMember(dest => dest.NotificationAckId, opts => opts.MapFrom(src => src.Id));

            });

        }

        //public static CreateBidding()
        //{


        //}
        
        public static List<BiddingItem> GetItems(string group = null, string status = null, int? ownerId = null, int? bidderId = null, bool includeFailedActions = true)
        {
            List<Data.BiddingItem> bidItems = null;
            List<string> statuses = status?.Split(',').ToList();
            using (var context = new Data.BiddingContext())
            {
                var query = context.BiddingItems
                         .Include("Setting")
                         .Include("Actions")
                         .AsQueryable();
                if (!string.IsNullOrEmpty(group))
                {
                    query = query.Where(i => i.Setting.GroupNames.Contains(group));
                }
                if (statuses != null)
                {
                    query = query.Where(i => statuses.Contains(i.Status));
                }
                if (ownerId.HasValue)
                {
                    query = query.Where(i => i.OwnerId == ownerId);
                }
                if (bidderId.HasValue)
                {
                    query = query.Where(i => i.Actions.Any(a => a.BidderId == bidderId));
                }

                bidItems = query.ToList();
            }
            var items = bidItems.Select(i => Mapper.Map<BiddingItem>(i)).ToList();
            if (!includeFailedActions)
            {
                items.ForEach(i => i.History = i.History?.Where(h => h.Status == BiddingAction.SuccessStatus).ToList());
            }
            return items;
        }
        public static BiddingItem GetItem(int itemId, bool includeFailedActions = true)
        {
            Data.BiddingItem bidItem = null;
            using (var context = new Data.BiddingContext())
            {
                bidItem = context.BiddingItems
                     .Include("Setting")
                     .Include("Actions")
                     .FirstOrDefault(i => i.BiddingItemId == itemId);
            }
            if (bidItem == null) return null;
            var item = Mapper.Map<BiddingItem>(bidItem);
            if (!includeFailedActions)
            {
                item.History = item.History?.Where(h => h.Status == BiddingAction.SuccessStatus).ToList();
            }
            return item;
        }

        public static BiddingAction GetAction(int actionId)
        {
            Data.BiddingAction action = null;
            using (var context = new Data.BiddingContext())
            {
                action = context.BiddingActions
                     .FirstOrDefault(i => i.BiddingActionId == actionId);
            }
            if (action == null) return null;
            return Mapper.Map<BiddingAction>(action);
        }

        public static List<BiddingAction> GetActions(int id, bool isUser)
        {
            var actionList = new List<BiddingAction>();
            IQueryable<Data.BiddingAction> actions = null;
            try
            {
               using (var context = new Data.BiddingContext())
               {
                    if( isUser )
                    {
                        actions = context.BiddingActions.Where(a => a.BidderId == id);
                    }
                    else
                    {
                        actions = context.BiddingActions.Where(i => i.ItemId == id);
                    }

                    if (actions != null && actions.Count() > 0)
                    {
                        foreach (var action in actions)
                        {
                            actionList.Add(Mapper.Map<BiddingAction>(action));
                        }
                   }
                }
            }
            catch(Exception)
            {

            }
 
            return actionList;
        }

        public static void CreateItem(BiddingItem item)
        {
            var dItem = Mapper.Map<Data.BiddingItem>(item);
            using (var db = new Data.BiddingContext())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        dItem.CreateDate = DateTime.Now;
                        db.BiddingItems.Add(dItem);
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch( Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                    }
                }

                item.Id = dItem.BiddingItemId;
                item.Setting.Id = dItem.Setting.BiddingSettingId;
            }
        }

        private static void copyItem(Data.BiddingItem newOne, Data.BiddingItem oldOne)
        {
            oldOne.Description = newOne.Description;
            oldOne.ImageUrl = newOne.ImageUrl;
            oldOne.Name = newOne.Name;
            oldOne.Status = newOne.Status;
            Mapper.Map<Data.BiddingSetting, Data.BiddingSetting>(newOne.Setting, oldOne.Setting);
        }

        public static BiddingReturn UpdateItem(BiddingItem item)
        {
            BiddingReturn ret = new BiddingReturn()
            {
                Success = true
            };
            var newItem = Mapper.Map<Data.BiddingItem>(item);
            using (var db = new Data.BiddingContext())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var oldItem = db.BiddingItems.Find(item.Id);
                        copyItem(newItem, oldItem);
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                    }
                }
                item.Id = newItem.BiddingItemId;
                item.Setting.Id = newItem.Setting.BiddingSettingId;
            }
            return ret;
        }

        public static BiddingReturn AddAction(BiddingAction action)
        {
            BiddingReturn ret = new BiddingReturn()
            {
                Success = true
            }; 
            var dAction = Mapper.Map<Data.BiddingAction>(action);
            var notificationMessage = "";
            var imageUrl = "";
            using (var db = new Data.BiddingContext())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var item = db.BiddingItems
                                 .Include("Setting")
                                 .Include("Actions")
                                 .FirstOrDefault(i => i.BiddingItemId == action.ItemId);
                        if (item != null)
                        {
                            var setting = item.Setting;
                            if (item.Actions != null && item.Actions.Any(i => i.Status == BiddingAction.SuccessStatus))
                            {
                                var lastAction = item.Actions.Where(i => i.Status == BiddingAction.SuccessStatus).OrderBy(i => i.TimeStamp).LastOrDefault();
                                if (setting.Type == Data.BiddingType.HighWin)
                                {
                                    var nextPrice = lastAction.Price + setting.MinIncrement;
                                    if (action.Price < nextPrice)
                                    {
                                        ret = new BiddingReturn()
                                        {
                                            Success = false,
                                            Message = string.Format("Bidding Price is lower than the next required price {0}!", nextPrice)
                                        };
                                    }
                                }
                                else
                                {
                                    var nextPrice = lastAction.Price - Math.Abs(setting.MinIncrement);
                                    if (action.Price > nextPrice)
                                    {
                                        ret = new BiddingReturn()
                                        {
                                            Success = false,
                                            Message = string.Format("Bidding Price is higher than the next required price {0}!", nextPrice)
                                        };
                                    }
                                }
                            }
                            else
                            {
                                //check minimum price
                                if (setting.StartPrice > 0)
                                {
                                    if (setting.Type == Data.BiddingType.HighWin && action.Price < setting.StartPrice)
                                    {
                                        ret = new BiddingReturn()
                                        {
                                            Success = false,
                                            Message = string.Format("Bidding price is lower than the minimum start price: {0}", setting.StartPrice)
                                        };
                                    }
                                    else if (setting.Type == Data.BiddingType.LowWin && action.Price > setting.StartPrice)
                                    {
                                        ret = new BiddingReturn()
                                        {
                                            Success = false,
                                            Message = string.Format("Bidding price is higher than the maximum start price: {0}", setting.StartPrice)
                                        };
                                    }
                                }
                            }
                            bool skipSave = false;
                            if (!ret.Success)
                            {
                                var myLastOne = item.Actions.Where(a => a.BidderId == dAction.BidderId).OrderBy(i => i.TimeStamp).LastOrDefault();
                                if (myLastOne != null)
                                {
                                    if (setting.Type == Data.BiddingType.HighWin && dAction.Price < (myLastOne.Price + setting.MinIncrement))
                                    {
                                        skipSave = true;
                                    }
                                    else if (setting.Type == Data.BiddingType.LowWin && dAction.Price > (myLastOne.Price - Math.Abs(setting.MinIncrement)))
                                    {
                                        skipSave = true;
                                    }
                                }

                            }

                            if (!skipSave)
                            {
                                dAction.Status = ret.Success ? BiddingAction.SuccessStatus : BiddingAction.FailStatus;
                                item.Actions.Add(dAction);
                                if (ret.Success)
                                {
                                    item.Price = action.Price;
                                    item.BidTimes = item.Actions.Where(i => i.Status == BiddingAction.SuccessStatus).Count();
                                    notificationMessage = string.Format("bidder {0} submitted new bid price {1} for {2}", action.Bidder.Name, action.Price, item.Name);
                                    imageUrl = item.ImageUrl;
                                }
                                db.SaveChanges();
                                dbContextTransaction.Commit();
                                action.Id = dAction.BiddingActionId;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                    }
                }
                if (ret.Success && !string.IsNullOrEmpty(notificationMessage))
                {
                    AddNotification(action.ItemId, imageUrl, notificationMessage);
                }
                return ret;
            }
        }

        public static List<Watcher> GetWatchers(int userId)
        {
            List<Data.Watcher> watchers = null;
            using (var db = new Data.BiddingContext())
            {
                watchers = db.Watchers.Where(w => w.UserId == userId && w.IsActive).ToList();
            }
            var items = watchers.Select(i => Mapper.Map<Watcher>(i)).ToList();
            return items;
        }

        public static BiddingReturn AddOrUpdateWatcher(Watcher watcher)
        {
            BiddingReturn ret = new BiddingReturn()
            {
                Success = true
            };
            using (var db = new Data.BiddingContext())
            {
                var dwatcher = db.Watchers.FirstOrDefault(w => w.UserId == watcher.UserId && w.BiddingItemId == watcher.BiddingItemId);
                if (dwatcher != null)
                {
                    dwatcher.IsActive = watcher.IsActive;
                    dwatcher.UpdateDate = DateTime.Now;
                }
                else
                {
                    dwatcher = Mapper.Map<Data.Watcher>(watcher);
                    dwatcher.CreateDate = DateTime.Now;
                    db.Watchers.Add(dwatcher);
                }
                db.SaveChanges();
            }
            return ret;
        }


        public static NotificationAck GetNotificationAck(int userId)
        {
            Data.NotificationAck dack = null;
            using (var db = new Data.BiddingContext())
            {
                dack = db.NotificationAcks.FirstOrDefault(n => n.UserId == userId);
            }
            return Mapper.Map<NotificationAck>(dack);
        }

        public static BiddingReturn AddOrUpdateNotificationAck(NotificationAck notificationAck)
        {
            BiddingReturn ret = new BiddingReturn()
            {
                Success = true
            };
            using (var db = new Data.BiddingContext())
            {
                var dack = db.NotificationAcks.FirstOrDefault(w => w.UserId == notificationAck.UserId);
                if (dack != null)
                {
                    dack.LastAccessDate = notificationAck.LastAccessDate;
                }
                else
                {
                    dack = Mapper.Map<Data.NotificationAck>(notificationAck);
                    db.NotificationAcks.Add(dack);
                }
                db.SaveChanges();
            }
            return ret;
        }

        public static BiddingReturn AddNotification(long itemId, string imageUrl, string message, DateTime? eventTime = null)
        {
            BiddingReturn ret = new BiddingReturn()
            {
                Success = true
            };
            using (var db = new Data.BiddingContext())
            {
                var notification = new Data.Notification() { BiddingItemId = itemId, ImageUrl = imageUrl, Message = message, CreateDate = DateTime.Now, EventTime = eventTime.HasValue ? eventTime.Value : DateTime.Now };
                db.Notifications.Add(notification);
                db.SaveChanges();
            }
            return ret;
        }


        public static List<Notification> GetNotifications(int userId)
        {
            List<Data.Notification> notifications = null;
            using (var db = new Data.BiddingContext())
            {
                var dack = db.NotificationAcks.FirstOrDefault(n => n.UserId == userId);
                //find user's owned and participated and watched items
                var itemIds = db.BiddingItems.Where(b => b.OwnerId == userId || b.Actions.Any(a => a.BidderId == userId)).Select(b => b.BiddingItemId).ToList();
                itemIds.AddRange(db.Watchers.Where(w => w.UserId == userId && w.IsActive).Select(w => w.BiddingItemId));
                var query = db.Notifications.Where(n => (n.UserId.HasValue ? n.UserId == userId : false || itemIds.Contains(n.BiddingItemId)) && n.EventTime <= DateTime.Now);
                if (dack != null)
                    query = query.Where(n => n.EventTime >= dack.LastAccessDate);
                notifications = query.ToList();
            }
            var items = notifications.Select(i => Mapper.Map<Notification>(i)).ToList();
            return items;
        }

        public static User GetUser(string email, string password)
        {
            User user = null;
            using (var db = new Data.BiddingContext())
            {
                var modelUser = db.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower() && u.Password == password);
                if( modelUser != null)
                {
                    user = Mapper.Map<User>(modelUser);
                }
            }
            return user;
        }

        public static void ProcessStatusChange()
        {
            //STAGE => ACTIVE if start time is reached.
            using (var db = new Data.BiddingContext())
            {
                //find all the items with STAGE status and start time is passed
                var items = db.BiddingItems.Where(b => b.Status == BiddingItem.StagingStatus && b.Setting.StartDate <= DateTime.Now).ToList();
                foreach (var item in items)
                {
                    item.Status = BiddingItem.ActiveStatus;
                    var notification = new Data.Notification() { BiddingItemId = item.BiddingItemId, ImageUrl = item.ImageUrl, Message = string.Format("Item({0}) {1} is ready for bidding", item.BiddingItemId, item.Name), CreateDate = DateTime.Now, EventTime = DateTime.Now };
                    db.Notifications.Add(notification);
                }
                //find all the items with ACTIVE status and end time is passed
                items = db.BiddingItems.Where(b => b.Status == BiddingItem.ActiveStatus && b.Setting.EndDate <= DateTime.Now).ToList();
                foreach (var item in items)
                {
                    string newStatus = BiddingItem.FailStatus;
                    switch (item.Setting.Type)
                    {
                        case Data.BiddingType.HighWin:
                            if (item.Price >= item.Setting.AcceptPrice)
                                newStatus = BiddingItem.SuccessStatus;
                            break;
                        case Data.BiddingType.LowWin:
                            if (item.Price > 0 && item.Price <= item.Setting.AcceptPrice)
                                newStatus = BiddingItem.SuccessStatus;
                            break;
                    }

                    item.Status = newStatus;
                    string message = "";
                    if (newStatus == BiddingItem.SuccessStatus)
                    {
                        message = string.Format("Item {0} has a winning bid of {1}", item.Name, item.Price);
                    }
                    else
                    {
                        message = string.Format("Bidding for item {0} failed because no bid met the owner's reserved price", item.Name);
                    }
                    var notification = new Data.Notification() { BiddingItemId = item.BiddingItemId, ImageUrl = item.ImageUrl, Message = string.Format("Item({0}) {1} is ready for bidding", item.BiddingItemId, item.Name), CreateDate = DateTime.Now, EventTime = DateTime.Now };
                    db.Notifications.Add(notification);
                }
                db.SaveChanges();
            }

        }
    }
}
