namespace Bidding.Bol
{
    public class Watcher
    {
        public long WatcherId { get; set; }
        public long ItemId { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }

        public User User { get; set; }
        public BiddingItem BiddingItem { get; set; }
    }
}
