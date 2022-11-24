using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using Pair_Scalp_2._0.Classes;
using Pair_Scalp_2._0.Classes_binance;

namespace Pair_Scalp_2._0
{
    public partial class Chart : Form
    {
        public Chart(Pair_work pair_Work)
        {
            this.pair_Work = pair_Work;
            InitializeComponent();
            this.Text = pair_Work.Symbol_sam_str;
            str_in_smbl(pair_Work.Symbol_sam_str);
        }
       
        public Pair_work pair_Work;
        public string Symbol = "BTCUSDT";
        public string Symbol2 = "CVCUSDT";
        Draws_Bars_Class Draw = new Draws_Bars_Class();
        public List<bars_Class> bars = new List<bars_Class>();
        public List<bars_Class> bars2 = new List<bars_Class>();
        API API = new API();
        public double kf1 = -1;
        public double kf2 = 1;
        string priod = "1d";
        int periud_indekator = 21;
        int Br = 200; // скрул
        double ch = 0;


        private void Chart_Load(object sender, EventArgs e)
        {
         
            BinanceClient.GetAccount();

            API.binance_klines(Symbol, priod, ref bars);
            API.binance_klines(Symbol2, priod, ref bars2);
            Up_data_sort();
            Up_data_kf();
            Draw.pair_Work = pair_Work;
            Draws();
            timer1.Enabled = true;
            timer1.Start();
            timer2.Enabled = true;
            timer2.Start();
            timer3.Enabled = true;
            timer3.Start();
            checkBox1.Checked = pair_Work.trade;

        }

        public void str_in_smbl(string str)
        {
            string[] arr = null;

            arr = str.Split(new char[] { '_' });

            if (arr.Length < 3) return;

            Symbol = arr[0];
            Symbol2 = arr[1];

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");


            string text = arr[2];
            if (int.TryParse(text, out int number))
            {
                kf1 = Convert.ToDouble(text);

            }
            if (double.TryParse(text, out double number1))
            {
                kf1 = Convert.ToDouble(text);

            }

            text = arr[3];
            if (int.TryParse(text, out int number2))
            {
                kf2 = Convert.ToDouble(text);

            }
            if (double.TryParse(text, out double number3))
            {
                kf2 = Convert.ToDouble(text);

            }

        }
     
        public void indicator_MA(int p)
        {
            for (int i = 0; i < bars.Count; i++)
            {
                if (i >= p)
                {
                    double ma_h = 0;
                    double ma_l = 0;
                    for (int j = i - p; j < i; j++)
                    {
                        ma_h = ma_h + bars[j].high;
                        ma_l = ma_l + bars[j].low;

                    }

                    bars[i].indicator_high = ma_h / p;
                    bars[i].indicator_low = ma_l / p;


                }
                else
                {
                    bars[i].indicator_high = 0;
                    bars[i].indicator_low = 0;
                }
            }

        }

        public void Up_data_sort()
        {
            for (int i = 0; i < bars.Count; i++)
            {
                bars[i].Dis_F = false;
                bars[i].zero();

            }

            for (int i = 0; i < bars.Count; i++)
            {

                for (int j = 0; j < bars2.Count; j++)
                {
                    if (bars[i].data == bars2[j].data) { bars[i].add_Symbol2(bars2[j]); bars[i].Dis_F = true; break; }

                }

            }

        }

        public void Up_data_kf()
        {

            for (int i = 0; i < bars.Count; i++)
            {

                bars[i].sort(kf1, kf2);

            }

        }

