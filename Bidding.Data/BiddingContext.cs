using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Data
{
    public class BiddingContext : DbContext
    {
        public BiddingContext():base("name=BiddingContext")
        {
        }
        public DbSet<BiddingItem> BiddingItems { get; set; }
        public DbSet<BiddingSetting> BiddingConfigs { get; set; }

        public DbSet<BiddingAction> BiddingActions { get; set; }

    }
}
