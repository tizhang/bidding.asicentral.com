using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Bol
{

    public class BiddingItem
    {

        public const string PendingStatus = "PEND";
        public const string ActiveStatus = "ACTV";
        public const string FinalStatus = "FINL";
        public const string FailStatus = "FAIL";

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

        public string Description
        {
            get;
            set;
        }

        public string ImageUrl
        {
            get;
            set;
        }

        public int BidTimes
        {
            get;
            set;
        }

        public double Price
        {
            get;
            set;
        }

        public User Owner
        { 
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public DateTime CreateDate
        {
            get;
            set;
        }

        public BiddingSetting Setting
        {
            get;
            set;
        }

        public List<BiddingAction> History
        {
            get;
            set;
        }

    }


}
