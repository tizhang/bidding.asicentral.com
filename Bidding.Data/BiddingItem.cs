using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bidding.Data
{
    public class BiddingItem
    {
        public long BiddingItemId
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

        public virtual BiddingConfig Config
        {
            get;
            set;
        }

        public virtual List<BiddingAction> Actions
        {
            get;
            set;
        }

    }
}
