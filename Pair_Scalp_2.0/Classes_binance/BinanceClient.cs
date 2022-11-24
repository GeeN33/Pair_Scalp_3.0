using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Net.Http;

namespace Pair_Scalp_2._0.Classes_binance
{
    public static class BinanceClient
    {
        static HttpClient _httpClient;
        private static string key = "***************************************";
        private static string secret = "**************************************************";

        #region // Объявляем делегат

        public delegate void delegAccount(Account account); // 1. Объявляем делегат
        public static event delegAccount evenAccount;

        public delegate void delegBalance(List<Balance> balance); // 1. Объявляем делегат
        public static event delegBalance evenBalance;


        public delegate void delegOrder(Order order); // 1. Объявляем делегат
        public static event delegOrder evenOrder;

        public delegate void delegError(string error); // 1. Объявляем делегат
        public static event delegError evenError;
        #endregion

        public static void start()
        {

            string url = "https://fapi.binance.com/fapi/";

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(url)

            };
            _httpClient.DefaultRequestHeaders
                 .Add("X-MBX-APIKEY", key);

            _httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        }

        private static string GetTimestamp()
        {
            long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            return milliseconds.ToString();
        }

        public static void PlaceBuyOrderMarket(string symbol, double quantity, double price, string type = "MARKET")
        {
            PostSigned("v1/order", "symbol=" + symbol + "&" + "side=BUY" + "&" + "type=" + type + "&" + "quantity=" + quantity.ToString() + "&" + "recvWindow=6000");

        }

        public static void PlaceSellOrderMarket(string symbol, double quantity, double price, string type = "MARKET")
        {
            PostSigned("v1/order", "symbol=" + symbol + "&" + "side=SELL" + "&" + "type=" + type + "&" + "quantity=" + quantity.ToString() + "&" + "recvWindow=6000");

        }

        public static void PlaceBuyOrder(string symbol, double quantity, double price, string type = "LIMIT")
        {
            PostSigned("v1/order", "symbol=" + symbol + "&" + "side=BUY" + "&" + "type=" + type + "&" + "quantity=" + quantity.ToString() + "&" + "price=" + price.ToString() + "&" + "timeInForce=GTC" + "&" + "recvWindow=6000");

        }

        public static void CancelOrder(string symbol, string orderId)
        {
            DeleteSigned("v1/order", "symbol=" + symbol + "&" + "orderId=" + orderId);

        }

        public static async void GetBalance(string endpoint = "v2/balance", string args = null)
        {
            string headers = _httpClient.DefaultRequestHeaders.ToString();
            string timestamp = GetTimestamp();
            args += "&timestamp=" + timestamp;

            var signature = CreateSignature(args, secret);

            var response = await _httpClient.GetAsync($"{endpoint}?{args}&signature={signature}");

            if (response.Content != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                balance_save(responseContent);
            }

        }

        public static async void PostSigned(string endpoint, string args = null)
        {
            string headers = _httpClient.DefaultRequestHeaders.ToString();

            string timestamp = GetTimestamp();

            args += "&timestamp=" + timestamp;


            var signature = CreateSignature(args, secret);


            var response = await _httpClient.PostAsync($"{endpoint}?{args}&signature={signature}", null);

            if (response.Content != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                order_save(responseContent);

            }

        }

        public static async void DeleteSigned(string endpoint, string args = null)
        {
            string headers = _httpClient.DefaultRequestHeaders.ToString();
            string timestamp = GetTimestamp();
            args += "&timestamp=" + timestamp;

            var signature = CreateSignature(args, secret);

            var response = await _httpClient.DeleteAsync($"{endpoint}?{args}&signature={signature}");

            if (response.Content != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

            }
        }

        public static async void  GetAccount(string endpoint = "v2/account", string args = null)
        {
            string headers = _httpClient.DefaultRequestHeaders.ToString();
            string timestamp = GetTimestamp();
            args += "&timestamp=" + timestamp;

            var signature = CreateSignature(args, secret);

            var response = await _httpClient.GetAsync($"{endpoint}?{args}&signature={signature}");

            if (response.Content != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                account_save(responseContent);
            }

            
        }

        public static async void GetOpenOrders(string endpoint = "v1/openOrders", string args = null)
        {
            string headers = _httpClient.DefaultRequestHeaders.ToString();
            string timestamp = GetTimestamp();
            args += "&timestamp=" + timestamp;

            var signature = CreateSignature(args, secret);

            var response = await _httpClient.GetAsync($"{endpoint}?{args}&signature={signature}");

            if (response.Content != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
             
            }
        }


        private static readonly Encoding SignatureEncoding = Encoding.UTF8;

        static string CreateSignature(string message, string secret)
        {

            byte[] keyBytes = SignatureEncoding.GetBytes(secret);
            byte[] messageBytes = SignatureEncoding.GetBytes(message);
            HMACSHA256 hmacsha256 = new HMACSHA256(keyBytes);

            byte[] bytes = hmacsha256.ComputeHash(messageBytes);

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        static void order_save(string res)
        {
            try
            {              
                var Data = JsonConvert.DeserializeObject<Order>(res);

                evenOrder?.Invoke(Data);
            }
            catch { evenError?.Invoke(res); }

        }

        static void balance_save(string res)
        {
            try
            {

                var Data = JsonConvert.DeserializeObject<List<Balance>>(res);

                evenBalance?.Invoke(Data);
            }
            catch {  }

        }

        static void account_save(string res)
        {
            try
            {               

                var Data = JsonConvert.DeserializeObject<Account>(res);

                evenAccount?.Invoke(Data);

            }
            catch {  }

        }

    }
}
