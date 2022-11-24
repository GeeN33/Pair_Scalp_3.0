using System;
using System.Collections.Generic;
using System.Text;

namespace Pair_Scalp_2._0.Classes_binance
{
    public class Account
    {
        public string feeTier { get; set; }       // account commisssion tier 
        public bool canTrade { get; set; }   // if can trade
        public bool canDeposit { get; set; }     // if can transfer in asset
        public bool canWithdraw { get; set; }    // if can transfer out asset
        public string updateTime { get; set; }
        public string totalInitialMargin { get; set; }   // total initial margin required with current mark price (useless with isolated positions), only for USDT asset
        public string totalMaintMargin { get; set; }     // total maintenance margin required, only for USDT asset
        public string totalWalletBalance { get; set; }     // total wallet balance, only for USDT asset
        public string totalUnrealizedProfit { get; set; }  // total unrealized profit, only for USDT asset
        public string totalMarginBalance { get; set; }     // total margin balance, only for USDT asset
        public string totalPositionInitialMargin { get; set; }    // initial margin required for positions with current mark price, only for USDT asset
        public string totalOpenOrderInitialMargin { get; set; }   // initial margin required for open orders with current mark price, only for USDT asset
        public string totalCrossWalletBalance { get; set; }     // crossed wallet balance, only for USDT asset
        public string totalCrossUnPnl { get; set; }     // unrealized profit of crossed positions, only for USDT asset
        public string availableBalance { get; set; }       // available balance, only for USDT asset
        public string maxWithdrawAmount { get; set; }

        public Assets[] assets;

        public Positions[] positions;

    }
}
