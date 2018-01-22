using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Bol
{
    public class BiddingAction
    {
        public const string SuccessStatus = "GOOD";
        public const string FailStatus = "FAIL";

        public long Id
        {
            get;
            set;
        }

        public long ItemId { get; set; }

        public DateTime ActionTime
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

        public User Bidder
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

    }
}
