using System;

namespace Bidding.Data
{
    public class Watcher
    {
        public long WatcherId { get; set; }
        public long BiddingItemId { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual User User { get; set; }
        public virtual BiddingItem BiddingItem { get; set; }
    }
}
