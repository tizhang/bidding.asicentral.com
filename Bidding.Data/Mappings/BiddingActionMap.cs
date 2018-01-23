using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Data.Mappings
{
    public class BiddingActionMap: EntityTypeConfiguration<BiddingAction>
    {
        public BiddingActionMap()
        {
            ToTable("BiddingActions");
            HasKey(t => t.BiddingActionId);

            Property(t => t.BiddingActionId)
                .HasColumnName("BiddingActionId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.ItemId)
                            .HasColumnName("BiddingItem_BiddingItemId");

            HasRequired(action => action.Item)
                            .WithMany(item => item.Actions)
                            .HasForeignKey( item => item.ItemId )
                            .WillCascadeOnDelete();
        }
    }
}
