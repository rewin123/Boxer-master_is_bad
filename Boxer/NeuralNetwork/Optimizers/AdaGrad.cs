using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class AdaGrad : Optimizer
    {
        double d = 1e-7;
        double r = 0;
        public AdaGrad(Network network) : base(network)
        {
            network.CreateGradients(2);
            network.Normalization();
        }

        public override void Init(int batch)
        {
            throw new NotImplementedException();
        }

        public override double[] TrainBatch(IDataEnumerator data, int batch, int count)
        {
            double[] errors = new double[count];
            for (int c = 0; c < count; c++)
            {
                double err = 0;
                for (int j = 0; j < batch; j++)
                {
                    var pair = data.GetRandom(ref network);
                   

                    var res = network.WriteG(0, pair.Key, pair.Value, 1);
                    
                    err += res.Key;
                    network.ActionTwoG(0, 1, 1, (x, y) => x + y);
                    
                }

                network.ActionG(1, 1, val => val / batch);

                double norm = 0;
                int norm_count = 0;

                network.ActionG(1, 1, val => {
                    norm += val * val;
                    norm_count++;
                    return val;
                });

                norm /= norm_count;

                r += norm;

                double k = 1e-1 / (d + Math.Sqrt(r));

                network.ActionG(1, 1, val => k * val);

                network.GradWeights(1);
                network.ActionG(1, 1, x => 0);

                errors[c] = err / batch;
                Console.WriteLine(err / batch);
                
            }

            return errors;
        }
    }
}
