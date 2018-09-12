using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class PretrainAutoEncoder
    {
        Network network;
        Network background;
        Network autoencoder;
        int index;
        public PretrainAutoEncoder(ref Network network)
        {
            this.network = network;
        }

        public bool SetupPretrain(int i)
        {
            index = i;
            if (!network.layers[i].ITrained)
                return false;

            background = new Network();
            for (int j = 0; j < i; j++)
                background.layers.Add(network.layers[j]);

            autoencoder = new Network();
            autoencoder.layers.Add(network.layers[i]);
            autoencoder.layers.Add(network.layers[i].Mirror);

            background.CompileOnlyError();
            autoencoder.CompileOnlyError();

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
            NetworkEnumerator nEm = new NetworkEnumerator(background, data);
            MirrorEnumerator Mem = new MirrorEnumerator(nEm);

            optimizer.network = autoencoder;
            double err = optimizer.TrainBatch(Mem, batch, 1).Last();

            return err;
        }

        public double TrainContinue(Optimizer optimizer, IDataEnumerator data, int batch)
        {
            NetworkEnumerator nEm = new NetworkEnumerator(background, data);
            MirrorEnumerator Mem = new MirrorEnumerator(nEm);

            optimizer.network = autoencoder;
            double err = optimizer.TrainBatchContinue(Mem, batch, 1).Last();

            return err;
        }

        public KeyValuePair<double,double> TrainBatch(Optimizer optimizer, IDataEnumerator data, IDataEnumerator val, int count, int batch = 32)
        {
            DateTime start = DateTime.Now;
            NetworkEnumerator nEm = new NetworkEnumerator(background, data);
            MirrorEnumerator Mem = new MirrorEnumerator(nEm);

            optimizer.network = autoencoder;

            optimizer.TrainBatch(Mem, batch, 1).Last();

            for (int k = 1; k < count; k++)
            {
                optimizer.TrainBatchContinue(Mem, batch, 1);
                if ((DateTime.Now - start).TotalMinutes >= 10)
                {
                    System.IO.File.WriteAllText("autosave_" + k + ".neural", network.SaveJSON());
                    start = DateTime.Now;
                }
            }

            return autoencoder.GetError(val);
        }

        public static KeyValuePair<double,double> Action(Network network, Optimizer optimizer, IDataEnumerator data, IDataEnumerator val, int count, int batch = 32)
        {
            DateTime start;
            start = DateTime.Now;
            int layers = network.layers.Count;
            for(int i = 0;i < layers - 1;i++)
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
                MirrorEnumerator Mem = new MirrorEnumerator(nEm);

                optimizer.network = autoencoder;


                double err = optimizer.TrainBatch(Mem, batch, 1).Last();


                for(int k = 1;k < count;k++)
                {
                    Console.Write("{0} ", k);
                    optimizer.TrainBatchContinue(Mem, batch, 1);
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

        
    }

    class MirrorEnumerator : IDataEnumerator
    {
        IDataEnumerator enumer;
        public MirrorEnumerator(IDataEnumerator enumer)
        {
            this.enumer = enumer;
        }

        public KeyValuePair<double[,,], double[,,]> Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        object IEnumerator.Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<double[,,], double[,,]>> GetEnumerator()
        {
            return this;
        }

        public KeyValuePair<double[,,], double[,,]> GetRandom()
        {
            var pair = enumer.GetRandom();
            return new KeyValuePair<double[,,], double[,,]>(pair.Key, pair.Key);
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public double[,,] Process(Bitmap input)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }

    class NetworkEnumerator : IDataEnumerator
    {
        Network network;
        IDataEnumerator enumer;
        public NetworkEnumerator(Network network, IDataEnumerator enumer)
        {
            this.network = network;
            this.enumer = enumer;
        }

        public KeyValuePair<double[,,], double[,,]> Current
        {
            get
            {
                var pair = enumer.Current;
                var data = network.GetOutput(pair.Key);
                return new KeyValuePair<double[,,], double[,,]>(data, pair.Value);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
            
        }

        public KeyValuePair<double[,,], double[,,]> GetRandom()
        {
            var pair = enumer.GetRandom();
            var data = network.GetOutput(pair.Key);
            return new KeyValuePair<double[,,], double[,,]>(data, pair.Value);
        }

        public bool MoveNext()
        {
            return enumer.MoveNext();
        }

        public void Reset()
        {
            enumer.Reset();
        }

        public IEnumerator<KeyValuePair<double[,,], double[,,]>> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public double[,,] Process(Bitmap input)
        {
            throw new NotImplementedException();
        }
    }
}
