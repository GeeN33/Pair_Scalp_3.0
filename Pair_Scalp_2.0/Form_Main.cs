using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pair_Scalp_2._0.Classes_binance;
using Pair_Scalp_2._0.Classes;
using Pair_Scalp_2._0.Properties;

namespace Pair_Scalp_2._0
{
    public partial class Form_Main : Form
    {
        public Form_Main()
        {
            InitializeComponent();
        }

        public bool online_connect = false;
        public bool last_connect = true;
        public List<string> OpenPosition_pair = new List<string>();
        public List<GO_Class> GO = new List<GO_Class>();
        double GO_Sam = 0;
        public int error1 = 0;
        public int error2 = 0;
        Wss_MarkPrice wss_Mark = new Wss_MarkPrice();

        public string AppDir; // путь к папке приложения

        #region // Объявляем делегат

        public delegate void delegwsstick(mark_Prices mark_prices); // 1. Объявляем делегат      
        public event delegwsstick evenWssTick;

        public delegate void delegtimer(); // 2. Объявляем делегат
        public event delegtimer evenTimer;

        public delegate void delegOpen(string str); // 2. Объявляем делегат
        public event delegOpen evenOpen;

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            // определение папки, в которой запущена программа
            string path = Application.ExecutablePath;
            AppDir = path.Substring(0, path.LastIndexOf('\\') + 1);
            Online_Connect.online_connect(ref online_connect);
            BinanceClient.start();
            BinanceClient.evenBalance += GetBalance;
            BinanceClient.evenAccount += Get_account;
            BinanceClient.evenOrder += evenOrder;
            BinanceClient.evenError += evenError;
            BinanceClient.GetBalance();
            BinanceClient.GetAccount();

            Zagruzka();

            My_Telegram.evenvUp_data += My_Telegram_evenvUp_data;
            My_Telegram.Start();
            My_Telegram.id = Convert.ToInt64(Settings.Default["My_Telegram_id"]);

            wss_Mark.Socket_tick();
            wss_Mark.evenWssTick += Wss_Mark_evenWssTick;

            timer1.Enabled = true;
            timer1.Start();
            timer1.Interval = 20000; 

            timer2.Enabled = true;
            timer2.Start();

            if (online_connect) label1.Text = "Online";
            else label1.Text = "Offline";
        }

        private void Wss_Mark_evenWssTick(mark_Prices mark_prices)
        {
            evenWssTick?.Invoke(mark_prices);
        }

        void evenError(string error)
        {
            textBox3.Text = error;
        }

        void evenOrder(Order order)
        {

        }

        public void GO_Sam_order(string Symbol_str, double GO_)
        {
            bool f = true;
            for (int i = 0; i < GO.Count; i++)
            {
                if (GO[i].Symbol == Symbol_str)
                {
                    GO[i].go = GO_;

                    f = false;

                }
            }
            if (f)
            {
                GO.Add(new GO_Class() { Symbol = Symbol_str,  go = GO_ });

            }

            GO_Sam = 0;
            for (int i = 0; i < GO.Count; i++)
            {
                GO_Sam = GO_Sam + GO[i].go;
               
            }
           
            label2.Text = "ГО: " + (GO_Sam/25).ToString("0.##");
        }

