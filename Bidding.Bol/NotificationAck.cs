using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Bol
{
    public class NotificationAck
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public DateTime LastAccessDate { get; set; }
    }
}
