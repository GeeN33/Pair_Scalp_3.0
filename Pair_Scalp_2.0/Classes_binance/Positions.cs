using System;
using System.Collections.Generic;
using System.Text;

namespace Pair_Scalp_2._0.Classes_binance
{
    public class Positions
    {
        public string symbol { get; set; }    // symbol name
        public string initialMargin { get; set; }   // initial margin required with current mark price 
        public string maintMargin { get; set; }     // maintenance margin required
        public string unrealizedProfit { get; set; }  // unrealized profit
        public string positionInitialMargin { get; set; }      // initial margin required for positions with current mark price
        public string openOrderInitialMargin { get; set; }     // initial margin required for open orders with current mark price
        public string leverage { get; set; }      // current initial leverage
        public string isolated { get; set; }      // if the position is isolated
        public string entryPrice { get; set; }    // average entry price
        public string maxNotional { get; set; }    // maximum available notional with current leverage
        public string bidNotional { get; set; } // bids notional, ignore
        public string askNotional { get; set; } // ask norional, ignore
        public string positionSide { get; set; }     // position side
        public string positionAmt { get; set; }         // position amount
        public string updateTime { get; set; }
    }
}