        public void OpenPosition(string Symbol_str)
        {
            bool f = true;
            for(int i = 0; i < OpenPosition_pair.Count; i++)
            {
                if (OpenPosition_pair[i] == Symbol_str)
                { 
                    f = false;
                   
                }
            }
            if (f) 
            {
                OpenPosition_pair.Add(Symbol_str);
               
            }
        }
        public void GetBalance(List<Balance> balance) 
        {
            listBox2.Items.Clear();
            for (int i = 0; i < balance.Count; i++)
            {
                if (balance[i].asset == "USDT") 
                {
                    listBox2.Items.Add(DateTime.Now.ToString());
                    listBox2.Items.Add("balance: " + balance[i].balance);              
                    listBox2.Items.Add("availableBalance: " + balance[i].availableBalance);
                    listBox2.Items.Add("crossUnPnl: " + balance[i].crossUnPnl);

                    My_Telegram.balance = balance[i].balance;
                    My_Telegram.availableBalance = balance[i].availableBalance;

                }
            }


        }
        public void Get_account(Account Data)
        {
            error1 = 0;

            if (Data.positions != null)
            {
                listBox1.Items.Clear();
               
                GO_Sam = 0;

                listBox1.Items.Add(DateTime.Now.ToString());
                listBox1.Items.Add("positions:");


                for (int i = 0; i < Data.positions.Length; i++)
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                    double prf = Convert.ToDouble(Data.positions[i].unrealizedProfit);
                    double entryPrice = Convert.ToDouble(Data.positions[i].entryPrice);
                    double positionAmt = Convert.ToDouble(Data.positions[i].positionAmt);

                    if (prf != 0)
                    {
                        listBox1.Items.Add(Data.positions[i].symbol +
                           " =  entryPrice: " + entryPrice.ToString("0.####") +
                           "  | positionAmt: " + positionAmt.ToString("0.####") +
                            "  | prf:  " + prf.ToString("0.##"));

                        error1++;
                    }

                }
               
            }
            else { listBox1.Items.Add("Account error"); }

            if(!chet_not_chet(error1))
            { 
                error2++;
                if (error2 > 0 && error2 < 3)
                {
                    Thread.Sleep(100);
                    BinanceClient.GetAccount();

                }
            } 
            else 
            {
                if (error2 >= 3)
                {
                    string str = DateTime.Now.ToString() + "- Main OK";
                    My_Telegram.Send_Message(str);
                }
                error2 = 0;
                this.BackColor = Color.Linen;
            }

            if (error2 == 7) 
            {
                string str = DateTime.Now.ToString() + "- Main ERROR";
                My_Telegram.Send_Message(str);
                this.BackColor = Color.Red;
            } 

        }

        public void Zagruzka()
        {

            string s = AppDir + @"\Symbols_coins_T.txt";


            if (File.Exists(s))
            {
                StreamReader sr = new StreamReader(s);

                while (!sr.EndOfStream)
                {
                    string temp = sr.ReadLine();

                    listBox3.Items.Add(temp);
                    Thread_Start(temp);

                }

                sr.Close();
            }


        }

        bool chet_not_chet(int ch)
        {
            int c = (int)ch / 2;
            int h = ch - (c * 2);
            if (h == 0) { return true; } else { return false; }
        }

        void Thread_Start(string str)
        {

            Pair_work Pair_Thread = new Pair_work(this, str);

            Thread myThread = new Thread(new ThreadStart(Pair_Thread.Start));
            myThread.IsBackground = true;
            myThread.Start();

        }

        private void My_Telegram_evenvUp_data(string str)
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                textBox2.Text = str;
            }));
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex >= 0)
            {
                textBox1.Text = listBox3.SelectedItem.ToString();

            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex >= 0)
            {
                textBox1.Text = listBox4.SelectedItem.ToString();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            evenOpen?.Invoke(textBox1.Text);
            //Chart chart = new Chart(textBox1.Text);
            //chart.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listBox4.Items.Clear();

            if (online_connect)
            {
                timer1.Interval = 60000;
                if (wss_Mark.FF) wss_Mark.Re_Socket_tick();

                BinanceClient.GetBalance();
                BinanceClient.GetAccount();

                evenTimer?.Invoke();

                wss_Mark.FF = true;

                for (int i = 0; i < OpenPosition_pair.Count; i++)
                {
                   listBox4.Items.Add(OpenPosition_pair[i]);
                }

                label2.Text = "ГО: " + (GO_Sam / 25).ToString("0.##");
            }
       
        }

        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default["My_Telegram_id"] = My_Telegram.id.ToString();
            Settings.Default.Save();

            My_Telegram.Cancel_T();
            wss_Mark.ws_Closing();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Online_Connect.online_connect(ref online_connect);
            if (online_connect) label1.Text = "Online";
            else label1.Text = "Offline";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "") 
            {
                listBox3.Items.Add(textBox4.Text);
                Thread_Start(textBox4.Text);
            }
        }
    }
}
