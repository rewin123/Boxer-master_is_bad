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
    public partial class PretrainMaster : Form
    {
        Task<double> pretrain_task;
        Bitmap testImg;
        double[,,] test_arr;
        double[,,] base_arr;
        FullConPretrain pretrain;
        Optimizer opt = new MomentumParallel(NetworkData.network,0.9, 1e-3);
        public PretrainMaster()
        {
            InitializeComponent();

            Network network = NetworkData.network;

            for (int i = 0; i < network.layers.Count; i++)
            {
                listBox1.Items.Add(network.layers[i].GetType().Name + " {" + network.layers[i].output_size[0] + ","
                            + network.layers[i].output_size[1] + "," + network.layers[i].output_size[2] + "}");
            }

            test_arr = NetworkData.train.GetRandom(ref network).Key;
            base_arr = test_arr;
            testImg = ImageDataConverter.GetImage(test_arr.ConvertToRGD());
            //pictureBox1.Image = testImg;

            pretrain = new FullConPretrain(ref NetworkData.network);

            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(pretrain_task != null)
            {
                if(pretrain_task.IsCompleted)
                {
                    pretrain.UpdateWeights(ref NetworkData.network);
                    label1.Text = "Error: " + (float)pretrain_task.Result;
                    double[,,] res = pretrain.ProcessAutoEncoder(base_arr);
                    ContinuePretrain();

                    double max = -1;
                    res.ForEach((x) => { max = Math.Max(max, x); });
                    res.ForEach((x) => x / max);
                    //pictureBox2.Image = ImageDataConverter.GetImage(res.ConvertToRGD());

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
            {
                pretrain.SetupPretrain(listBox1.SelectedIndex);

                double[,,] res = pretrain.ProcessBackground(base_arr);
                double max = -1;
                res.ForEach((x) => { max = Math.Max(max, x); });
                res.ForEach((x) => x / max);
                //pictureBox1.Image = ImageDataConverter.GetImage(res.ConvertToRGD());

                RunPretrain();
                timer1.Start();
            }
        }

        void RunPretrain()
        {
            pretrain_task = Task.Run<double>(() => pretrain.TrainStart(opt,NetworkData.train,32));
        }

        void ContinuePretrain()
        {
            pretrain_task = Task.Run<double>(() => pretrain.TrainContinue(opt, NetworkData.train, 32));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(pretrain_task != null)
            {
                pretrain_task.Wait();
                pretrain.UpdateWeights(ref NetworkData.network);
                pretrain_task = null;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pretrain_task != null)
                Task.WaitAll(pretrain_task);

            pretrain.UpdateWeights(ref NetworkData.network);
            NetworkData.SaveNetwork("pretrain");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pretrain_task != null)
                pretrain_task.Wait();
            MomentumDialog mm = new MomentumDialog();
            if(mm.ShowDialog() == DialogResult.OK)
            {
                opt = mm.opt;
            }
        }
    }
}
