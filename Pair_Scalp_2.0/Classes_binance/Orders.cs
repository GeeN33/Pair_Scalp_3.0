using System;
using System.Collections.Generic;
using System.Text;

namespace Pair_Scalp_2._0.Classes_binance
{
    public class Orders
    {
        public string avgPrice { get; set; }
        public string clientOrderId { get; set; }
        public string cumQuote { get; set; }
        public string executedQty { get; set; }
        public string orderId { get; set; }
        public string origQty { get; set; }
        public string origType { get; set; }
        public string price { get; set; }
        public string reduceOnly { get; set; }
        public string side { get; set; }
        public string positionSide { get; set; }
        public string status { get; set; }
        public string stopPrice { get; set; }            // please ignore when order type is TRAILING_STOP_MARKET
        public string closePosition { get; set; }  // if Close-All
        public string symbol { get; set; }
        public string time { get; set; }           // order time
        public string timeInForce { get; set; }
        public string type { get; set; }
        public string activatePrice { get; set; }      // activation price, only return with TRAILING_STOP_MARKET order
        public string priceRate { get; set; }               // callback rate, only return with TRAILING_STOP_MARKET order
        public string updateTime { get; set; }       // update time
        public string workingType { get; set; }
        public string priceProtect { get; set; }

    }
}
