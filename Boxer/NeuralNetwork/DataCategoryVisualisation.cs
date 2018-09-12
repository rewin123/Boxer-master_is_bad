using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetwork
{
    public partial class DataCategoryVisualisation : Form
    {
        ArrDataEnumerator enumerator;
        int channel1;
        int channel2;
        float lineDistance = 5;
        public DataCategoryVisualisation(ArrDataEnumerator enumerator, int channel1, int channel2, int size, string[] classes)
        {
            InitializeComponent();

            this.enumerator = enumerator;
            this.channel1 = channel1;
            this.channel2 = channel2;

            double min1 = double.MaxValue;
            double min2 = double.MaxValue;
            double max1 = double.MinValue;
            double max2 = double.MinValue;

            double mean1 = 0;
            double mean2 = 0;
            int count = 0;
            enumerator.ProcessChannel(0, 0, channel1, v =>
              {
                  min1 = Math.Min(min1, v);
                  max1 = Math.Max(max1, v);
                  mean1 += v;
                  count++;
              });

            mean1 /= count;

            enumerator.ProcessChannel(0, 0, channel2, v =>
            {
                min2 = Math.Min(min2, v);
                max2 = Math.Max(max2, v);
                mean2 += v;
            });
            mean2 /= count;

            Bitmap map = new Bitmap(size, size);
            Graphics gr = Graphics.FromImage(map);
            Brush[] brushes = new Brush[] { Brushes.Black, Brushes.Red, Brushes.Gray, Brushes.Green, Brushes.Yellow, Brushes.Pink, Brushes.Black, Brushes.Aqua, Brushes.Purple, Brushes.Wheat, Brushes.Brown, Brushes.Chocolate };
            List<int> xs = new List<int>();
            List<int> ys = new List<int>();
            List<int> brushes_list = new List<int>();
            enumerator.Process2Channel(0, 0, channel1, 0, 0, channel2, (v1, v2, c, y, x) =>
                 {
                     //int x1 = (int)((v1 - min1) / (max1 - min1) * size);
                     //int y1 = (int)((v2 - min2) / (max2 - min2) * size);
                     //gr.FillEllipse(brushes[x], x1 - 5, y1 - 5, 10, 10);

                     xs.Add((int)((v1 - min1) / (max1 - min1) * size));
                     ys.Add((int)((v2 - min2) / (max2 - min2) * size));
                     brushes_list.Add(x);
                 });
            Random r = new Random();
            while(xs.Count > 0)
            {
                int pos = r.Next(xs.Count);
                gr.FillEllipse(brushes[brushes_list[pos]], xs[pos] - 5, ys[pos] - 5, 10, 10);
                xs.RemoveAt(pos);
                ys.RemoveAt(pos);
                brushes_list.RemoveAt(pos);
            }

            float shift = 0;
            for(int i = 0;i < classes.Length;i++)
            {
                gr.DrawString(classes[i], Font, Brushes.Black, 0, shift);
                var s = gr.MeasureString(classes[i], Font);
                gr.DrawRectangle(new Pen(brushes[i], 3), -1, shift, s.Width, s.Height);
                shift += s.Height + lineDistance;
            }
            pictureBox1.Image = map;
        }
    }
}
