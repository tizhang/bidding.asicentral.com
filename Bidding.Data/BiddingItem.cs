using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Data
{
    public class BiddingItem
    {
        public long BiddingItemId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string ImageUrl
        {
            get;
            set;
        }

        public int BidTimes
        {
            get;
            set;
        }

        public double Price
        {
            get;
            set;
        }

        public int OwnerId
        {
            get;
            set;
        }

        public string OwnerEmail
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public DateTime CreateDate
        {
            get;
            set;
        }

        public virtual BiddingSetting Setting
        {
            get;
            set;
        }

        public virtual User User { get; set; }
        public virtual List<BiddingAction> Actions
        {
            get;
            set;
        }

        public virtual List<Watcher> Watchers { get; set; }
    }
}
