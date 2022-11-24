using System;
using System.Collections.Generic;
using System.Text;

namespace Pair_Scalp_2._0.Classes_binance
{
    public class Assets
    {
        public string asset { get; set; }            // asset name
        public string walletBalance { get; set; }      // wallet balance
        public string unrealizedProfit { get; set; }   // unrealized profit
        public string marginBalance { get; set; }      // margin balance
        public string maintMargin { get; set; }        // maintenance margin required
        public string initialMargin { get; set; }    // total initial margin required with current mark price 
        public string positionInitialMargin { get; set; }    //initial margin required for positions with current mark price
        public string openOrderInitialMargin { get; set; }   // initial margin required for open orders with current mark price
        public string crossWalletBalance { get; set; }     // crossed wallet balance
        public string crossUnPnl { get; set; }      // unrealized profit of crossed positions
        public string availableBalance { get; set; }       // available balance
        public string maxWithdrawAmount { get; set; }     // maximum amount for transfer out
        public bool marginAvailable { get; set; }   // whether the asset can be used as margin in Multi-Assets mode
        public long updateTime { get; set; } // last update time 

    }
}
