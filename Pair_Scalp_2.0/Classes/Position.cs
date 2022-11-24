using System;
using System.Collections.Generic;
using System.Text;

namespace Pair_Scalp_2._0.Classes
{
    public class Position
    {
        public string symbol { get; set; }    // symbol name
        public double Profit { get; set; }  // unrealized profit
        public double entryPrice { get; set; }    // average entry price
        public double positionAmt { get; set; }         // position amount
    }
}
