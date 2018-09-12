using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class SGDParallel : Optimizer
    {
        double k;
        public SGDParallel(Network network, double learning_rate) : base(network)
        {
            k = learning_rate;
            network.CreateGradients(2);
            network.Normalization();

        }

        public override void Init(int batch)
        {
            network.CreateGradients(batch + 2);
        }

        public override double[] TrainBatch(IDataEnumerator data, int batch, int count)
        {
            double[] errors = new double[count];
            network.CreateGradients(batch + 2);
            for (int c = 0; c < count; c++)
            {
                int max_found = 0;

                double[] loc_errors = new double[batch];
                int[] loc_found = new int[batch];

                DateTime start = DateTime.Now;
                double err = 0;
                Parallel.For(0, batch, b =>
                {
                    var pair = data.GetRandom(ref network);
                    var res = network.ParallelWriteG(b, pair.Key, pair.Value, k);
                    loc_errors[b] = res.Key;
                });

                for (int b = 0; b < batch; b++)
                {
                    network.ActionTwoG(b, batch, batch, (v1, v2) => v1 + v2);
                    err += loc_errors[b];
                    max_found += loc_found[b];
                }

                network.ActionG(batch, batch, val => val / batch);
                network.GradWeights(batch);
                network.ActionG(batch, batch, val => 0);
                errors[c] = err / batch;
                
            }

            return errors;
        }

        public override double[] TrainBatchContinue(IDataEnumerator data, int batch, int count)
        {
            double[] errors = new double[count];
            for (int c = 0; c < count; c++)
            {
                int max_found = 0;

                double[] loc_errors = new double[batch];
                int[] loc_found = new int[batch];

                DateTime start = DateTime.Now;
                double err = 0;
                Parallel.For(0, batch, b =>
                {
                    var pair = data.GetRandom(ref network);
                    var res = network.ParallelWriteG(b, pair.Key, pair.Value, k);
                    loc_errors[b] = res.Key;
                });

                for (int b = 0; b < batch; b++)
                {
                    network.ActionTwoG(b, batch, batch, (v1, v2) => v1 + v2);
                    err += loc_errors[b];
                    max_found += loc_found[b];
                }

                network.ActionG(batch, batch, val => val / batch);
                network.GradWeights(batch);
                network.ActionG(batch, batch, val => 0);
                errors[c] = err / batch;

            }

            return errors;
        }
    }
}
