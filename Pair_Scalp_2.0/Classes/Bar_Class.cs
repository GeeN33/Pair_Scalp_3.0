using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Pair_Scalp_2._0.Classes
{
    public class Bar_Class
    {
        public double Id { get; set; }
        public string Time{ get; set; }

        #region Price_open

        public double Last1 = 0;
        public double Last2 = 0;

        public double Price_open { get; set; }
        public double Price_close { get; set; }
        public double Price_high { get; set; }
        public double Price_low { get; set; }

        public double Delta_open { get; set; }
        public double Delta_close { get; set; }
        public double Delta_high { get; set; }
        public double Delta_low { get; set; }

        public bool F = true;
        public int t = 0;
        public int tt = 0;

        #endregion

        public double price_sum_p;

        public double indicator_high = 0;
        public double indicator_low = 0;

        public Bar_Class()
        {

        }

    

        public Bar_Class(Bar_Class bar_)
        {

            Time = DateTime.Now.ToString();
            Price_open = bar_.Price_open;
            Price_close = bar_.Price_close;
            Price_high = bar_.Price_high; ;
            Price_low = bar_.Price_low;

            Delta_open = bar_.Delta_open;
            Delta_close = bar_.Delta_close;
            Delta_high = bar_.Delta_high;
            Delta_low = bar_.Delta_low;

            price_sum_p = bar_.price_sum_p;

        }

        public Bar_Class(Bar_Class bar_, int t2)
        {

            Time = DateTime.Now.ToString();
            tt = t2;
            Price_open = bar_.Price_close;
            Price_close = bar_.Price_close;
            Price_high = bar_.Price_close; ;
            Price_low = bar_.Price_close;

            Delta_open = bar_.Delta_close; 
            Delta_close = bar_.Delta_close;
            Delta_high = bar_.Delta_close;
            Delta_low = bar_.Delta_close;

        }
  

        public void sort(Bar_Class bar_)
        {
            Price_close = bar_.Price_close;
            if (bar_.Price_high > Price_high) Price_high = bar_.Price_high;
            if (Price_low > bar_.Price_low) Price_low = bar_.Price_low;

            Delta_close = bar_.Delta_close;
            if (bar_.Delta_high > Delta_high) Delta_high = bar_.Delta_high;
            if (Delta_low > bar_.Delta_low) Delta_low = bar_.Delta_low;

            t++;
            if (t >= tt) F = false;

        }

        public void sort_last(Bar_Class bar_)
        {
            Price_close = bar_.Price_close;
            if (bar_.Price_high > Price_high) Price_high = bar_.Price_high;
            if (Price_low > bar_.Price_low) Price_low = bar_.Price_low;

            Delta_close = bar_.Delta_close;
            if (bar_.Delta_close > Delta_high) Delta_high = bar_.Delta_close;
            if (Delta_low > bar_.Delta_close) Delta_low = bar_.Delta_close;

            price_sum_p = bar_.price_sum_p;

        }

        public void Draw_Bar_Price(Graphics graph, int size, int j, int Height, double max, double min)
        {

            float high_y = (float)(Height - (Price_high - min) * (Height / (max - min)));
            float low_y = (float)(Height - (Price_low - min) * (Height / (max - min)));
            //  float close_y = (float)(Height - (Price_close - min) * (Height / (max - min)));

            if (Price_open < Price_close)
            {
                graph.DrawLine(new Pen(Color.Green, size - 1), j, high_y, j, low_y);

            }
            else if (Price_open > Price_close)
            {
                graph.DrawLine(new Pen(Color.Red, size - 1), j, high_y, j, low_y);
            }
            else
            {

                graph.DrawLine(new Pen(Color.Yellow, size - 1), j, high_y, j, low_y);

            }



        }

        public void Draw_Bar_Delta(Graphics graph, int size, int j, int Height, double max, double min)
        {

            float high_y = (float)(Height - (Delta_high - min) * (Height / (max - min)));
            float low_y = (float)(Height - (Delta_low - min) * (Height / (max - min)));

            if (Delta_open < Delta_close)
            {
                graph.DrawLine(new Pen(Color.Green, size - 1), j, high_y, j, low_y);

            }
            else if (Delta_open > Delta_close)
            {
                graph.DrawLine(new Pen(Color.Red, size - 1), j, high_y, j, low_y);
            }
            else
            {

                graph.DrawLine(new Pen(Color.Yellow, size - 1), j, high_y, j, low_y - 3);

            }

        }

        public void Draw_Last_Price(Graphics graph, int size, int j, int Height, int Width, double max, double min)
        {

            float close_y = (float)(Height - (Price_close - min) * (Height / (max - min)));

            if (Price_open < Price_close)
            {
                graph.DrawLine(new Pen(Color.Black, 1), j + (size - 3), close_y, Width, close_y);

                graph.DrawLine(new Pen(Color.Green, 20), Width - 40, close_y + 6, Width, close_y + 6);

                graph.DrawString(Price_close.ToString(), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(Width - 40, close_y));
            }
            else if (Price_open > Price_close)
            {
                graph.DrawLine(new Pen(Color.Black, 1), j + (size - 3), close_y, Width, close_y);

                graph.DrawLine(new Pen(Color.Red, 20), Width - 40, close_y + 6, Width, close_y + 6);

                graph.DrawString(Price_close.ToString(), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(Width - 40, close_y));
            }
            else
            {
                graph.DrawLine(new Pen(Color.Black, 1), j + (size - 3), close_y, Width, close_y);

                graph.DrawLine(new Pen(Color.Yellow, 20), Width - 40, close_y + 6, Width, close_y + 6);

                graph.DrawString(Price_close.ToString(), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(Width - 40, close_y));

            }


        }

        public void Draw_Last_Delta(Graphics graph, int size, int j, int Height, int Width, double max, double min)
        {

            float close_y = (float)(Height - (Delta_close - min) * (Height / (max - min)));

            if (Delta_open < Delta_close)
            {

                graph.DrawLine(new Pen(Color.Black, 1), j + (size - 3), close_y, Width, close_y);

                graph.DrawLine(new Pen(Color.Green, 20), Width - 40, close_y + 6, Width, close_y + 6);

                graph.DrawString(Delta_close.ToString(), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(Width - 40, close_y));


            }
            else if (Delta_open > Delta_close)
            {

                graph.DrawLine(new Pen(Color.Black, 1), j + (size - 3), close_y, Width, close_y);

                graph.DrawLine(new Pen(Color.Red, 20), Width - 40, close_y + 6, Width, close_y + 6);

                graph.DrawString(Delta_close.ToString(), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(Width - 40, close_y));

            }
            else
            {

                graph.DrawLine(new Pen(Color.Black, 1), j + (size - 3), close_y, Width, close_y);

                graph.DrawLine(new Pen(Color.Yellow, 20), Width - 40, close_y + 6, Width, close_y + 6);

                graph.DrawString(Delta_close.ToString(), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(Width - 40, close_y));

            }

        }

        public void Draw_Last_Procent(Graphics graph, int size, int j, int Height, int Width, double max, double min)
        {

                float close_y = (float)(Height - (price_sum_p - min) * (Height / (max - min)));

         
                graph.DrawLine(new Pen(Color.White, 1), j + (size - 3), close_y, Width, close_y);

        }

        public void indicator_MA_high_low(Graphics graph, int size, int j, int Height, double max, double min)
        {
            if (indicator_high != 0 && indicator_low != 0)
            {
                float high_y = (float)(Height - (indicator_high - min) * (Height / (max - min)));
                float low_y = (float)(Height - (indicator_low - min) * (Height / (max - min)));

                graph.DrawLine(new Pen(Color.Black, size - 1), j, high_y, j, high_y + 3);

                graph.DrawLine(new Pen(Color.White, size - 1), j, low_y, j, low_y + 3);

            }

        }

        public int correlciy()
        {
            int ccp = 0;

            if (Price_close > Price_open && Delta_close > Delta_open) ccp = 0;
            if (Price_close < Price_open && Delta_close < Delta_open) ccp = 0;

            if (Price_close > Price_open && Delta_close < Delta_open) ccp = 1;
            if (Price_close < Price_open && Delta_close > Delta_open) ccp = -1;

            return ccp;
        }

    }
}
