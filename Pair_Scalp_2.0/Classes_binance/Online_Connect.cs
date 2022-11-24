using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Pair_Scalp_2._0.Classes_binance
{
    public static class Online_Connect
    {
       static string request = "";

        public static void online_connect(ref bool fff)
        {
            fff = false;

            request = GET("https://fapi.binance.com/fapi/v1/time");

            if (request != "исключения")
            {

                fff = true;


            }

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