        public void Draws()
        {
            indicator_MA(periud_indekator);

            if (this.WindowState != FormWindowState.Minimized)
            {
                if (bars.Count > 0)
                {
                    if (bars.Count > Br - 1)
                    {

                        hScrollBar1.Maximum = bars.Count - Br;
                        if (hScrollBar1.Value >= hScrollBar1.Maximum - 20) { hScrollBar1.Value = hScrollBar1.Maximum; }
                        hScrollBar1.Minimum = 0;
                      
                      //  Draw.pair_Work = pair_Work;
                        Draw.Bar_ch = Br;
                        Draw.Bar = bars;
                        Draw.Draws_Bars(pictureBox1, hScrollBar1.Value, hScrollBar1.Value + Br);



                    }
                    else if (bars.Count <= Br)
                    {
                        hScrollBar1.Maximum = 0;
                        hScrollBar1.Minimum = 0;
                        hScrollBar1.Minimum = 0;
                        hScrollBar1.Value = 0;
                      //  Draw.pair_Work = pair_Work;
                        Draw.Bar_ch = Br;
                        Draw.Bar = bars;
                        Draw.Draws_Bars(pictureBox1, 0, bars.Count);

                    }

                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Draws();
            label4.Text = "Amount: " + pair_Work.position1.positionAmt.ToString() + " / " + pair_Work.position2.positionAmt.ToString();
            label5.Text = "Profit2: " + pair_Work.Profit2.ToString("0.###");
            label1.Text = "Profit: " + pair_Work.Profit.ToString("0.###");
            if(pair_Work.entry_Price.Buy_Sell == "N") { panel1.BackColor = Color.Silver; }
            if (pair_Work.entry_Price.Buy_Sell == "B") { panel1.BackColor = Color.GreenYellow; }
            if (pair_Work.entry_Price.Buy_Sell == "S") { panel1.BackColor = Color.Pink; }
            if (pair_Work.error > 0) { panel1.BackColor = Color.Red; }

            textBox1.Text = pair_Work.entry_Price.Price.ToString("0.##") +" - "+ pair_Work.Price_last.ToString("0.##") + " * " + pair_Work.kf_lot_sam.ToString("0.##");

            if (bars.Count > 0) { bars[bars.Count - 1].sort_last(pair_Work.Price_1, pair_Work.Price_2); }

            label6.Text = "ГО: " + (pair_Work.GO/25).ToString("0.##");
        }

        #region // Timeframe
        private void button1_Click(object sender, EventArgs e)
        {
            priod = "5m";

            API.binance_klines(Symbol, priod, ref bars);

            API.binance_klines(Symbol2, priod, ref bars2);

            Up_data_sort();
            Up_data_kf();


            Draws();

            label2.Text = "Timeframe: " + priod;
            ch = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            priod = "1h";

            API.binance_klines(Symbol, priod, ref bars);

            API.binance_klines(Symbol2, priod, ref bars2);

            Up_data_sort();
            Up_data_kf();


            Draws();

            label2.Text = "Timeframe: " + priod;
            ch = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            priod = "4h";

            API.binance_klines(Symbol, priod, ref bars);

            API.binance_klines(Symbol2, priod, ref bars2);

            Up_data_sort();
            Up_data_kf();


            Draws();

            label2.Text = "Timeframe: " + priod;
            ch = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            priod = "1d";

            API.binance_klines(Symbol, priod, ref bars);

            API.binance_klines(Symbol2, priod, ref bars2);

            Up_data_sort();
            Up_data_kf();


            Draws();

            label2.Text = "Timeframe: " + priod;
            ch = 0;
        }
        #endregion

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Draw.true_Price = true;
            Draw.LineY = e.Y;
            Draw.LineX = e.X;
            Draws();
            Draw.true_Price = false;
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (!pair_Work.order_work[i].order_wd)
                {
                    if (pair_Work.order_work[i].e_Y > Draw.LineY && pair_Work.order_work[i].e_Y - 5 < Draw.LineY)
                    {
                        if (pair_Work.order_work[i].selective) { pair_Work.order_work[i].selective = false; }
                        else { pair_Work.order_work[i].selective = true; }
                    }
                }
            }

            Draws();
        }

