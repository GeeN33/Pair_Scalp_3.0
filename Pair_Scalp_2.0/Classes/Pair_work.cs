using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Pair_Scalp_2._0.Classes_binance;

namespace Pair_Scalp_2._0.Classes
{
    public class Pair_work
    {
        Form_Main form_Main;
        public bool trade = false;
        public List<Order_work> order_work = new List<Order_work>();      
        public Position position1 = new Position();
        public Position position2 = new Position();
        public Entry_Price entry_Price = new Entry_Price() { Buy_Sell = "N", Price = 0 };
        public string Symbol_sam_str = "";
        public double Profit = 0;
        public double Profit2 = 0;
        public int error = 0;
        public string Symbol1 = "BTCUSDT";
        public string Symbol2 = "CVCUSDT";
        public double Price_last = 0;
        public double Price_1 = 0;
        public double Price_2 = 0;
        public double lot1 = 1;
        public double lot2 = 1;
        public double lot_sam_1 = 1;
        public double lot_sam_2 = 1;
        public double kf_lot_sam = 1;
        public double kf1 = -1;
        public double kf2 = 1;
        public int Amount_Error = 0;
        public bool Buy_true = false;
        public bool Sell_true = false;
        public double GO = 0;

        public Pair_work(Form_Main form_Main, string str)
        {
            this.form_Main = form_Main;
            Symbol_sam_str = str;
            str_in_smbl(str);
        }
        public void Start()
        {
            form_Main.evenTimer += timer1_Tick;
            form_Main.evenWssTick += evenWssTick;
            form_Main.evenOpen += Form_evenOpen;
            BinanceClient.evenAccount += Get_account;
        }     
        public void Get_account(Account Data)
        {
            GO_Sam_order();

            if (Data.positions != null)
            {
                position1.entryPrice = 0;
                position1.Profit = 0;
                position1.positionAmt = 0;
                position2.entryPrice = 0;
                position2.Profit = 0;
                position2.positionAmt = 0;
                entry_Price.Buy_Sell = "N";

                for (int i = 0; i < Data.positions.Length; i++)
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                    double prf = Convert.ToDouble(Data.positions[i].unrealizedProfit);
                    double entryPrice = Convert.ToDouble(Data.positions[i].entryPrice);
                    double positionAmt = Convert.ToDouble(Data.positions[i].positionAmt);

                    if (Data.positions[i].symbol == Symbol1 && prf != 0)
                    {
                        position1.symbol = Data.positions[i].symbol;
                        position1.Profit = prf;
                        position1.entryPrice = entryPrice;
                        position1.positionAmt = positionAmt;


                    }
                    if (Data.positions[i].symbol == Symbol2 && prf != 0)
                    {
                        position2.symbol = Data.positions[i].symbol;
                        position2.Profit = prf;
                        position2.entryPrice = entryPrice;
                        position2.positionAmt = positionAmt;

                    }

                }

                if ((position1.Profit + position2.Profit) != 0)
                {

                    if ((position1.positionAmt/ kf1) == ( position2.positionAmt/ kf2))
                    {

                        if (position1.positionAmt < 0 && position2.positionAmt > 0)
                        {
                            entry_Price.Buy_Sell = "B";
                            entry_Price.Price = (position1.entryPrice * kf1) + (position2.entryPrice * kf2);
                            kf_lot_sam = from_minus_to_plus(position2.positionAmt / kf2);
                            lot_sam_1 = from_minus_to_plus(position1.positionAmt);
                            lot_sam_2 = from_minus_to_plus(position2.positionAmt);
                        }
                        if (position1.positionAmt > 0 && position2.positionAmt < 0)
                        {
                            entry_Price.Buy_Sell = "S";
                            entry_Price.Price = (position1.entryPrice * kf1) + (position2.entryPrice * kf2);
                            kf_lot_sam = from_minus_to_plus(position1.positionAmt / kf1);
                            lot_sam_1 = from_minus_to_plus(position1.positionAmt);
                            lot_sam_2 = from_minus_to_plus(position2.positionAmt);
                        }

                        error = 0;
                        form_Main.OpenPosition(Symbol_sam_str);
                    }
                    else
                    {
                       
                        error++;

                        if (trade)
                        {
                            if (error > 1 && error < 4)
                            {
                                Thread.Sleep(100);
                                BinanceClient.GetAccount();

                            }

                            if (error == 20)
                            {
                                string str = DateTime.Now.ToString() + "- " + Symbol_sam_str + " ERROR";
                                My_Telegram.Send_Message(str);
                            }
                        }

                        if (error > 500) error = 1;
                    }
              
                }              
                else
                {                 
                    error = 0;
                }
                Amount_Error = 0;

               
                Profit = position1.Profit + position2.Profit;
           
            }
            else Amount_Error++; 

        }     
        private void Form_evenOpen(string str)
        {
            if (str_in_smbl_true(str))
            {
                Chart chart = new Chart(this);
                chart.Show();
            }
        }
        public bool str_in_smbl_true(string str)
        {
            string[] arr = null;

            arr = str.Split(new char[] { '_' });

            if (arr.Length < 3) return false;

            if(Symbol1 == arr[0] && Symbol2 == arr[1]) return true;


            return false;

        }            
        public void str_in_smbl(string str)
        {
            string[] arr = null;

            arr = str.Split(new char[] { '_' });

            if (arr.Length < 3) return;

            Symbol1 = arr[0];
            Symbol2 = arr[1];

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");


            string text = arr[2];
            if (int.TryParse(text, out int number))
            {
                kf1 = Convert.ToDouble(text);
                lot1 = from_minus_to_plus(kf1);
            }
            if (double.TryParse(text, out double number1))
            {
                kf1 = Convert.ToDouble(text);
                lot1 = from_minus_to_plus(kf1);
            }

            text = arr[3];
            if (int.TryParse(text, out int number2))
            {
                kf2 = Convert.ToDouble(text);
                lot2 = from_minus_to_plus(kf2);
            }
            if (double.TryParse(text, out double number3))
            {
                kf2 = Convert.ToDouble(text);
                lot2 = from_minus_to_plus(kf2);
            }

        }
        void Profit_colc()
        {
            if (entry_Price.Buy_Sell == "B")
            {
                Profit2 = (Price_last - entry_Price.Price)* kf_lot_sam;
              
            }
            if (entry_Price.Buy_Sell == "S")
            {
                Profit2 = (entry_Price.Price - Price_last)* kf_lot_sam;
              
            }
            if (entry_Price.Buy_Sell == "N")
            {
                Profit2 = 0;
             
            }
        }
        private void evenWssTick(mark_Prices mark_prices)
        {
            for (int i = 0; i < mark_prices.data.Length - 1; i++)
            {

                if (mark_prices.data[i].s == Symbol1)
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                    Price_1 = Convert.ToDouble(mark_prices.data[i].p);
                }
                if (mark_prices.data[i].s == Symbol2)
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                    Price_2 = Convert.ToDouble(mark_prices.data[i].p);
                }

               
            }

            Price_last = (Price_1 * kf1) + (Price_2 * kf2);
            Profit_colc();

            for (int i = 0; i < order_work.Count; i++)
            {             
                order_work[i].order_execute(Price_last);
            }
        }
        private void timer1_Tick()
        {
            
        }
        double from_minus_to_plus(double namber)
        {
            if(namber < 0)
            {
                return namber * -1;

            }
          
            return namber;
        }

        public void GO_Sam_order()
        {
            int b = 0;
            int s = 0;

            for (int i = 0; i < order_work.Count; i++)
            {
                if (order_work[i].order_worked && !order_work[i].order_wd && order_work[i].type == "S_LT")
                {
                    s++;
                }
                if (order_work[i].order_worked && !order_work[i].order_wd && order_work[i].type == "B_LT")
                {
                    b++;
                }
            }

            if(b > s)
            {
                GO = ((Price_1 * lot1) + (Price_2 * lot2)) * b;
            }
            if (b < s)
            {
                GO = ((Price_1 * lot1) + (Price_2 * lot2)) * s;
            }
            if (b == s)
            {
                GO = ((Price_1 * lot1) + (Price_2 * lot2)) * b;
            }

            form_Main.GO_Sam_order(Symbol_sam_str, GO);


        }
    }
}
