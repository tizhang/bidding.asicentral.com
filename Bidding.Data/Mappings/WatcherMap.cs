using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Data.Mappings
{
    public class WatcherMap : EntityTypeConfiguration<Watcher>
    {
        public WatcherMap()
        {
            this.ToTable("Watchers");
            this.HasKey(t => t.WatcherId);

            HasRequired(w => w.User)
                .WithMany(user => user.WatchItems)
                .HasForeignKey(w => w.UserId )
                .WillCascadeOnDelete();

            HasRequired(w => w.BiddingItem)
               .WithMany(item => item.Watchers)
               .HasForeignKey(w => w.BiddingItemId)
               .WillCascadeOnDelete();
        }
    }
}
