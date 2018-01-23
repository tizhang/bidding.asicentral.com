using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Bol
{
    public class User
    {
        public long Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string Groups { get; set; }

        public List<BiddingItem> PostedItems { get; set; }
        public List<BiddingAction> BidActions { get; set; }
        public List<Watcher> WatchItems { get; set; }
    }
}
