using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NeuralNetwork;

namespace NetworkLab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Size_select sel = new Size_select();
            sel.ShowDialog();
        }

        private void загрузитьОбучающиеДпнныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openImageDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap map = new Bitmap(100,100);
                

                string path = openImageDialog.FileName;
                path = path.Replace("\\" + openImageDialog.SafeFileName, "");
                path = path.Replace("\\" + path.Split('\\').Last(),"");

                NetworkData.train = new BitmapCatEnumerator(path, NetworkData.image_size);

                var pair = NetworkData.train.GetRandom();
                pictureBox1.Image = ImageDataConverter.GetImage(pair.Key.ConvertToRGD());
            }
        }

        private void загрузитьСтандартнуюМодельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Network network = new Network();
            
            network.AddLayer(new Conv2D(new Relu(), 3, 3, 16));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            network.AddLayer(new Conv2D(new Relu(), 3, 3, 32));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            network.AddLayer(new Conv2D(new Relu(), 3, 3, 64));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            //network.AddLayer(new FullyConnLayar(new Sigmoid(), new NeuralNetwork.Size(1, 1, 100)));
            network.AddLayer(new FullyConnLayar(new Sigmoid(), new NeuralNetwork.Size(1, 1, 2)));
            
            network.Compile(new NeuralNetwork.Size(3, NetworkData.image_size.Height, NetworkData.image_size.Width), true);

            network.Normalization();
            network.Normalization();

            NetworkData.network = network;

            string message = "";
            for(int i = 0;i < network.layers.Count;i++)
            {
                message += network.layers[i].GetType().Name + " {" + network.layers[i].output_size[0] + "," + network.layers[i].output_size[1] + "," + network.layers[i].output_size[2] + "}\n";
            }

            WriteNetwork();

            MessageBox.Show(message);
        }

        void WriteNetwork()
        {
            listBox2.Items.Clear();
            var network = NetworkData.network;   
            for (int i = 0; i < network.layers.Count; i++)
            {
                 listBox2.Items.Add(network.layers[i].GetType().Name + " {" + network.layers[i].output_size[0] + "," + network.layers[i].output_size[1] + "," + network.layers[i].output_size[2] + "}");
            }
        }

        private void мастерПредобученияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(NetworkData.network == null)
            {
                MessageBox.Show("Незагружена нейронная сеть!","Ошибка");
                return;
            }
            if(NetworkData.train == null)
            {
                MessageBox.Show("Незагружены обучающие данные","Ошибка");
                return;
            }
            PretrainMaster master = new PretrainMaster();
            Hide();
            master.ShowDialog();
            Show();
        }

        private void загрузитьИзФайлаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openNeuralDialog.ShowDialog() == DialogResult.OK)
            {
                NetworkData.LoadNetwork(openNeuralDialog.FileName);
                WriteNetwork();
            }
        }

        Task<double> task = null;
        
        double[,,] test_arr;
        double[,,] test_res;

        private void начатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(NetworkData.optimizer == null)
            {
                MessageBox.Show("Не выбран оптимизатор", "Ошибка");
                return;
            }
            
            var pair = NetworkData.train.GetRandom();
            //pictureBox1.Image = ImageDataConverter.GetImage(pair.Key.ConvertToRGD());
            test_arr = pair.Key;
            test_res = pair.Value;

            NetworkData.optimizer.TrainBatch(NetworkData.train, 128, 1);
            StartTrain();
            timer1.Start();
        }

        void StartTrain()
        {
            task = Task.Run(() =>
            {
                return NetworkData.optimizer.TrainBatch(NetworkData.train, 128, 1)[0];
            });
        }

        void ContinueTrain()
        {
            task = Task.Run(() => NetworkData.optimizer.TrainBatchContinue(NetworkData.train, 128, 1)[0]);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(task != null)
            {
                if(task.Status == TaskStatus.RanToCompletion)
                {
                    double err = task.Result;
                    
                    var res = NetworkData.network.GetOutput(test_arr);
                    ContinueTrain();
                    

                    listBox1.Items.Insert(0,test_res[0, 0, 0] + " " + test_res[0, 0, 1]);
                    listBox1.Items.Insert(0, (float)res[0, 0, 0] + " " + (float)res[0, 0, 1]);
                    listBox1.Items.Insert(0, (float)err);

                    if(listBox1.Items.Count > 1000)
                    {
                        listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                        listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                        listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                        listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                    }
                }
                else if(task.Status == TaskStatus.Faulted)
                {
                    MessageBox.Show(task.Exception.ToString());
                    task = null;
                }
            }
        }

        private void остановитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(task != null)
            {
                task.Wait();
                task = null;
            }
        }

        private void сохранитьВФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, NetworkData.network.SaveJSON());
            }
        }

        private void adamParallelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdamDialog adam = new AdamDialog();
            if(adam.ShowDialog() == DialogResult.OK)
            {
                NetworkData.optimizer = adam.opt;
            }
        }

        private void обработатьПланшетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BoxerDLL.BoxCut2D cutter = new BoxerDLL.BoxCut2D(7, 10, 0.143f, 0.1f);
            if (openImageDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap map = new Bitmap(openImageDialog.FileName);
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

                ImageShow img_show = new ImageShow(bitmap);
                img_show.Show();

            }
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

        private void momentumPaarallelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MomentumDialog adam = new MomentumDialog();
            if (adam.ShowDialog() == DialogResult.OK)
            {
                NetworkData.optimizer = adam.opt;
            }
        }

        FullConPretrain pretrain;
        Task<double> pret_task = null;

        int pretrained_layer = -1;

        private void настроитьПредобучениеСлояToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listBox2.SelectedIndex != -1)
            {
                pretrain = new FullConPretrain(ref NetworkData.network);
                pretrain.SetupPretrain(listBox2.SelectedIndex);
                pretrained_layer = listBox2.SelectedIndex;
                timer2.Start();
            }
        }

        private void предобучатьСлойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pret_task = Task<double>.Run(() => pretrain.TrainStart(NetworkData.optimizer, NetworkData.train, 64));
            pretrain_index = 0;
            min_err = double.MaxValue;
        }

        int pretrain_index = 0;
        int bad_val_count = 0;
        double min_err = double.MaxValue;

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (pret_task != null)
            {
                if (pret_task.IsCompleted)
                {
                    double err = pret_task.Result;
                    pretrain_index++;

                    if(pretrain_index % 20 == 0)
                    {
                        NetworkData.SaveNetwork("autosave_pretrain");
                        pretrain.SaveAutoencoderToFile(pretrained_layer + "_autosave_autoencoder.neural");
                        if(NetworkData.val != null)
                        {
                            var val_pair = pretrain.Validation(NetworkData.val);

                            if (min_err > val_pair.Key)
                            {
                                listBox1.Items.Insert(0, "New minimum! Save at min.neural");
                                NetworkData.SaveNetwork("min");
                                pretrain.SaveAutoencoderToFile(pretrained_layer + "_autoencoder_min.neural");
                            }

                            min_err = Math.Min(min_err, val_pair.Key);
                            listBox1.Items.Insert(0, pretrained_layer + " :Val_err: " + (float)val_pair.Key);
                            listBox1.Items.Insert(0, pretrained_layer + " :Val_win: " + (float)val_pair.Value);



                            if (val_pair.Key > 1.2 * min_err)
                            {
                                bad_val_count++;
                                listBox1.Items.Insert(0, "Bad validation " + bad_val_count);
                                if (bad_val_count > 6)
                                {
                                    listBox1.Items.Insert(0, "Stop train. Bad validation");
                                    try
                                    {
                                        listBox1.Items.Insert(0, "Загружаю наилучшую сеть по валидации");
                                        NetworkData.LoadNetwork("min.neural");
                                    }
                                    catch
                                    {
                                        listBox1.Items.Insert(0, "Ошибка загрузки!");
                                    }
                                    if (pretrained_layer < NetworkData.network.layers.Count - 2)
                                    {
                                        listBox1.Items.Insert(0, "Начинаю предобучение следующего слоя");

                                        min_err = double.MaxValue;

                                        pretrained_layer++;
                                        while (!NetworkData.network.layers[pretrained_layer].ITrained && pretrained_layer < NetworkData.network.layers.Count - 1)
                                            pretrained_layer++;

                                        if (pretrained_layer < NetworkData.network.layers.Count - 1)
                                        {
                                            listBox1.Items.Insert(0, "Начинаю предобучение следующего слоя");

                                            pretrain = new FullConPretrain(ref NetworkData.network);
                                            pretrain.SetupPretrain(pretrained_layer);
                                            pret_task = Task<double>.Run(() => pretrain.TrainStart(NetworkData.optimizer, NetworkData.train, 64));
                                        }
                                    }
                                    else
                                    {
                                        pret_task = null;
                                        return;
                                    }
                                }
                            }
                            else bad_val_count = 0;
                        }
                    }

                    //var res = NetworkData.network.GetOutput(test_arr);

                    pret_task = Task<double>.Run(() => pretrain.TrainContinue(NetworkData.optimizer, NetworkData.train, 64));


                    //listBox1.Items.Insert(0, test_res[0, 0, 0] + " " + test_res[0, 0, 1]);
                    //listBox1.Items.Insert(0, (float)res[0, 0, 0] + " " + (float)res[0, 0, 1]);
                    listBox1.Items.Insert(0, pretrained_layer + " : " + (float)err);

                    if (listBox1.Items.Count > 1000)
                    {
                        listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                        listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                        listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                        listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                    }

                    
                }
            }
        }

        private void остановитьПредобучениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pret_task != null)
            {
                pret_task.Wait();
                pret_task = null;
            }
        }

        private void обработатьПланшетПреобученнойСетьюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BoxerDLL.BoxCut2D cutter = new BoxerDLL.BoxCut2D(7, 10, 0.143f, 0.1f);
            if (openImageDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap map = new Bitmap(openImageDialog.FileName);
                RectangleF[] rects = cutter.Rects1D(map.Width, map.Height);
                Color[] colors = new Color[rects.Length];
                Color plus = Color.FromArgb(100, Color.Red);
                Color minus = Color.FromArgb(100, Color.Green);
                var maps = cutter.Enumerator(ref map);
                int j = 0;
                //List<double[,,]> rgbs = new List<double[,,]>();
                do
                {
                    double[,,] output = pretrain.ProcessAutoEncoder(ImageDataConverter.GetRGBArray(new Bitmap( maps.Current, NetworkData.image_size)));
                    GC.Collect();
                    colors[j] = output[0, 0, 0] > output[0, 0, 1] ? plus : minus;
                    j++;
                } while (maps.MoveNext());
                
                var bitmap = DrawColoredRects(map, rects, colors);

                ImageShow img_show = new ImageShow(bitmap);
                img_show.Show();
                
            }
        }

        private void загрузитьБольшуюМодельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Network network = new Network();

            network.AddLayer(new Conv2D(new Relu(), 5, 5, 16));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            network.AddLayer(new Conv2D(new Relu(), 5, 5, 20));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            network.AddLayer(new Conv2D(new Relu(), 3, 3, 32));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            network.AddLayer(new Conv2D(new Relu(), 3, 3, 54));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            network.AddLayer(new Conv2D(new Relu(), 3, 3, 80));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            network.AddLayer(new FullyConnLayar(new Sigmoid(), new NeuralNetwork.Size(1, 1, 100)));
            network.AddLayer(new FullyConnLayar(new Sigmoid(), new NeuralNetwork.Size(1, 1, 2)));

            network.Compile(new NeuralNetwork.Size(3, NetworkData.image_size.Height, NetworkData.image_size.Width), true);

            network.Normalization();
            network.Normalization();

            NetworkData.network = network;

            string message = "";
            for (int i = 0; i < network.layers.Count; i++)
            {
                message += network.layers[i].GetType().Name + " {" + network.layers[i].output_size[0] + "," + network.layers[i].output_size[1] + "," + network.layers[i].output_size[2] + "}\n";
            }

            WriteNetwork();

            MessageBox.Show(message);
        }

        private void загрузитьВалидационныеДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openImageDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openImageDialog.FileName;
                path = path.Replace("\\" + openImageDialog.SafeFileName, "");
                path = path.Replace("\\" + path.Split('\\').Last(), "");

                NetworkData.val = new BitmapCatEnumerator(path, NetworkData.image_size);

                var pair = NetworkData.val.GetRandom();
                pictureBox1.Image = ImageDataConverter.GetImage(pair.Key.ConvertToRGD());
            }
        }

        private void визуализаторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkVisual vis = new NetworkVisual();
            vis.Show();
        }

        private void данныеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void eToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openImageDialog.Multiselect = true;
            if (openImageDialog.ShowDialog() == DialogResult.OK)
            {
                NeuralSort sort = new NeuralSort(openImageDialog.FileNames);
                sort.Show();
            }
        }

        private void загрузитьФинансовыеДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openImageDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openImageDialog.FileName);
                sr.ReadLine();

                TimeData data = new TimeData(10);
                while(!sr.EndOfStream)
                {
                    string[] data_line = sr.ReadLine().Replace('.',',').Split(' ');
                    double[,,] data_arr = new double[1, 1, 4];
                    data_arr[0, 0, 0] = double.Parse(data_line[4]);
                    data_arr[0, 0, 1] = double.Parse(data_line[5]);
                    data_arr[0, 0, 2] = double.Parse(data_line[6]);
                    data_arr[0, 0, 3] = double.Parse(data_line[7]);

                    data.AddTimeFrame(data_arr);
                }

                NetworkData.train = data;
            }
        }

        private void финСетьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Network network = new Network();
            network.AddLayer(new FullyConnLayar(new Relu(), new NeuralNetwork.Size(1, 1, 400)));
            network.AddLayer(new MaxPool2D(new SimpleFunc(), 2, 1));
            network.AddLayer(new FullyConnLayar(new Relu(), new NeuralNetwork.Size(1, 1, 100)));
            network.AddLayer(new MaxPool2D(new SimpleFunc(), 2, 1));
            network.AddLayer(new FullyConnLayar(new Relu(), new NeuralNetwork.Size(1, 1, 4)));
            network.Compile(new NeuralNetwork.Size(10, 1, 4));

            //network.Normalization();
            //network.Normalization();

            NetworkData.network = network;

            WriteNetwork();
        }

        private void загрузитьОбучающиеДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openImageDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openImageDialog.FileName;
                path = path.Replace("\\" + openImageDialog.SafeFileName, "");
                path = path.Replace("\\" + path.Split('\\').Last(), "");

                NetworkData.train = new BitmapCatEnumerator(path, NetworkData.image_size);

                var pair = NetworkData.train.GetRandom();
                pictureBox1.Image = ImageDataConverter.GetImage(pair.Key.ConvertToRGD());
            }
        }

        private void загрузитьПрограммДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openImageDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openImageDialog.FileName;
                path = path.Replace("\\" + openImageDialog.SafeFileName, "");
                path = path.Replace("\\" + path.Split('\\').Last(), "");

                NetworkData.train = new ProcessCatEnumerator(path,"Updown.exe",4);

                var pair = NetworkData.train.GetRandom();
                //pictureBox1.Image = ImageDataConverter.GetImage(pair.Key.ConvertToRGD());
            }
        }

        private void входнаяСетьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Network network = new Network();
            network.AddLayer(new FullyConnLayar(new Relu(), new NeuralNetwork.Size(1, 1, 100)));
            network.AddLayer(new MaxPool2D(new SimpleFunc(), 2, 1));
            network.AddLayer(new FullyConnLayar(new Sigmoid(), new NeuralNetwork.Size(1, 1, 2)));
            network.Compile(new NeuralNetwork.Size(1, 1, 4));

            //network.Normalization();
            //network.Normalization();

            NetworkData.network = network;

            WriteNetwork();
        }

        private void тестСкоростиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Network network = new Network();

            network.AddLayer(new Conv2D(new Relu(), 3, 3, 16));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            network.AddLayer(new Conv2D(new Relu(), 3, 3, 32));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            network.AddLayer(new Conv2D(new Relu(), 3, 3, 64));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            //network.AddLayer(new FullyConnLayar(new Sigmoid(), new NeuralNetwork.Size(1, 1, 100)));
            network.AddLayer(new FullyConnLayar(new Sigmoid(), new NeuralNetwork.Size(1, 1, 2)));

            network.Compile(new NeuralNetwork.Size(3, NetworkData.image_size.Height, NetworkData.image_size.Width), true);

            network.Normalization();
            network.Normalization();
            

            DateTime start = DateTime.Now;
            double[,,] input = new double[3, NetworkData.image_size.Height, NetworkData.image_size.Width];
            int count = 0;
            while((DateTime.Now - start).TotalSeconds < 5)
            {
                network.GetOutput(input);
                count++;
            }

            listBox1.Items.Insert(0, "Count: " + count);
        }
    }
}