        #region // trade
        private void button5_Click(object sender, EventArgs e) // Buy Market
        {
            if (checkBox1.Checked)
            {
                BinanceClient.PlaceSellOrderMarket(pair_Work.Symbol1, pair_Work.lot1, pair_Work.Price_1);
                Thread.Sleep(100);
                BinanceClient.PlaceBuyOrderMarket(pair_Work.Symbol2, pair_Work.lot2, pair_Work.Price_2);
                Thread.Sleep(100);
                BinanceClient.GetAccount();

            }
        }

        private void button6_Click(object sender, EventArgs e) // Sell Market
        {
            if (checkBox1.Checked)
            {

                BinanceClient.PlaceBuyOrderMarket(pair_Work.Symbol1, pair_Work.lot1, pair_Work.Price_1);
                Thread.Sleep(100);
                BinanceClient.PlaceSellOrderMarket(pair_Work.Symbol2, pair_Work.lot2, pair_Work.Price_2);
                Thread.Sleep(100);
                BinanceClient.GetAccount();

            }
        }

        private void button7_Click(object sender, EventArgs e)  // fixed B_LT
        {
            if (pair_Work.Price_last > Draw.Line_Price)
            {
                int id = pair_Work.order_work.Count + 1;

                pair_Work.order_work.Add(new Order_work ( id, pair_Work, Draw.Line_Price, "B_LT", Draw.LineY));

                Draws();
            }
     
            listBox1.Items.Clear();
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                listBox1.Items.Add(pair_Work.order_work[i].Id.ToString() + ". " + pair_Work.order_work[i].type + " - " + pair_Work.order_work[i].price.ToString("0.###") + " - " + pair_Work.order_work[i].order_worked.ToString());
            }

            pair_Work.GO_Sam_order();


        }

        private void button8_Click(object sender, EventArgs e)  // fixed S_LT
        {
            if (pair_Work.Price_last < Draw.Line_Price)
            {
                int id = pair_Work.order_work.Count + 1;

                pair_Work.order_work.Add(new Order_work(id, pair_Work, Draw.Line_Price, "S_LT", Draw.LineY));

                Draws();
            }

            listBox1.Items.Clear();
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                listBox1.Items.Add(pair_Work.order_work[i].Id.ToString() + ". " + pair_Work.order_work[i].type + " - " + pair_Work.order_work[i].price.ToString("0.###") + " - " + pair_Work.order_work[i].order_worked.ToString());
            }

