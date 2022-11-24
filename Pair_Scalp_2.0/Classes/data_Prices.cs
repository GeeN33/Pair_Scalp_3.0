using System;
using System.Collections.Generic;
using System.Text;

namespace Pair_Scalp_2._0.Classes
{
    public class data_Prices
    {
        public string e { get; set; }    // Event type
        public string E { get; set; }          // Event time
        public string s { get; set; }         // Symbol
        public string p { get; set; }       // Mark price
        public string i { get; set; }       // Index price
        public string P { get; set; }      // Estimated Settle Price, only useful in the last hour before the settlement starts
        public string r { get; set; }           // Funding rate
        public string T { get; set; }

    }
}
