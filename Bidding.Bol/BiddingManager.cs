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


        public static void CreateItem(BiddingItem item)
        {
            var dItem = Mapper.Map<Data.BiddingItem>(item);
            using (var db = new Data.BiddingContext())
            {
                db.BiddingItems.Add(dItem);
                db.SaveChanges();
            }
        }

        public static BiddingReturn AddAction(int itemId, BiddingAction action)
        {
            BiddingReturn ret = new BiddingReturn()
            {
                Success = true
            }; 
            var dAction = Mapper.Map<Data.BiddingAction>(action);
            using (var db = new Data.BiddingContext())
            {
                var item = db.BiddingItems
                         .Include("Setting")
                         .Include("Actions")
                         .FirstOrDefault(i => i.BiddingItemId == itemId);
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
                            var nextPrice = lastAction.Price - setting.MinIncrement;
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
                        if (setting.AcceptMinPrice > 0 && action.Price < setting.AcceptMinPrice)
                        {
                            ret = new BiddingReturn()
                            {
                                Success = false,
                                Message = string.Format("Bidding Price is lower than the minimum price: {0}", setting.AcceptMinPrice)
                            };
                        }
                    }
                    bool skipSave = false;
                    if (!ret.Success)
                    {
                        var myLastOne = item.Actions.Where(a => a.BidderId == dAction.BidderId).OrderBy(i => i.TimeStamp).LastOrDefault();
                        if (myLastOne != null)
                        {
                            if (setting.Type == Data.BiddingType.HighWin && myLastOne.Price >= dAction.Price)
                            {
                                skipSave = true;
                            }
                            else if (setting.Type == Data.BiddingType.LowWin && myLastOne.Price <= dAction.Price)
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
                    }
                }
                
                return ret;
            }
        }

    }
}
