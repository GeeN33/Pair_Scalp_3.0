using System;
using System.Collections.Generic;
using System.Text;

namespace Pair_Scalp_2._0.Classes_binance
{
    public class Balance
    {
        public string accountAlias { get; set; }    // unique account code
        public string asset { get; set; }     // asset name
        public string balance { get; set; }  // wallet balance
        public string crossWalletBalance { get; set; }  // crossed wallet balance
        public string crossUnPnl { get; set; }   // unrealized profit of crossed positions
        public string availableBalance { get; set; }        // available balance
        public string maxWithdrawAmount { get; set; }     // maximum amount for transfer out
        public string marginAvailable { get; set; }   // whether the asset can be used as margin in Multi-Assets mode
        public string updateTime { get; set; }

    }
}
