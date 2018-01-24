using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Bol
{
    public class Notification
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; }
        public long BiddingItemId { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EventTime { get; set; }
    }
}
