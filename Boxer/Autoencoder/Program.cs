using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NetworkLab;
using NeuralNetwork;
using NeuralNetwork.Activations;
using NeuralNetwork.Callbacks;
using NeuralNetwork.Layers;
using NeuralNetwork.Losses;
using NeuralNetwork.Metrics;

namespace Autoencoder
{
    class Program
    {
        static void Main(string[] args)
        {
            //PredictorClasses();
            LinePredictor();
        }

        
        static void PredictorClasses()
        {
            StreamReader rd = new StreamReader("data.txt");
            string classes_line = rd.ReadLine();
            string[] classes = classes_line.Split(' ');
            List<double[,,]> inputs = new List<double[,,]>();
            List<double[,,]> outputs = new List<double[,,]>();
            while (!rd.EndOfStream)
            {
                inputs.Add(LineToArr3(rd.ReadLine()));
                outputs.Add(LineToArr3(rd.ReadLine()));
            }

            int output_len = outputs[0].Length;

            int nancount = 0;
            for (int i = 0; i < inputs.Count; i++)
            {
                var l = inputs[i];
                for (int j = 0; j < l.Length; j++)
                {
                    if (double.IsNaN(l[0, 0, j]) || double.IsInfinity(l[0, 0, j]))
                    {
                        nancount++;
                        inputs.RemoveAt(i);
                        outputs.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }


            ArrDataEnumerator enumerator = new ArrDataEnumerator(inputs, outputs);

            var network = new Network();
            network.LoadJSON(File.ReadAllText("network.neural"));
            List<int> vals = new List<int>();
            List<int> train_vals = new List<int>();
            
            foreach(var pair in enumerator)
            {
                var output = network.GetOutput(pair.Key);
                var true_output = pair.Value;
                output.ArgMax(out int c, out int y, out int x);
                true_output.ArgMax(out int tc, out int ty, out int tx);
                if (vals.Count > x)
                {
                    vals[x]++;
                }
                else
                {
                    while (vals.Count <= x)
                        vals.Add(0);
                    vals[x]++;
                }

                if (train_vals.Count > tx)
                {
                    train_vals[tx]++;
                }
                else
                {
                    while (train_vals.Count <= tx)
                        train_vals.Add(0);
                    train_vals[tx]++;
                }
            }

            ;

            Console.ReadLine();
        }

        static void LinePredictor()
        {
            StreamReader rd = new StreamReader("data.txt");
            string classes_line = rd.ReadLine();
            string[] classes = classes_line.Split(' ');
            List<double[,,]> inputs = new List<double[,,]>();
            List<double [,,]> outputs = new List<double[,,]>();
            while (!rd.EndOfStream)
            {
                inputs.Add(LineToArr3(rd.ReadLine()));
                outputs.Add(LineToArr3(rd.ReadLine()));
            }

            int output_len = outputs[0].Length;

            int nancount = 0;
            for (int i = 0; i < inputs.Count; i++)
            {
                var l = inputs[i];
                for (int j = 0; j < l.Length; j++)
                {
                    if (double.IsNaN(l[0, 0, j]) || double.IsInfinity(l[0, 0, j]))
                    {
                        nancount++;
                        inputs.RemoveAt(i);
                        outputs.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }


            ArrDataEnumerator enumerator = new ArrDataEnumerator(inputs, outputs);

            DataCategoryVisualisation visualisation = new DataCategoryVisualisation(enumerator, 2, 6, 600, classes);
            visualisation.ShowDialog();

            Network network = new Network();
            network.loss = new MaxLoss();
            network.AddLayer(new FullyConnLayar(new LeackyRelu(0.01), new NeuralNetwork.Size(1, 1, 64)));
            network.AddLayer(new FullyConnLayar(new Sigmoid(), new NeuralNetwork.Size(1, 1, output_len)));
            network.Compile(new NeuralNetwork.Size(1, 1, inputs[0].GetLength(2)), true);
            network.AddNoise();

            Optimizer parallel = new MomentumParallel(network, 0.9, 0.0005);

            enumerator.SplitTrainVal(0.8, out ArrDataEnumerator train, out ArrDataEnumerator val);
            Learner learner = new Learner()
            {
                optimizer = parallel,
                train_data = train,
                val_data = val
            };

            learner.metrics.Add(new ArgMaxMetrics());
            learner.metrics.Add(new ArgmaxCount(0, 0, 4));
            learner.metrics.Add(new ArgmaxTrainCount(0, 0, 4));
            learner.callbacks.Add(new MetricSaveCallback("val_argmax"));

            learner.Learn(8, 50, 10000);

        }

        static double[] LineToArr(string line)
        {
            var ar = line.Split(' ');
            int len = ar.Length;
            double[] arr = new double[ar.Length];
            for (int i = 0; i < len; i++)
            {
                arr[i] = double.Parse(ar[i]);
            }
            return arr;
        }

        static double[,,] LineToArr3(string line)
        {
            var ar = line.Split(' ');
            int len = ar.Length;
            double[,,] arr = new double[1, 1, ar.Length];
            for (int i = 0; i < len; i++)
            {
                arr[0, 0, i] = double.Parse(ar[i]);
            }
            return arr;
        }

        static void Test()
        {
            BitmapCatEnumerator enumerator = new BitmapCatEnumerator("Sorted", new System.Drawing.Size(100, 300));
            NeuralNetwork.MirrorEnumerator mirror = new MirrorEnumerator(enumerator);

            Network network = new Network();
            network.LoadJSON(File.ReadAllText("test_neural.neural"));
            network.CompileOnlyError();

            ImageShow show = new ImageShow(network.GetOutput(mirror.Current.Key).GetImage());
            show.ShowDialog();
        }
    }
}
