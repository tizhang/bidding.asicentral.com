using System.Data.Entity.ModelConfiguration;

namespace Bidding.Data.Mappings
{
    public class BiddingItemMap : EntityTypeConfiguration<BiddingItem>
    {
        public BiddingItemMap()
        {
            this.ToTable("BiddingItems");
            this.HasKey(t => t.BiddingItemId);
        }
    }
}
