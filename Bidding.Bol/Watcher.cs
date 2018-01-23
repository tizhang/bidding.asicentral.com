namespace Bidding.Bol
{
    public class Watcher
    {
        public int WatcherId { get; set; }
        public long BiddingItemId { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }

        public User User { get; set; }
        public BiddingItem BiddingItem { get; set; }
    }
}
