using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Pair_Scalp_2._0.Classes
{
    public class Draws_Bars_Class
    {
        public Pair_work pair_Work;
        public List<bars_Class> Bar;
        public int Bar_ch;
        public int LineX = 0;
        public int LineY = 0;
        public double Line_Price = 0;
        

        public bool true_Price = false;

        public void Draws_Bars(PictureBox pictureBox1, int st, int sp)
        {

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics graph = Graphics.FromImage(bmp);
            int Width = pictureBox1.Width;
            int Height = pictureBox1.Height;
            int size = (Width - 20) / (Bar_ch);
            int j = 0;

            double max = Bar[sp - 1].high;
            double min = Bar[sp - 1].low;


            #region // отрисовка баров

            for (int i = st; i < sp; i++)
            {
                if (Bar[i].Dis_F)
                {
                    if (Bar[i].high > max && Bar[i].close != 0) { max = Bar[i].high; }
                    if (Bar[i].low < min && Bar[i].close != 0) { min = Bar[i].low; }

                    if (Bar[i].low > max && Bar[i].close != 0) { max = Bar[i].low; }
                    if (Bar[i].high < min && Bar[i].close != 0) { min = Bar[i].high; }
                }
            }

            double mm = (max - min) / 10;

            max = max + mm;
            min = min - mm;

            if (max == min) return;


            for (int i = st; i < sp; i++)
            {
                if (!Bar[i].Dis_F || Bar[i].close == 0 || min == max) { continue; }

                j = j + size;

                Bar[i].Draw_Bar_Price(graph, size, j, Height, max, min);
                Bar[i].indicator_MA_high_low(graph, size, j, Height, max, min);

                if (Bar.Count - 1 == i) // тик цены на графеке
                {
                   
                    Bar[i].Draw_Last_Price(graph, size, j, Height, Width, max, min, pair_Work.Price_last);

                }

            }

            #endregion


            #region // линии на графике1

            if(true_Price) Line_Price = LineY_in_Price(LineY, Height, max, min);

         
            Draw_Price(graph, Height, Width, max, min);

            Draw_OpenOrder(graph, Height, Width, max, min);

            Draw_entryPrice(graph, Height, Width, max, min);

            //graph.DrawLine(new Pen(Color.Black, 2), 0, LineY, Width, LineY);
            //graph.DrawLine(new Pen(Color.White, 20), 0, LineY + 6, 47, LineY + 6);
            //graph.DrawString(Price(LineY, Height, max, min), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, LineY));

            graph.DrawLine(new Pen(Color.Black, 2), LineX, 0, LineX, Height);
            #endregion

            #region // отрисовка  рамка 

            graph.DrawLine(new Pen(Color.Black, 2), 0, 0, 0, Height);
            graph.DrawLine(new Pen(Color.Black, 2), 0, 0, Width, 0);
            graph.DrawLine(new Pen(Color.Black, 2), 0, Height, Width, Height);
            graph.DrawLine(new Pen(Color.Black, 2), Width, 0, Width, Height);

            #endregion

            pictureBox1.Image = bmp;

        }

        public void Draw_entryPrice(Graphics graph, int Height, int Width, double max, double min)
        {
            if (pair_Work.entry_Price.Price != 0 && pair_Work.entry_Price.Buy_Sell == "B")
            {
                float close_y = (float)(Height - (pair_Work.entry_Price.Price - min) * (Height / (max - min)));

                graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
                graph.DrawLine(new Pen(Color.Green, 20), 0, close_y + 6, 47, close_y + 6);

                graph.DrawString(pair_Work.entry_Price.Price.ToString("0.###"), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));
            }
            if (pair_Work.entry_Price.Price != 0 && pair_Work.entry_Price.Buy_Sell == "S")
            {
                float close_y = (float)(Height - (pair_Work.entry_Price.Price - min) * (Height / (max - min)));

                graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
                graph.DrawLine(new Pen(Color.Red, 20), 0, close_y + 6, 47, close_y + 6);

                graph.DrawString(pair_Work.entry_Price.Price.ToString("0.###"), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));
            }


            //if (true_Price_buy)
            //{
            //    if (entry_Price.Price != 0)
            //    {
            //        float close_y = (float)(Height - (entry_Price.Price - min) * (Height / (max - min)));

            //        graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
            //        graph.DrawLine(new Pen(Color.GreenYellow, 20), 0, close_y + 6, 47, close_y + 6);

            //        graph.DrawString(entry_Price.Price.ToString("0.###"), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));
            //    }

            //    if (entry_Price.stop_sell_Price != 0)
            //    {
            //        float close_y = (float)(Height - (entry_Price.stop_sell_Price - min) * (Height / (max - min)));

            //        graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
            //        graph.DrawLine(new Pen(Color.Coral, 20), 0, close_y + 6, 47, close_y + 6);

            //        graph.DrawString(entry_Price.stop_sell_Price.ToString("0.###") + " S", new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));
            //    }

            //    if (entry_Price.profit_sell_Price != 0)
            //    {
            //        float close_y = (float)(Height - (entry_Price.profit_sell_Price - min) * (Height / (max - min)));

            //        graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
            //        graph.DrawLine(new Pen(Color.Coral, 20), 0, close_y + 6, 47, close_y + 6);

            //        graph.DrawString(entry_Price.profit_sell_Price.ToString("0.###") + " P", new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));
            //    }

            //}

            //if (true_Price_sell) 
            //{
            //    if (entry_Price.entry_sell_Price != 0)
            //    {
            //        float close_y = (float)(Height - (entry_Price.entry_sell_Price - min) * (Height / (max - min)));

            //        graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
            //        graph.DrawLine(new Pen(Color.Pink, 20), 0, close_y + 6, 47, close_y + 6);

            //        graph.DrawString(entry_Price.entry_sell_Price.ToString("0.###"), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));
            //    }

            //    if (entry_Price.stop_buy_Price != 0)
            //    {
            //        float close_y = (float)(Height - (entry_Price.stop_buy_Price - min) * (Height / (max - min)));

            //        graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
            //        graph.DrawLine(new Pen(Color.Aqua, 20), 0, close_y + 6, 47, close_y + 6);

            //        graph.DrawString(entry_Price.stop_buy_Price.ToString("0.###") + " S", new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));
            //    }

            //    if (entry_Price.profit_buy_Price != 0)
            //    {
            //        float close_y = (float)(Height - (entry_Price.profit_buy_Price - min) * (Height / (max - min)));

            //        graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
            //        graph.DrawLine(new Pen(Color.Aqua, 20), 0, close_y + 6, 47, close_y + 6);

            //        graph.DrawString(entry_Price.profit_buy_Price.ToString("0.###") + " P", new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));
            //    }

            //}

        }

        public void Draw_OpenOrder(Graphics graph, int Height, int Width, double max, double min)
        {
            for (int i = 0; i < pair_Work.order_work.Count; i++)
            {
                if (pair_Work.order_work[i].order_wd)
                {
                    if (pair_Work.order_work[i].type == "B_LT")
                    {
                        float close_y = (float)(Height - (pair_Work.order_work[i].price - min) * (Height / (max - min)));
                        graph.DrawLine(new Pen(Color.Silver, 1), 0, close_y, Width, close_y);
                        graph.DrawLine(new Pen(Color.Silver, 20), 0, close_y + 6, 47, close_y + 6);
                        graph.DrawString(pair_Work.order_work[i].price.ToString("0.###") + " B", new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));

                    }

                    if (pair_Work.order_work[i].type == "S_LT")
                    {
                        float close_y = (float)(Height - (pair_Work.order_work[i].price - min) * (Height / (max - min)));

                        graph.DrawLine(new Pen(Color.Silver, 1), 0, close_y, Width, close_y);

                        graph.DrawLine(new Pen(Color.Silver, 20), 0, close_y + 6, 47, close_y + 6);
                        graph.DrawString(pair_Work.order_work[i].price.ToString("0.###") + " S", new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));

                    }

                    if (pair_Work.order_work[i].type == "B_TP")
                    {
                        float close_y = (float)(Height - (pair_Work.order_work[i].price - min) * (Height / (max - min)));

                      
                       graph.DrawLine(new Pen(Color.Silver, 1), 0, close_y, Width, close_y);

                        graph.DrawLine(new Pen(Color.Silver, 20), 0, close_y + 6, 57, close_y + 6);
                        graph.DrawString(pair_Work.order_work[i].price.ToString("0.###") + " TP", new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));

                    }

                    if (pair_Work.order_work[i].type == "B_SL")
                    {
                        float close_y = (float)(Height - (pair_Work.order_work[i].price - min) * (Height / (max - min)));

                      
                        graph.DrawLine(new Pen(Color.Silver, 1), 0, close_y, Width, close_y);

                        graph.DrawLine(new Pen(Color.Silver, 20), 0, close_y + 6, 57, close_y + 6);
                        graph.DrawString(pair_Work.order_work[i].price.ToString("0.###") + " SL", new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));

                    }

                    if (pair_Work.order_work[i].type == "S_TP")
                    {
                        float close_y = (float)(Height - (pair_Work.order_work[i].price - min) * (Height / (max - min)));

                   
                       graph.DrawLine(new Pen(Color.Silver, 1), 0, close_y, Width, close_y);

                        graph.DrawLine(new Pen(Color.Silver, 20), 0, close_y + 6, 57, close_y + 6);
                        graph.DrawString(pair_Work.order_work[i].price.ToString("0.###") + " TP", new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));

                    }

                    if (pair_Work.order_work[i].type == "S_SL")
                    {
                        float close_y = (float)(Height - (pair_Work.order_work[i].price - min) * (Height / (max - min)));

                     
                        graph.DrawLine(new Pen(Color.Silver, 1), 0, close_y, Width, close_y);

                        graph.DrawLine(new Pen(Color.Silver, 20), 0, close_y + 6, 57, close_y + 6);
                        graph.DrawString(pair_Work.order_work[i].price.ToString("0.###") + " SL", new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));

                    }

                }

                if (pair_Work.order_work[i].order_worked)
                {               
                    if (pair_Work.order_work[i].type == "B_LT")
                    {
                        float close_y = (float)(Height - (pair_Work.order_work[i].price - min) * (Height / (max - min)));

                        if (!pair_Work.order_work[i].selective) graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
                        if (pair_Work.order_work[i].selective) graph.DrawLine(new Pen(Color.Silver, 4), 0, close_y, Width, close_y);

                        graph.DrawLine(new Pen(Color.GreenYellow, 20), 0, close_y + 6, 47, close_y + 6);
                        graph.DrawString(pair_Work.order_work[i].price.ToString("0.###"), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));

                    }

                    if (pair_Work.order_work[i].type == "S_LT")
                    {
                        float close_y = (float)(Height - (pair_Work.order_work[i].price - min) * (Height / (max - min)));

                        if (!pair_Work.order_work[i].selective) graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
                        if (pair_Work.order_work[i].selective) graph.DrawLine(new Pen(Color.Silver, 4), 0, close_y, Width, close_y);

                        graph.DrawLine(new Pen(Color.Pink, 20), 0, close_y + 6, 47, close_y + 6);
                        graph.DrawString(pair_Work.order_work[i].price.ToString("0.###"), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));

                    }

                    if (pair_Work.order_work[i].type == "B_TP")
                    {
                        float close_y = (float)(Height - (pair_Work.order_work[i].price - min) * (Height / (max - min)));

                        if (!pair_Work.order_work[i].selective) graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
                        if (pair_Work.order_work[i].selective) graph.DrawLine(new Pen(Color.Silver, 4), 0, close_y, Width, close_y);

                        graph.DrawLine(new Pen(Color.Aqua, 20), 0, close_y + 6, 57, close_y + 6);
                        graph.DrawString(pair_Work.order_work[i].price.ToString("0.###") + " TP", new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));

                    }

                    if (pair_Work.order_work[i].type == "B_SL")
                    {
                        float close_y = (float)(Height - (pair_Work.order_work[i].price - min) * (Height / (max - min)));

                        if (!pair_Work.order_work[i].selective) graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
                        if (pair_Work.order_work[i].selective) graph.DrawLine(new Pen(Color.Silver, 4), 0, close_y, Width, close_y);

                        graph.DrawLine(new Pen(Color.Aqua, 20), 0, close_y + 6, 57, close_y + 6);
                        graph.DrawString(pair_Work.order_work[i].price.ToString("0.###")+" SL", new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));

                    }

                    if (pair_Work.order_work[i].type == "S_TP")
                    {
                        float close_y = (float)(Height - (pair_Work.order_work[i].price - min) * (Height / (max - min)));

                        if (!pair_Work.order_work[i].selective) graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
                        if (pair_Work.order_work[i].selective) graph.DrawLine(new Pen(Color.Silver, 4), 0, close_y, Width, close_y);

                        graph.DrawLine(new Pen(Color.Coral, 20), 0, close_y + 6, 57, close_y + 6);
                        graph.DrawString(pair_Work.order_work[i].price.ToString("0.###") + " TP", new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));

                    }

                    if (pair_Work.order_work[i].type == "S_SL")
                    {
                        float close_y = (float)(Height - (pair_Work.order_work[i].price - min) * (Height / (max - min)));

                        if (!pair_Work.order_work[i].selective) graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
                        if (pair_Work.order_work[i].selective) graph.DrawLine(new Pen(Color.Silver, 4), 0, close_y, Width, close_y);

                        graph.DrawLine(new Pen(Color.Coral, 20), 0, close_y + 6, 57, close_y + 6);
                        graph.DrawString(pair_Work.order_work[i].price.ToString("0.###") + " SL", new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));

                    }

                }
            }
           
        }

        public void Draw_Price(Graphics graph, int Height, int Width, double max, double min)
        {
            if (Line_Price != 0)
            {
                float close_y = (float)(Height - (Line_Price - min) * (Height / (max - min)));

                graph.DrawLine(new Pen(Color.White, 2), 0, close_y, Width, close_y);
                graph.DrawLine(new Pen(Color.White, 20), 0, close_y + 6, 47, close_y + 6);

                graph.DrawString(Line_Price.ToString("0.###"), new Font("Tahoma", 8, FontStyle.Regular), Brushes.Black, new PointF(0, close_y));
            }

        }

        public string Price(float y, int Height, double max, double min)
        {

            double p = (Height - y) / (Height / (max - min)) + min;

            return p.ToString("0.####");

        }

        public double LineY_in_Price(float y, int Height, double max, double min)
        {
            return (Height - y) / (Height / (max - min)) + min;
        }

        public void Line_P(ref float Line_y, double Price_y, int Height, double max, double min)
        {

            Line_y = (float)(Height - (Price_y - min) * (Height / (max - min)));


        }


    }
}
