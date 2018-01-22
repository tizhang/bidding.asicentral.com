using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Bol
{
    public class BiddingSetting
    {
        public long Id {
            get;
            set;
        }

        public List<string> Groups
        {
            get;
            set;
        }

        public double MinIncrement
        {
            get;
            set;
        }
        
        public bool ShowOwner
        {
            get;
            set;
        }
        
        public bool ShowCurrentPrice
        {
            get;
            set;
        }

        public int? BidTimePerUser
        {
            get;
            set;
        }

        public double AcceptPrice
        {
            get;
            set;
        }

        public double StartPrice
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get;
            set;
        }

        public DateTime StartDate
        {
            get;
            set;
        }

    }
}
