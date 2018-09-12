using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BoxerDLL;
using NeuralNetwork;

namespace NetworkTests
{
    public partial class NeuralTest : Form
    {
        public NeuralTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap map = new Bitmap(openFileDialog1.FileName);

                BoxerDLL.BoxCut2D cutter = new BoxCut2D(7, 10, 0.143f, 0.1f);
                List<Bitmap> maps = cutter.Cut(map);
                List<double[,,]> rgbs = new List<double[,,]>();
                for (int i = 0; i < maps.Count;i++)
                {
                    maps[i] = new Bitmap(maps[i], new System.Drawing.Size(24, 12));
                    rgbs.Add(ImageDataConverter.GetRGBArray(maps[i]));
                }


                RectangleF[] rects = cutter.Rects1D(map.Width, map.Height);

                BitmapCatEnumerator enums = new BitmapCatEnumerator("Sorted", new System.Drawing.Size(24, 12));
                BitmapCatEnumerator val = new BitmapCatEnumerator("Val", new System.Drawing.Size(24, 12));
                Network network = new Network();
                network.AddLayer(new Dropout(new Relu(), 0.05));
                network.AddLayer(new Conv2D(new Relu(), 7, 7, 32));
                network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
                network.AddLayer(new Dropout(new Relu(), 0.05));

                network.AddLayer(new Conv2D(new Relu(), 3, 3, 64));
                network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
                network.AddLayer(new Dropout(new Relu(), 0.05));

                network.AddLayer(new FullyConnLayar(new Sigmoid(), new NeuralNetwork.Size(1, 1, 256)));
                network.AddLayer(new FullyConnLayar(new Sigmoid(), new NeuralNetwork.Size(1, 1, 2)));

                network.Compile(new NeuralNetwork.Size(3, 12, 24), true);

                network.Normalization();

                pictureBox1.Image = map;

                MomentumParallel sgd = new MomentumParallel(network, 0.9, 1e-4);

                int last = 0;

                for (int i = 0;i < 1000;i++)
                {
                   var errors = sgd.TrainBatchPercent(enums, 32, 1);

                    if (errors.Last().Value > 90 || i - last > 20)
                    {

                        last = i;
                        Color[] colors = new Color[rects.Length];
                        Color plus = Color.FromArgb(100, Color.Red);
                        Color minus = Color.FromArgb(100, Color.Green);

                        for (int j = 0; j < maps.Count; j++)
                        {
                            double[,,] output = network.GetOutput(rgbs[j]);

                            colors[j] = output[0, 0, 0] > output[0, 0, 1] ? plus : minus;
                        }

                        pictureBox1.Image = DrawColoredRects(map, rects, colors);

                        var validation = network.GetError(val);
                        label2.Text = "Validation\nError: " + validation.Key + "\nPercent: " + validation.Value;
                    }
                    label1.Text = i + "\nError: " + errors.Last().Key + "\nPercent: " + errors.Last().Value;
                    Update();
                }
            }
        }

        Bitmap DrawColoredRects(Bitmap input, RectangleF[] rects, Color[] colors)
        {
            Bitmap map = new Bitmap(input);
            Graphics gr = Graphics.FromImage(map);

            for(int i = 0;i < rects.Length;i++)
            {
                
                gr.FillRectangle(new SolidBrush(colors[i]), rects[i].X,rects[i].Y,rects[i].Width,rects[i].Height);
            }

            return map;
        }
    }
}
