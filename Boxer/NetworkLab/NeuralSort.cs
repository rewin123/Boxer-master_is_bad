using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NeuralNetwork;

namespace NetworkLab
{
    public partial class NeuralSort : Form
    {
        BoxerDLL.BoxCut2D cutter = new BoxerDLL.BoxCut2D(7, 10, 0.143f, 0.1f);
        string[] planes;
        int index = 0;
        public NeuralSort(string[] planes)
        {
            InitializeComponent();
            this.planes = planes;
            Translate();
        }

        void Translate()
        {
            Bitmap map = new Bitmap(planes[index]);
            RectangleF[] rects = cutter.Rects1D(map.Width, map.Height);
            Color[] colors = new Color[rects.Length];
            Color plus = Color.FromArgb(100, Color.Red);
            Color minus = Color.FromArgb(100, Color.Green);
            var maps = cutter.Enumerator(ref map);
            int j = 0;
            //List<double[,,]> rgbs = new List<double[,,]>();
            do
            {
                double[,,] output = NetworkData.network.GetOutput(ImageDataConverter.GetRGBArray(new Bitmap(maps.Current, NetworkData.image_size)));
                GC.Collect();
                colors[j] = output[0, 0, 0] > output[0, 0, 1] ? plus : minus;
                j++;
            } while (maps.MoveNext());

            var bitmap = DrawColoredRects(map, rects, colors);
            pictureBox1.Image = bitmap;
        }

        Bitmap DrawColoredRects(Bitmap input, RectangleF[] rects, Color[] colors)
        {
            Bitmap map = new Bitmap(input);
            Graphics gr = Graphics.FromImage(map);

            for (int i = 0; i < rects.Length; i++)
            {

                gr.FillRectangle(new SolidBrush(colors[i]), rects[i].X, rects[i].Y, rects[i].Width, rects[i].Height);
            }

            return map;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
