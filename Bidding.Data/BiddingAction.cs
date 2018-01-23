using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Data
{
    public class BiddingAction
    {

        public long BiddingActionId
        {
            get;
            set;
        }

        public long ItemId { get; set; }

        public DateTime TimeStamp
        {
            get;
            set;
        }

        public double Price
        {
            get;
            set;
        }

        public string Comment
        {
            get;
            set;
        }

        public int BidderId
        {
            get;
            set;
        }

        public string BidderEmail
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public virtual BiddingItem Item { get; set; }
    }
}
