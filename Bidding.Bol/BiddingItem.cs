using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Bol
{

    public class BiddingItem
    {
        //owner can change item in this state
        public const string DraftStatus = "DRAF";
        //user can view item in this state
        public const string StagingStatus = "STAG";
        //user can bid item in this state
        public const string ActiveStatus = "ACTV";
        //bid item in success state
        public const string SuccessStatus = "SUCC";
        //bid item in failure state
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
