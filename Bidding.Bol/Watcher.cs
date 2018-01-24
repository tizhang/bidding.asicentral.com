using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Bol
{
    public class Watcher
    {
        public long Id { get; set; }
        public long BiddingItemId { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }

    }
}