            pair_Work.GO_Sam_order();
        }
       
        private void button9_Click(object sender, EventArgs e)   // fixed B_SL
        {
            bool s_sl = true;
            double price_max = 0;
            if (pair_Work.entry_Price.Buy_Sell == "S") { price_max = pair_Work.entry_Price.Price;}
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
               if(pair_Work.order_work[i].order_worked  && pair_Work.order_work[i].type == "S_LT") price_max = pair_Work.order_work[i].price;
            }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "S_LT")
                {
                    if (pair_Work.order_work[i].price > price_max) price_max = pair_Work.order_work[i].price;
                }

                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "B_SL")
                {
                    s_sl = false;
                }
            }

            if (s_sl && price_max != 0)
            {
                if (price_max < Draw.Line_Price)
                {
                    int id = pair_Work.order_work.Count + 1;

                    pair_Work.order_work.Add(new Order_work(id, pair_Work, Draw.Line_Price, "B_SL", Draw.LineY));

                    Draws();
                }

                listBox1.Items.Clear();
                for (int i = 0; i < pair_Work.order_work.Count; i++)
                {
                    listBox1.Items.Add(pair_Work.order_work[i].Id.ToString() + ". " + pair_Work.order_work[i].type + " - " + pair_Work.order_work[i].price.ToString("0.###") + " - " + pair_Work.order_work[i].order_worked.ToString());
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)  //  fixed S_SL
        {
          
            bool s_sl = true;
            double price_min = 0;
            if (pair_Work.entry_Price.Buy_Sell == "B") { price_min = pair_Work.entry_Price.Price; }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "B_LT") price_min = pair_Work.order_work[i].price;
            }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "B_LT")
                {
                    if (pair_Work.order_work[i].price < price_min) price_min = pair_Work.order_work[i].price;
                }

                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "S_SL")
                {
                    s_sl = false;
                }
            }

            if (s_sl && price_min != 0)
            {
                if (price_min > Draw.Line_Price)
                {
                    int id = pair_Work.order_work.Count + 1;

                    pair_Work.order_work.Add(new Order_work(id, pair_Work, Draw.Line_Price, "S_SL", Draw.LineY));

                    Draws();
                }

                listBox1.Items.Clear();
                for (int i = 0; i < pair_Work.order_work.Count; i++)
                {
                    listBox1.Items.Add(pair_Work.order_work[i].Id.ToString() + ". " + pair_Work.order_work[i].type + " - " + pair_Work.order_work[i].price.ToString("0.###") + " - " + pair_Work.order_work[i].order_worked.ToString());
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)  // fixed S_TP
        {
            bool s_sl = true;
            double price_max = 0;
            if (pair_Work.entry_Price.Buy_Sell == "B") { price_max = pair_Work.entry_Price.Price; }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "B_LT") price_max = pair_Work.order_work[i].price;
            }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "B_LT")
                {
                    if (pair_Work.order_work[i].price > price_max) price_max = pair_Work.order_work[i].price;
                }

                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "S_TP")
                {
                    s_sl = false;
                }
            }

            if (s_sl && price_max != 0)
            {
                if (price_max < Draw.Line_Price)
                {
                    int id = pair_Work.order_work.Count + 1;

                    pair_Work.order_work.Add(new Order_work(id, pair_Work, Draw.Line_Price, "S_TP", Draw.LineY));

                    Draws();
                }

                listBox1.Items.Clear();
                for (int i = 0; i < pair_Work.order_work.Count; i++)
                {
                    listBox1.Items.Add(pair_Work.order_work[i].Id.ToString() + ". " + pair_Work.order_work[i].type + " - " + pair_Work.order_work[i].price.ToString("0.###") + " - " + pair_Work.order_work[i].order_worked.ToString());
                }
            }
        }

        private void button12_Click(object sender, EventArgs e) // fixed B_TP 
        {

            bool s_sl = true;
            double price_min = 0;
            if (pair_Work.entry_Price.Buy_Sell == "S") { price_min = pair_Work.entry_Price.Price; }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "S_LT") price_min = pair_Work.order_work[i].price;
            }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "S_LT")
                {
                    if (pair_Work.order_work[i].price < price_min) price_min = pair_Work.order_work[i].price;
                }

                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "B_TP")
                {
                    s_sl = false;
                }
            }

            if (s_sl && price_min != 0)
            {
                if (price_min > Draw.Line_Price)
                {
                    int id = pair_Work.order_work.Count + 1;

                    pair_Work.order_work.Add(new Order_work(id, pair_Work, Draw.Line_Price, "B_TP", Draw.LineY));

                    Draws();
                }

                listBox1.Items.Clear();
                for (int i = 0; i < pair_Work.order_work.Count; i++)
                {
                    listBox1.Items.Add(pair_Work.order_work[i].Id.ToString() + ". " + pair_Work.order_work[i].type + " - " + pair_Work.order_work[i].price.ToString("0.###") + " - " + pair_Work.order_work[i].order_worked.ToString());
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)  // Delet select Order
        {
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].selective)
                {               
                   pair_Work.order_work[i].order_worked = false;                   
                }
            }

            Draws();

            listBox1.Items.Clear();
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                listBox1.Items.Add(pair_Work.order_work[i].Id.ToString() + ". " + pair_Work.order_work[i].type + " - " + pair_Work.order_work[i].price.ToString("0.###") + " - " + pair_Work.order_work[i].order_worked.ToString());
            }

            pair_Work.GO_Sam_order();
        }

        private void button14_Click(object sender, EventArgs e) // Move select Order
        {
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].selective && !pair_Work.order_work[i].order_wd)
                {
                    if (pair_Work.order_work[i].type== "B_LT" && pair_Work.Price_last > Draw.Line_Price)
                    {                     
                        pair_Work.order_work[i].price = Draw.Line_Price;
                        pair_Work.order_work[i].selective = false;
                        pair_Work.order_work[i].e_Y = Draw.LineY;
                        Draws();
                    }

                    if (pair_Work.order_work[i].type == "S_LT" && pair_Work.Price_last < Draw.Line_Price)
                    {
                        pair_Work.order_work[i].price = Draw.Line_Price;
                        pair_Work.order_work[i].selective = false;
                        pair_Work.order_work[i].e_Y = Draw.LineY;
                        Draws();
                    }

                    if (pair_Work.order_work[i].type == "B_SL")
                    {
                        move_B_SL(i);
                    }

                    if (pair_Work.order_work[i].type == "S_SL")
                    {
                        move_S_SL(i);
                    }

                    if (pair_Work.order_work[i].type == "B_TP")
                    {
                        move_B_TP(i);
                    }

                    if (pair_Work.order_work[i].type == "S_TP")
                    {
                        move_S_TP(i);
                    }

                }
            }

            listBox1.Items.Clear();
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                listBox1.Items.Add(pair_Work.order_work[i].Id.ToString() + ". " + pair_Work.order_work[i].type + " - " + pair_Work.order_work[i].price.ToString("0.###") + " - " + pair_Work.order_work[i].order_worked.ToString());
            }

        }

        private void button15_Click(object sender, EventArgs e)// Delet All Order
        {
            pair_Work.order_work.Clear();

            listBox1.Items.Clear();
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                listBox1.Items.Add(pair_Work.order_work[i].Id.ToString() + ". " + pair_Work.order_work[i].type + " - " + pair_Work.order_work[i].price.ToString("0.###") + " - " + pair_Work.order_work[i].order_worked.ToString());
            }

            pair_Work.GO_Sam_order();
        }

        void move_B_SL(int j)
        {
            double price_max = 0;
            if (pair_Work.entry_Price.Buy_Sell == "S") { price_max = pair_Work.entry_Price.Price; }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "S_LT") price_max = pair_Work.order_work[i].price;
            }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "S_LT")
                {
                    if (pair_Work.order_work[i].price > price_max) price_max = pair_Work.order_work[i].price;
                }
            }

            if (price_max != 0)
            {
                if (price_max < Draw.Line_Price)
                {
                    pair_Work.order_work[j].price = Draw.Line_Price;
                    pair_Work.order_work[j].selective = false;
                    pair_Work.order_work[j].e_Y = Draw.LineY;
                    Draws();

                    listBox1.Items.Clear();
                    for (int i = 0; i < pair_Work.order_work.Count; i++)
                    {
                        listBox1.Items.Add(pair_Work.order_work[i].Id.ToString() + ". " + pair_Work.order_work[i].type + " - " + pair_Work.order_work[i].price.ToString("0.###") + " - " + pair_Work.order_work[i].order_worked.ToString());
                    }
                }
            }

        }
        void move_S_SL(int j)
        {
            double price_min = 0;
            if (pair_Work.entry_Price.Buy_Sell == "B") { price_min = pair_Work.entry_Price.Price; }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "B_LT") price_min = pair_Work.order_work[i].price;
            }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "B_LT")
                {
                    if (pair_Work.order_work[i].price < price_min) price_min = pair_Work.order_work[i].price;
                }
            }

            if (price_min != 0)
            {
                if (price_min > Draw.Line_Price)
                {
                    pair_Work.order_work[j].price = Draw.Line_Price;
                    pair_Work.order_work[j].selective = false;
                    pair_Work.order_work[j].e_Y = Draw.LineY;
                    Draws();

                    listBox1.Items.Clear();
                    for (int i = 0; i < pair_Work.order_work.Count; i++)
                    {
                        listBox1.Items.Add(pair_Work.order_work[i].Id.ToString() + ". " + pair_Work.order_work[i].type + " - " + pair_Work.order_work[i].price.ToString("0.###") + " - " + pair_Work.order_work[i].order_worked.ToString());
                    }
                }


            }
        }

        void move_B_TP(int j)
        {
            double price_min = 0;
            if (pair_Work.entry_Price.Buy_Sell == "S") { price_min = pair_Work.entry_Price.Price; }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "S_LT") price_min = pair_Work.order_work[i].price;
            }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "S_LT")
                {
                    if (pair_Work.order_work[i].price < price_min) price_min = pair_Work.order_work[i].price;
                }
            }

            if ( price_min != 0)
            {
                if (price_min > Draw.Line_Price)
                {
                    pair_Work.order_work[j].price = Draw.Line_Price;
                    pair_Work.order_work[j].selective = false;
                    pair_Work.order_work[j].e_Y = Draw.LineY;
                    Draws();

                    listBox1.Items.Clear();
                    for (int i = 0; i < pair_Work.order_work.Count; i++)
                    {
                        listBox1.Items.Add(pair_Work.order_work[i].Id.ToString() + ". " + pair_Work.order_work[i].type + " - " + pair_Work.order_work[i].price.ToString("0.###") + " - " + pair_Work.order_work[i].order_worked.ToString());
                    }
                }

            }

        }
        void move_S_TP(int j)
        {
            double price_max = 0;
            if (pair_Work.entry_Price.Buy_Sell == "B") { price_max = pair_Work.entry_Price.Price; }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "B_LT") price_max = pair_Work.order_work[i].price;
            }
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_worked && pair_Work.order_work[i].type == "B_LT")
                {
                    if (pair_Work.order_work[i].price > price_max) price_max = pair_Work.order_work[i].price;
                }
            }

            if (price_max != 0)
            {
                if (price_max < Draw.Line_Price)
                {
                    pair_Work.order_work[j].price = Draw.Line_Price;
                    pair_Work.order_work[j].selective = false;
                    pair_Work.order_work[j].e_Y = Draw.LineY;
                    Draws();

                    listBox1.Items.Clear();
                    for (int i = 0; i < pair_Work.order_work.Count; i++)
                    {
                        listBox1.Items.Add(pair_Work.order_work[i].Id.ToString() + ". " + pair_Work.order_work[i].type + " - " + pair_Work.order_work[i].price.ToString("0.###") + " - " + pair_Work.order_work[i].order_worked.ToString());
                    }
                }

            }
        }

        #endregion

        private void Chart_FormClosed(object sender, FormClosedEventArgs e)
        {
          
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            ch++;
            if(priod == "5m" && ch >= 1)
            {

                if (bars.Count > 0) 
                { 
                    bars.Add(new bars_Class(bars[bars.Count - 1]));
                    bars.RemoveAt(0);
                }
                ch = 0;

            }
            if (priod == "1h" && ch >= 12)
            {

                if (bars.Count > 0)
                {
                    bars.Add(new bars_Class(bars[bars.Count - 1]));
                    bars.RemoveAt(0);
                }

                ch = 0;

            }
            if (priod == "4h" && ch >= 48)
            {

                API.binance_klines(Symbol, priod, ref bars);

                API.binance_klines(Symbol2, priod, ref bars2);

                Up_data_sort();
                Up_data_kf();


                Draws();

                label2.Text = "Timeframe: " + priod;
                ch = 0;

            }
            if (priod == "1d" && ch >= 48)
            {

                API.binance_klines(Symbol, priod, ref bars);

                API.binance_klines(Symbol2, priod, ref bars2);

                Up_data_sort();
                Up_data_kf();


                Draws();

                label2.Text = "Timeframe: " + priod;
                ch = 0;

            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
          
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
          
            pair_Work.trade = checkBox1.Checked;
          
  
        }

       
    }
}
