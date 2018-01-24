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

        static BiddingManager()
        {
            Initialize();
        }

        public static void Initialize()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Data.BiddingItem, Bol.BiddingItem>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.BiddingItemId))
                .ForMember(dest => dest.Owner, opts => opts.MapFrom(src => new User() { Id = src.OwnerId, Email = src.OwnerEmail }))
                .ForMember(dest => dest.History, opts => opts.MapFrom(src => src.Actions));

                cfg.CreateMap<Bol.BiddingItem, Data.BiddingItem>()
                .ForMember(dest => dest.BiddingItemId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.OwnerId, opts => opts.MapFrom(src => src.Owner != null ? src.Owner.Id : 0))
                .ForMember(dest => dest.OwnerEmail, opts => opts.MapFrom(src => src.Owner != null ? src.Owner.Email : null))
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
                .ForMember(dest => dest.Bidder, opts => opts.MapFrom(src => new User() { Id = src.BidderId, Email = src.BidderEmail }));

                cfg.CreateMap<Bol.BiddingAction, Data.BiddingAction>()
                .ForMember(dest => dest.BiddingActionId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.TimeStamp, opts => opts.MapFrom(src => src.ActionTime))
                .ForMember(dest => dest.BidderId, opts => opts.MapFrom(src => src.Bidder != null ? src.Bidder.Id : 0))
                .ForMember(dest => dest.BidderEmail, opts => opts.MapFrom(src => src.Bidder != null ? src.Bidder.Email : null));

                cfg.CreateMap<Bol.User, Data.User>()
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
                .ForMember(dest => dest.Groups, opts => opts.MapFrom(src => src.Groups));

                cfg.CreateMap<Data.User, Bol.User>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
                .ForMember(dest => dest.Groups, opts => opts.MapFrom(src => src.Groups));

               // cfg.CreateMap<Bol.Watcher, Data.Watcher>()
               // .ForMember(dest => dest.WatcherId, opts => opts.MapFrom(src => src.WatcherId))
               // .ForMember(dest => dest.BiddingItemId, opts => opts.MapFrom(src => src.BiddingItem != null ? src.BiddingItem.Id : 0))
               // .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserId))
               // .ForMember(dest => dest.IsActive, opts => opts.MapFrom(src => src.IsActive));

               // cfg.CreateMap<Data.Watcher, Bol.Watcher>()
               //.ForMember(dest => dest.WatcherId, opts => opts.MapFrom(src => src.WatcherId))
               //.ForMember(dest => dest.ItemId, opts => opts.MapFrom(src => src.BiddingItem != null ? src.BiddingItem.BiddingItemId : 0))
               //.ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserId))
               //.ForMember(dest => dest.IsActive, opts => opts.MapFrom(src => src.IsActive));
            });
        }

        //public static CreateBidding()
        //{


        //}
        
        public static List<BiddingItem> GetItems(string group = null, string status = null, bool includeFailedActions = true)
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
                                    if (action.Price <= nextPrice)
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
                                    if (action.Price >= nextPrice)
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
                                if (setting.AcceptPrice > 0)
                                {
                                    if (setting.Type == Data.BiddingType.HighWin && action.Price < setting.AcceptPrice)
                                    {
                                        ret = new BiddingReturn()
                                        {
                                            Success = false,
                                            Message = string.Format("Bidding price is lower than the minimum accepted price: {0}", setting.AcceptPrice)
                                        };
                                    }
                                    else if (setting.Type == Data.BiddingType.LowWin && action.Price > setting.AcceptPrice)
                                    {
                                        ret = new BiddingReturn()
                                        {
                                            Success = false,
                                            Message = string.Format("Bidding price is higher than the maximum accepted price: {0}", setting.AcceptPrice)
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
                return ret;
            }
        }

        #region Watch list
        public static List<Watcher> GetWatcher(int userId)
        {
            var watchList = new List<Watcher>();
            try
            {
                using (var context = new Data.BiddingContext())
                {
                    var watchers = context.Watchers.Where(w => w.UserId == userId && w.IsActive );
                    if( watchers != null && watchers.Count() > 0 )
                    {
                        foreach(var watcher in watchers)
                        {
                            watchList.Add(new Watcher() { WatcherId = watcher.WatcherId, ItemId = watcher.BiddingItemId, UserId = watcher.UserId, IsActive = watcher.IsActive});
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            return watchList;
        }

        public static Watcher AddWatcher(Watcher watcher)
        {
            using (var context = new Data.BiddingContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var modelWatcher = context.Watchers.FirstOrDefault(w => w.UserId == watcher.UserId && w.BiddingItemId == watcher.ItemId);
                        if (modelWatcher == null)
                        {
                            modelWatcher = new Data.Watcher()
                            {
                                BiddingItemId = watcher.ItemId,
                                UserId = watcher.UserId,
                                IsActive = watcher.IsActive,
                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now
                            };
                            context.Watchers.Add(modelWatcher);
                        }
                        else
                        {
                            modelWatcher.IsActive = watcher.IsActive;
                        }

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        watcher.WatcherId = modelWatcher.WatcherId;
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }

            return watcher;
       }
       #endregion Watch List

    }
}
