using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork;

namespace NetworkTests
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            TestLibrary.TestDictionary dic = new TestLibrary.TestDictionary();
            //dic.tests.Add("reluValTest", ReluValTest);
            //dic.tests.Add("CompileTest", FullConnTest);
            //dic.tests.Add("Network compile", NetworkCompile);
            //dic.tests.Add("Learning test", LearningTest);
            //dic.tests.Add("Multilayer learning", OneBigLearningTest);
            //dic.tests.Add("Conv learning", ConvLearning);
            //dic.tests.Add("Cat train", CatTrain);
            //dic.tests.Add("Full con", TestFullConnect);
            dic.tests.Add("Pretrain", PreTrain);
            //dic.tests.Add("Save load", SaveLoad);

            //NeuralTest test = new NeuralTest();
            //test.ShowDialog();
            dic.ConsoleTestAll();

            Console.ReadLine();
        }

        static bool ReluValTest()
        {
            Relu relu = new Relu();
            double[,,] input = new double[1, 1, 1];
            input[0, 0, 0] = 1;
            double[,,] result = relu.Func(input);
            return result[0, 0, 0] == input[0, 0, 0];
        }

        static bool FullConnTest()
        {
            FullyConnLayar layer = new FullyConnLayar(new Relu(), new Size(10, 10, 3));
            layer.Compile(new Size(10, 10, 3));

            double[,,] input = new double[10, 10, 3];
            double[,,] output = layer.GetOutput(input);

            for(int x = 0;x < 3;x++)
            {
                for(int y = 0;y < 10;y++)
                {
                    for(int z = 0;z < 10;z++)
                    {
                        if (output[z, y, x] != input[z, y, x])
                            return false;
                    }
                }
            }

            return true;
        }

        static bool NetworkCompile()
        {
            Network network = new Network();
            network.AddLayer(new FullyConnLayar(new Relu(), new Size(1, 1, 1)));
            network.Compile(new Size(1, 1, 1));

            return true;
        }

        static bool ErrorTest()
        {
            Network network = new Network();
            network.AddLayer(new FullyConnLayar(new Relu(), new Size(1, 1, 1)));
            network.Compile(new Size(1, 1, 1));

            double[,,] input = new double[1, 1, 1];
            input[0, 0, 0] = 1;
            double[,,] t = new double[1, 1, 1];
            t[0, 0, 0] = 1;

            double val = network.GetError(input, t);


            return val == 1;
        }

        static bool LearningTest()
        {
            Network network = new Network();
            network.AddLayer(new FullyConnLayar(new Relu(), new Size(1, 1, 1)));
            network.Compile(new Size(1, 1, 1));

            double[,,] input = new double[1, 1, 1];
            input[0, 0, 0] = 1;
            double[,,] t = new double[1, 1, 1];
            t[0, 0, 0] = 1;

            double start_error = network.GetError(input, t);
            double last_error = 0;

            for(int i = 0;i < 20;i++)
            {
                last_error = network.Learn(input, t, 0.1f).Key;
                if(i % 2 == 0)
                    Console.WriteLine(last_error);
            }


            return start_error > last_error;
        }

        static bool OneBigLearningTest()
        {
            Random r = new Random();
            Network network = new Network();
            network.AddLayer(new FullyConnLayar(new Relu(), new Size(1, 1, 10)));
            network.AddLayer(new FullyConnLayar(new Relu(), new Size(1, 1, 1)));
            network.Compile(new Size(1, 1, 2));

            double[,,] input = new double[1, 1, 2];
            double[,,] t = new double[1, 1, 1];

            input[0, 0, 0] = (double)r.NextDouble();
            input[0, 0, 1] = (double)r.NextDouble();
            t[0, 0, 0] = (double)r.NextDouble();

            double start_error = network.GetError(input, t);
            double last_error = 0;

            for (int i = 0; i < 10; i++)
            {
                last_error = network.Learn(input, t, 0.1f).Key;
                Console.WriteLine(last_error);
            }


            return start_error > last_error;
        }

        static bool ConvLearning()
        {
            int width = 100;

            Random r = new Random();
            Network network = new Network();
            network.AddLayer(new Conv2D(new Relu(), 2, 1, 2));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 1));
            network.AddLayer(new FullyConnLayar(new Relu(), new Size(1, 1, 1)));
            network.Compile(new Size(1, 1, width), true);


            double[,,] input = new double[width, width, width];
            double[,,] t = new double[1, 1, 1];

            for (int i = 0; i < width; i++)
                input[i, i, i] = (double)r.NextDouble();
            t[0, 0, 0] = (double)r.NextDouble();

            double start_error = network.GetError(input, t);
            double last_error = 0;

            SGD sgd = new SGD(network, 1e-1f);

            OneEnumerator one = new OneEnumerator();
            one.input = input;
            one.output = t;

            for (int i = 0; i < 20; i++)
            {
                //last_error = network.Learn(input, t, 0.1f);
                last_error = sgd.TrainBatch(one, 1, 1)[0];
                if(i % 4 == 0)
                    Console.WriteLine(last_error);
            }


            return start_error > last_error;
        }

        static bool CatLoad()
        {
            BitmapCatEnumerator enums = new BitmapCatEnumerator("Sorted", new System.Drawing.Size(50,25));
            return true;
        }

        static bool CatTrain()
        {
            BitmapCatEnumerator enums = new BitmapCatEnumerator("Sorted", new System.Drawing.Size(50, 25));
            Network network = new Network();
            network.AddLayer(new Conv2D(new Relu(), 7, 7, 32));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            
            network.AddLayer(new Conv2D(new Relu(), 5, 5, 64));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            

            network.AddLayer(new FullyConnLayar(new Relu(), new Size(1, 1, 256)));
            network.AddLayer(new FullyConnLayar(new Sigmoid(), new Size(1, 1, 2)));

            network.Compile(new Size(3, 25, 50), true);

            network.Normalization();

            var pair = enums.GetRandom(ref network);

            OneEnumerator one = new OneEnumerator();
            one.input = pair.Key;
            one.output = pair.Value;

            MomentumParallel sgd = new MomentumParallel(network,0.9,1e-6);
            double[] errors = sgd.TrainBatch(enums, 32, 1000);

            return errors[0] > errors.Last();
        }

        static bool PreTrain()
        {
            BitmapCatEnumerator enums = new BitmapCatEnumerator("Sorted", new System.Drawing.Size(24, 12));
            BitmapCatEnumerator val = new BitmapCatEnumerator("Val", new System.Drawing.Size(24, 12));
            Network network = new Network();
            //network.LoadJSON(System.IO.File.ReadAllText("pretrained_2.neural"));
            //network.CompileOnlyError();
            network.AddLayer(new Conv2D(new Relu(), 3, 3, 10));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));

            network.AddLayer(new Conv2D(new Relu(), 5, 5, 30));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));

            network.AddLayer(new FullyConnLayar(new Relu(), new Size(1, 1, 256)));
            network.AddLayer(new FullyConnLayar(new Sigmoid(), new Size(1, 1, 2)));

            network.Compile(new Size(3, 12, 24), true);

            network.Normalization();
            network.Normalization();

            MomentumParallel sgd = new MomentumParallel(network, 0.9, 1e-4);
            sgd.need_max = false;

            var pair = PretrainAutoEncoder.Action(network, sgd, enums, val, 2000, 32);

            Console.WriteLine("{0}\n{1}", pair.Key, pair.Value);
            Console.WriteLine("Start train");

            sgd = new MomentumParallel(network, 0, 1e-5);
            DateTime start = DateTime.Now;
            sgd.TrainBatch(enums, 256, 1);
            for (int i = 0; i < 100000; i++)
            {
                double[] errors = sgd.TrainBatchContinue(enums, 256, 1);
                if((DateTime.Now - start).TotalMinutes > 5)
                {
                    System.IO.File.WriteAllText("train_" + i + ".neural", network.SaveJSON());
                    start = DateTime.Now;
                    Console.WriteLine("Saved at " + "train_" + i + ".neural");
                }
            }
            return true;
        }

        static bool TestFullConnect()
        {
            ArrDataEnumerator datas = new ArrDataEnumerator();

            //and test
            double[,,] temp = new double[1, 1, 2];
            double[,,] otemp = new double[1, 1, 1];
            datas.AddSample(temp, otemp);

            temp = new double[1, 1, 2];
            otemp = new double[1, 1, 1];

            temp[0, 0, 0] = 1;
            datas.AddSample(temp, otemp);

            temp = new double[1, 1, 2];
            otemp = new double[1, 1, 1];

            temp[0, 0, 1] = 1;
            datas.AddSample(temp, otemp);

            temp = new double[1, 1, 2];
            otemp = new double[1, 1, 1];

            temp[0, 0, 0] = 1;
            temp[0, 0, 1] = 1;
            otemp[0, 0, 0] = 1;
            datas.AddSample(temp, otemp);

            Network network = new Network();

            network.AddLayer(new TSConv2D(new Sigmoid(), 2,1,1,10,1));
            network.AddLayer(new FullyConnLayar(new Sigmoid(), new Size(1, 1, 1)));
            //network.AddLayer(new FullyConnLayar(new Sigmoid(), new Size(1, 1, 20)));
            //network.AddLayer(new FullyConnLayar(new Sigmoid(), new Size(1, 1, 1)));

            network.Compile(new Size(1, 1, 2), true);

            network.Normalization();
            //network.Normalization();

            MomentumParallel mom = new MomentumParallel(network,0.9,1e-2);
            mom.TrainBatch(datas, 320, 3000);

            return true;
        }

        static bool SaveLoad()
        {
            BitmapCatEnumerator enums = new BitmapCatEnumerator("Sorted", new System.Drawing.Size(50, 25));

            Network network = new Network();
            network.AddLayer(new Conv2D(new Relu(), 7, 7, 32));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));

            network.AddLayer(new Conv2D(new Relu(), 5, 5, 64));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));

            network.AddLayer(new FullyConnLayar(new Relu(), new Size(1, 1, 256)));
            network.AddLayer(new FullyConnLayar(new Sigmoid(), new Size(1, 1, 2)));

            network.Compile(new Size(3, 25, 50), true);

            network.Normalization();
            network.Normalization();

            string data = network.SaveJSON();

            Network network2 = new Network();
            network2.LoadJSON(data);

            var pair = enums.GetRandom(ref network);

            var out1 = network.GetOutput(pair.Key);
            var out2 = network.GetOutput(pair.Key);

            return out1[0,0,0] == out2[0,0,0];
        }
    }
}
