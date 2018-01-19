using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Data
{
    public enum BiddingType { HighWin = 0, LowWin = 1}

    public class BiddingConfig
    {
        public long BiddingConfigId {
            get;
            set;
        }

        double minIncrement = 5;
        public double MinIncrement
        {
            get
            {
                return minIncrement;
            }
            set
            {
                minIncrement = value;
            }
        }
        
        public bool Blind
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

        public BiddingType Type
        {
            get;
            set;
        }
    }
}
