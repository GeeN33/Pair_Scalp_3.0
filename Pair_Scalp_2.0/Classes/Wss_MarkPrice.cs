using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using Newtonsoft.Json;

namespace Pair_Scalp_2._0.Classes
{
    public class Wss_MarkPrice
    {
        public delegate void delegwsstick(mark_Prices mark_prices); // 1. Объявляем делегат      
        public event delegwsstick evenWssTick;
        public bool FF=false;
        WebSocket ws_tick;
        public void Socket_tick()
        {
            // string Request = "wss://stream.binance.com:9443/stream?streams=btcusdt@aggTrade";
            string Request = "wss://fstream.binance.com/stream?streams=!markPrice@arr";
            ws_tick = new WebSocket(Request);

            ws_tick.OnMessage += Up_tick;


            try
            {
                ws_tick.Connect();

            }
            catch (Exception)
            {
                Re_Socket_tick();
            }
            finally
            {
                // ("Блок finally");

            }
        }

        public void Up_tick(object sender, MessageEventArgs e)
        {
            mark_Prices Tick = JsonConvert.DeserializeObject<mark_Prices>(e.Data);

            evenWssTick?.Invoke(Tick);

            FF = false;
        }

        public void Re_Socket_tick()
        {
            if (ws_tick != null) ws_tick.Close();
            ws_tick.Connect();
        }

        public void ws_Closing()
        {
            if (ws_tick != null) ws_tick.Close();
        }

    }
}
