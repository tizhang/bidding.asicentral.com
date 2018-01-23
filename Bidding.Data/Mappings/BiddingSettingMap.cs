using System.Data.Entity.ModelConfiguration;

namespace Bidding.Data.Mappings
{
    public class BiddingSettingMap : EntityTypeConfiguration<BiddingSetting>
    {
        public BiddingSettingMap()
        {
            this.ToTable("BiddingSettings");
            this.HasKey(t => t.BiddingSettingId);
        }
    }
}
