using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Pair_Scalp_2._0.Classes_binance;

namespace Pair_Scalp_2._0.Classes
{
    public class Order_work
    {
        public Pair_work pair_Work;
        public int Id = 0;         
        public bool order_worked = false;
        public bool selective = false;
        public bool order_wd = false;
        public string type = "";
        public double price = 0;
        public string status = "";
        public int e_Y = 0;
        public Order_work(int Id, Pair_work pair_Work, double price, string type, int e_Y)
        {
            this.Id = Id;
            this.pair_Work = pair_Work;
            this.price = price;
            this.type = type;
            order_worked = true;
            this.e_Y = e_Y;
        }
        public void order_execute(double price_last)
        {

            if (pair_Work.trade && order_worked)
            {     
                if (type == "B_LT")
                {
                   if (price_last <= price)
                    {

                        string str = DateTime.Now.ToString() + "- Id " + Id.ToString() + ". B_LT " + pair_Work.Symbol1 + "_" + pair_Work.Symbol2 + "_" + pair_Work.lot1.ToString() + "_" + pair_Work.lot2.ToString();
                        My_Telegram.Send_Message(str);

                        order_worked = false;
                        order_wd = true;

                               BinanceClient.PlaceSellOrderMarket(pair_Work.Symbol1, pair_Work.lot1, pair_Work.Price_1);
                               Thread.Sleep(100);
                               BinanceClient.PlaceBuyOrderMarket(pair_Work.Symbol2, pair_Work.lot2, pair_Work.Price_2);
                               Thread.Sleep(300);
                               BinanceClient.GetAccount();
                    }

                }

                if (type == "S_LT")
                {
                    if (price_last >= price)
                    {

                        string str = DateTime.Now.ToString() + "- Id " + Id.ToString() + ". S_LT " + pair_Work.Symbol1 + "_" + pair_Work.Symbol2 + "_" + pair_Work.lot1.ToString() + "_" + pair_Work.lot2.ToString();
                        My_Telegram.Send_Message(str);

                        order_worked = false;
                        order_wd = true;

                        BinanceClient.PlaceBuyOrderMarket(pair_Work.Symbol1, pair_Work.lot1, pair_Work.Price_1);
                        Thread.Sleep(100);
                        BinanceClient.PlaceSellOrderMarket(pair_Work.Symbol2, pair_Work.lot2, pair_Work.Price_2);
                        Thread.Sleep(300);
                        BinanceClient.GetAccount();
                    }

                }

                if (pair_Work.entry_Price.Buy_Sell == "B")
                {
                    if (type == "S_SL")
                    {
                        if (price_last <= price)
                        {

                            string str = DateTime.Now.ToString() + "- Id " + Id.ToString() + ". S_SL " + pair_Work.Symbol1 + "_" + pair_Work.Symbol2 + "_" + pair_Work.lot_sam_1.ToString() + "_" + pair_Work.lot_sam_2.ToString() + " Profit: " + pair_Work.Profit2.ToString("0.###");
                            My_Telegram.Send_Message(str);

                            order_worked = false;
                            order_wd = true;


                            BinanceClient.PlaceBuyOrderMarket(pair_Work.Symbol1, pair_Work.lot_sam_1, pair_Work.Price_1);
                            Thread.Sleep(100);
                            BinanceClient.PlaceSellOrderMarket(pair_Work.Symbol2, pair_Work.lot_sam_2, pair_Work.Price_2);
                            Thread.Sleep(300);
                            BinanceClient.GetAccount();
                        }

                    }

                    if (type == "S_TP")
                    {
                        if (price_last >= price)
                        {

                            string str = DateTime.Now.ToString() + "- Id " + Id.ToString() + ". S_TP " + pair_Work.Symbol1 + "_" + pair_Work.Symbol2 + "_" + pair_Work.lot_sam_1.ToString() + "_" + pair_Work.lot_sam_2.ToString() + " Profit: " + pair_Work.Profit2.ToString("0.###");
                            My_Telegram.Send_Message(str);

                            order_worked = false;
                            order_wd = true;

                            BinanceClient.PlaceBuyOrderMarket(pair_Work.Symbol1, pair_Work.lot_sam_1, pair_Work.Price_1);
                            Thread.Sleep(100);
                            BinanceClient.PlaceSellOrderMarket(pair_Work.Symbol2, pair_Work.lot_sam_2, pair_Work.Price_2);
                            Thread.Sleep(300);
                            BinanceClient.GetAccount();
                        }

                    }
                }

                if (pair_Work.entry_Price.Buy_Sell == "S") 
                {
                    if (type == "B_SL")
                    {
                        if (price_last >= price)
                        {

                            string str = DateTime.Now.ToString() + "- Id " + Id.ToString() + ". B_SL " + pair_Work.Symbol1 + "_" + pair_Work.Symbol2 + "_" + pair_Work.lot_sam_1.ToString() + "_" + pair_Work.lot_sam_2.ToString() + " Profit: " + pair_Work.Profit2.ToString("0.###");
                            My_Telegram.Send_Message(str);

                            order_worked = false;
                            order_wd = true;

                            BinanceClient.PlaceSellOrderMarket(pair_Work.Symbol1, pair_Work.lot_sam_1, pair_Work.Price_1);
                            Thread.Sleep(100);
                            BinanceClient.PlaceBuyOrderMarket(pair_Work.Symbol2, pair_Work.lot_sam_2, pair_Work.Price_2);
                            Thread.Sleep(300);
                            BinanceClient.GetAccount();
                        }

                    }

                    if (type == "B_TP")
                    {
                        if (price_last <= price)
                        {

                            string str = DateTime.Now.ToString() + "- Id " + Id.ToString() + ". B_TP " + pair_Work.Symbol1 + "_" + pair_Work.Symbol2 + "_" + pair_Work.lot_sam_1.ToString() + "_" + pair_Work.lot_sam_2.ToString()+ " Profit: " + pair_Work.Profit2.ToString("0.###");
                            My_Telegram.Send_Message(str);

                            order_worked = false;
                            order_wd = true;

                            BinanceClient.PlaceSellOrderMarket(pair_Work.Symbol1, pair_Work.lot_sam_1, pair_Work.Price_1);
                            Thread.Sleep(100);
                            BinanceClient.PlaceBuyOrderMarket(pair_Work.Symbol2, pair_Work.lot_sam_2, pair_Work.Price_2);
                            Thread.Sleep(300);
                            BinanceClient.GetAccount();
                        }

                    }
                }            
            }

        }

    }
}
