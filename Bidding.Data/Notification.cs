using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Data
{
    public class Notification
    {
        public long NotificationId { get; set; }
        public long BiddingItemId { get; set; }
        public int? UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EventTime { get; set; }
    }
}
