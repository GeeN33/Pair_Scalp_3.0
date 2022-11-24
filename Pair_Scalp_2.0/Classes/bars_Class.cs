using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Pair_Scalp_2._0.Classes
{
    public class bars_Class
    {
        public double data = 0;
        public double open = 0;


        public double close = 0;
        public double high = 0;
        public double low = 0;
        public double vol = 0;

        public double open1 = 0;
        public double close1 = 0;
        public double high1 = 0;
        public double low1 = 0;

        public double open2 = 0;
        public double close2 = 0;
        public double high2 = 0;
        public double low2 = 0;

        public double indicator_high = 0;
        public double indicator_low = 0;
        public double kf1 = 0;
        public double kf2 = 0;

        public bool Dis_F = true;

        public bars_Class(double data, double open, double high, double low, double close, double vol)
        {
            this.data = data;
            this.open = open;
            this.close = close;
            this.high = high;
            this.low = low;
            this.vol = vol;


            open1 = open;
            close1 = close;
            high1 = high;
            low1 = low;

        }

        public bars_Class(bars_Class bars_)
        {
              kf1 = bars_.kf1;
              kf2 = bars_.kf2;

             data = bars_.data;
            open = bars_.close;
            close = bars_.close;
            high = bars_.close;
            low = bars_.close;
            vol = bars_.vol;

           
            open1 = bars_.close1;
            close1 = bars_.close1;
            high1 = bars_.close1;
            low1 = bars_.close1;


            open2 = bars_.close2;
            close2 = bars_.close2;
            high2 = bars_.close2;
            low2 = bars_.close2;

        }

        public void sort(double kf1_, double kf2_)
        {
            kf1 = kf1_;
            kf2 = kf2_;

            if (kf1 == 0 && kf2 == 0) return;

            open = (open1 * kf1) + (open2 * kf2);
            close = (close1 * kf1) + (close2 * kf2);

            if (kf1 >= 0 && kf2 >= 0)
            {
                high = (high1 * kf1) + (high2 * kf2);
                low = (low1 * kf1) + (low2 * kf2);
            }

            if (kf1 >= 0 && kf2 <= 0)
            {

                high = (high1 * kf1) + (low2 * kf2);
                low = (low1 * kf1) + (high2 * kf2);
            }

            if (kf1 <= 0 && kf2 >= 0)
            {

                high = (low1 * kf1) + (high2 * kf2);
                low = (high1 * kf1) + (low2 * kf2);
            }

        }

        public void sort_last(double pr1, double pr2)
        {
            close1 = pr1;
            close2 = pr2;
            close = (close1 * kf1) + (close2 * kf2);

            if (high1 < pr1) { high1 = pr1; }
            if (low1 > pr1) { low1 = pr1; }


            if (high2 < pr2) { high2 = pr2; }
            if (low2 > pr2) { low2 = pr2; }


            if (kf1 >= 0 && kf2 >= 0)
            {
                high = (high1 * kf1) + (high2 * kf2);
                low = (low1 * kf1) + (low2 * kf2);
            }

            if (kf1 >= 0 && kf2 <= 0)
            {

                high = (high1 * kf1) + (low2 * kf2);
                low = (low1 * kf1) + (high2 * kf2);
            }

            if (kf1 <= 0 && kf2 >= 0)
            {

                high = (low1 * kf1) + (high2 * kf2);
                low = (high1 * kf1) + (low2 * kf2);
            }

        }

        public void add_Symbol2(bars_Class bar)
        {
            open2 = bar.open;
            close2 = bar.close;
            high2 = bar.high;
            low2 = bar.low;
        }

        public void zero()
        {
            open2 = 0;
            close2 = 0;
            high2 = 0;
            low2 = 0;

            open = 0;
            close = 0;
            high = 0;
            low = 0;

        }

        public void Draw_Bar_Price(Graphics graph, int size, int j, int Height, double max, double min)
        {

            float high_y = (float)(Height - (high - min) * (Height / (max - min)));
            float low_y = (float)(Height - (low - min) * (Height / (max - min)));
            // float close_y = (float)(Height - (close - min) * (Height / (max - min)));

            if (open < close)
            {
                graph.DrawLine(new Pen(Color.Green, size - 1), j, high_y, j, low_y);

            }
            else if (open > close)
            {
                graph.DrawLine(new Pen(Color.Red, size - 1), j, high_y, j, low_y);
            }
            else
            {

                graph.DrawLine(new Pen(Color.Yellow, size - 1), j, high_y, j, low_y);

            }



        }

        public void Draw_Last_Price(Graphics graph, int size, int j, int Height, int Width, double max, double min, double last)
        {

            float close_y = (float)(Height - (last - min) * (Height / (max - min)));

            if (open < last)
            {
                graph.DrawLine(new Pen(Color.Black, 1), j + (size - 3), close_y, Width, close_y);

                graph.DrawLine(new Pen(Color.Green, 20), Width - 40, close_y + 6, Width, close_y + 6);

                graph.DrawString(last.ToString(), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(Width - 40, close_y));
            }
            else if (open > last)
            {
                graph.DrawLine(new Pen(Color.Black, 1), j + (size - 3), close_y, Width, close_y);

                graph.DrawLine(new Pen(Color.Red, 20), Width - 40, close_y + 6, Width, close_y + 6);

                graph.DrawString(last.ToString(), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(Width - 40, close_y));
            }
            else
            {
                graph.DrawLine(new Pen(Color.Black, 1), j + (size - 3), close_y, Width, close_y);

                graph.DrawLine(new Pen(Color.Yellow, 20), Width - 40, close_y + 6, Width, close_y + 6);

                graph.DrawString(last.ToString(), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(Width - 40, close_y));

            }


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

    }
}
