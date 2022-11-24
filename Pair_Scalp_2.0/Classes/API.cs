using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Pair_Scalp_2._0.Classes
{
    public class API
    {
        string request = "";

        public class Pices
        {
            public string symbol { get; set; }
            public double price { get; set; }

        }

        public void binance_klines(string symbol, string interval, ref List<bars_Class> bars)
        {

            //   request = GET("https://api.binance.com/api/v1/klines?symbol=" + symbol + "&interval=" + interval + "&limit=240");

            request = GET("https://fapi.binance.com/fapi/v1/klines?symbol=" + symbol + "&interval=" + interval + "&limit=180");


            if (request != "исключения")
            {
                bars.Clear();
                var Data = JsonConvert.DeserializeObject<double[][]>(request);

                for (int i = 0; i < Data.Length - 1; i++)
                {

                    bars.Add(new bars_Class(Data[i][0], Data[i][1], Data[i][2], Data[i][3], Data[i][4], Data[i][5]));


                }


            }


        }

        public double price_last(string symbol)
        {

            string request = null;

           //  request = GET("https://api.binance.com/api/v3/ticker/price?symbol=" + symbol);
            request = GET("https://fapi.binance.com/fapi/v1/ticker/price?symbol=" + symbol);

            if (request != "исключения")
            {
                var Data = JsonConvert.DeserializeObject<Pices>(request);
                return Data.price;
            }



            return 0;

        }

        private static string GET(string Url)
        {
            try
            {
                System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.Stream stream = resp.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(stream);
                string Out = sr.ReadToEnd();
                sr.Close();
                return Out;
            }
            catch (Exception)
            {
                // MessageBox.Show("Сообщение", "Поверх всех окон", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);

                // Console.WriteLine("Ошибка: " + ex.Message);
                return "исключения"; ;
            }
            finally
            {
                // Console.WriteLine("Блок finally");

            }

        }

    }
}
