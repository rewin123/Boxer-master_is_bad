using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class FullConPretrain
    {
        Network network;
        Network background;
        Network autoencoder;
        int index;
        public FullConPretrain(ref Network network)
        {
            this.network = network;
        }

        public bool SetupPretrain(int i)
        {
            index = i;
            if (!network.layers[i].ITrained)
                return false;

            background = new Network();
            autoencoder = new Network();
            for (int j = 0; j < i; j++)
                background.layers.Add(network.layers[j]);

            autoencoder.AddLayer(network.layers[i]);
            if (i != network.layers.Count - 2)
            {
                autoencoder.AddLayer(new FullyConnLayar(new Sigmoid(),
                    network.layers.Last().output_size));
            }
            else autoencoder.AddLayer(network.layers.Last());
            

            background.CompileOnlyError();
            autoencoder.layers[1].Compile(autoencoder.layers[0].output_size);
            autoencoder.CompileOnlyError();
            //if (background.layers.Count != 0)
            //{
            //    autoencoder.Compile(background.layers.Last().output_size);
            //}
            //else
            //{
            //    autoencoder.Compile(network.layers[0].input_size);
            //}

            return true;
        }

        public void UpdateWeights(ref Network network)
        {
            network.layers[index] = autoencoder.layers[0];
        }

        public double[,,] ProcessAutoEncoder(double[,,] input)
        {
            return autoencoder.GetOutput(background.GetOutput(input));
        }

        public double[,,] ProcessBackground(double[,,] input)
        {
            return background.GetOutput(input);
        }

        public double TrainStart(Optimizer optimizer, IDataEnumerator data, int batch)
        {
            optimizer.network = autoencoder;
            if (background.layers.Count != 0)
            {
                NetworkEnumerator nEm = new NetworkEnumerator(background, data);

                double err = optimizer.TrainBatch(nEm, batch, 1).Last();
                return err;
            }
            else
            {
                double err = optimizer.TrainBatch(data, batch, 1).Last();
                return err;
            }

        }

        public double TrainContinue(Optimizer optimizer, IDataEnumerator data, int batch)
        {
            optimizer.network = autoencoder;
            if (background.layers.Count != 0)
            {
                NetworkEnumerator nEm = new NetworkEnumerator(background, data);

                double err = optimizer.TrainBatchContinue(nEm, batch, 1).Last();
                return err;
            }
            else
            {
                double err = optimizer.TrainBatchContinue(data, batch, 1).Last();
                return err;
            }


        }

        public KeyValuePair<double, double> TrainBatch(Optimizer optimizer, IDataEnumerator data, IDataEnumerator val, int count, int batch = 32)
        {
            DateTime start = DateTime.Now;
            NetworkEnumerator nEm = new NetworkEnumerator(background, data);

            optimizer.network = autoencoder;

            optimizer.TrainBatch(nEm, batch, 1).Last();

            for (int k = 1; k < count; k++)
            {
                optimizer.TrainBatchContinue(nEm, batch, 1);
                if ((DateTime.Now - start).TotalMinutes >= 10)
                {
                    System.IO.File.WriteAllText("autosave_" + k + ".neural", network.SaveJSON());
                    start = DateTime.Now;
                }
            }

            return autoencoder.GetError(val);
        }

        public KeyValuePair<double, double> Validation(IDataEnumerator data)
        {
            NetworkEnumerator nEm = new NetworkEnumerator(background, data);

            return autoencoder.GetError(nEm);
        }

        public static KeyValuePair<double, double> Action(Network network, Optimizer optimizer, IDataEnumerator data, IDataEnumerator val, int count, int batch = 32)
        {
            DateTime start;
            start = DateTime.Now;
            int layers = network.layers.Count;
            for (int i = 0; i < layers - 1; i++)
            {
                Network background;
                Network autoencoder;

                if (!network.layers[i].ITrained)
                    continue;

                background = new Network();
                for (int j = 0; j < i; j++)
                    background.layers.Add(network.layers[j]);

                autoencoder = new Network();
                autoencoder.layers.Add(network.layers[i]);
                autoencoder.layers.Add(network.layers[i].Mirror);

                background.CompileOnlyError();
                autoencoder.CompileOnlyError();

                NetworkEnumerator nEm = new NetworkEnumerator(background, data);

                optimizer.network = autoencoder;


                double err = optimizer.TrainBatch(nEm, batch, 1).Last();


                for (int k = 1; k < count; k++)
                {
                    Console.Write("{0} ", k);
                    optimizer.TrainBatchContinue(nEm, batch, 1);
                    if ((DateTime.Now - start).TotalMinutes >= 10)
                    {
                        System.IO.File.WriteAllText("autosave_" + i + "_" + k + ".neural", network.SaveJSON());
                        Console.WriteLine("Save to " + "autosave_" + i + "_" + k + ".neural");
                        start = DateTime.Now;
                    }
                }



                Console.WriteLine("Trained {0} with error {1}", i, (float)err);
            }

            return network.GetError(val);
        }

        public void SaveAutoencoderToFile(string path)
        {
            Network saved = new Network();
            saved.layers.AddRange(background.layers);
            saved.layers.AddRange(autoencoder.layers);

            System.IO.File.WriteAllText(path, saved.SaveJSON());
        }

    }
}
