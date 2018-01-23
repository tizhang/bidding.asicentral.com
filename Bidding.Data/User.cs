﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Data
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Groups { get; set; }
        public string Password { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual List<BiddingItem> PostedItems { get; set; }
        public virtual List<BiddingAction> BidActions { get; set; }

        public virtual List<Watcher> WatchItems { get; set; }
    }
}
