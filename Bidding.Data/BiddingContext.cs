using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bidding.Data.Mappings;

namespace Bidding.Data
{
    public class BiddingContext : DbContext
    {
        public BiddingContext():base("name=BiddingContext")
        {
            //disable initializer
            Database.SetInitializer<BiddingContext>(null);
        }
        public DbSet<BiddingItem> BiddingItems { get; set; }
        public DbSet<BiddingSetting> BiddingConfigs { get; set; }

        public DbSet<BiddingAction> BiddingActions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Configuration.LazyLoadingEnabled = true;
            modelBuilder.Configurations
                .Add(new BiddingActionMap())
                .Add(new BiddingItemMap())
                .Add(new BiddingSettingMap());
           ;

        }
    }
}
