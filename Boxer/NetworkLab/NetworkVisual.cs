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
    public partial class NetworkVisual : Form
    {
        NeuralNetwork.NetworkVisualizer vis = null;

        public NetworkVisual()
        {
            InitializeComponent();

            listBox1.Items.AddRange(WriteNetwork());
        }

        string[] WriteNetwork()
        {
            List<string> list = new List<string>();
            var network = NetworkData.network;
            for (int i = 0; i < network.layers.Count; i++)
            {
                list.Add(network.layers[i].GetType().Name + " {" + network.layers[i].output_size[0] + "," + network.layers[i].output_size[1] + "," + network.layers[i].output_size[2] + "}");
            }

            return list.ToArray();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (vis != null)
            {
                listBox2.Items.Clear();
                int count = NetworkData.network.layers[listBox1.SelectedIndex].output_size[0];
                for (int i = 0; i < count; i++)
                {
                    listBox2.Items.Add(i.ToString());
                }
                vis.layer = listBox1.SelectedIndex;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                vis.channel = listBox2.SelectedIndex;
                pictureBox1.Image = vis.Current;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                vis = new NeuralNetwork.NetworkVisualizer(NetworkData.network,
                    ImageDataConverter.GetRGBArray(new Bitmap(new Bitmap(openFileDialog1.FileName), NetworkData.image_size)));

                pictureBox2.Image = new Bitmap(new Bitmap(openFileDialog1.FileName), NetworkData.image_size);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            vis.SaveWeights("ws.txt");
        }
    }
}
