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
                .ForMember(dest => dest.GroupNames, opts => opts.MapFrom(src => src.Groups != null ? string.Join(",", src.Groups) : null));


                cfg.CreateMap<Data.BiddingAction, Bol.BiddingAction>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.BiddingActionId))
                .ForMember(dest => dest.ActionTime, opts => opts.MapFrom(src => src.TimeStamp))
                .ForMember(dest => dest.Bidder, opts => opts.MapFrom(src => new User() { Id = src.BidderId, Email = src.BidderEmail }));

                cfg.CreateMap<Bol.BiddingAction, Data.BiddingAction>()
                .ForMember(dest => dest.BiddingActionId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.TimeStamp, opts => opts.MapFrom(src => src.ActionTime))
                .ForMember(dest => dest.BidderId, opts => opts.MapFrom(src => src.Bidder != null ? src.Bidder.Id : 0))
                .ForMember(dest => dest.BidderEmail, opts => opts.MapFrom(src => src.Bidder != null ? src.Bidder.Email : null));

            });

        }

        //public static CreateBidding()
        //{

        //}
        public static BiddingItem GetItem(int itemId)
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
            return Mapper.Map<BiddingItem>(bidItem);
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

        public static void AddAction(int itemId, BiddingAction action)
        {
            var dAction = Mapper.Map<Data.BiddingAction>(action);
            using (var db = new Data.BiddingContext())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var item = db.BiddingItems.Find(itemId);
                        if (item != null)
                        {
                            item.Actions.Add(dAction);
                            db.SaveChanges();
                            dbContextTransaction.Commit();
                            action.Id = dAction.BiddingActionId;
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public static void AddAction(BiddingAction action)
        {
            var dAction = Mapper.Map<Data.BiddingAction>(action);
            using (var db = new Data.BiddingContext())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var item = db.BiddingItems.Find(action.ItemId);
                        if (item != null)
                        {
                            item.Actions.Add(dAction);
                            db.SaveChanges();
                            dbContextTransaction.Commit();

                            action.Id = dAction.BiddingActionId;
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
