using System;
using System.Collections.Generic;
using System.Text;

namespace Pair_Scalp_2._0.Classes_binance
{
    public class Order
    {
        public string orderId { get; set; }
        public string symbol { get; set; }
        public string status { get; set; }
        public string clientOrderId { get; set; }
        public string price { get; set; }
        public string avgPrice { get; set; }
        public string origQty { get; set; }
        public string executedQty { get; set; }
        public string cumQty { get; set; }
        public string cumQuote { get; set; }
        public string timeInForce { get; set; }
        public string type { get; set; }
        public string reduceOnly { get; set; }
        public string closePosition { get; set; }
        public string side { get; set; }
        public string positionSide { get; set; }
        public string stopPrice { get; set; }
        public string workingType { get; set; }
        public string priceProtect { get; set; }
        public string origType { get; set; }
        public string updateTime { get; set; }

    }
}
