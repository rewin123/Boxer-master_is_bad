using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class AdadeltaMomentum : Optimizer
    {
        double a;
        double k;
        double gamma;

        double r = 0;
        double d = 1e-3;

        int acc = 0;
        public AdadeltaMomentum(Network network, double a, double k, double gamma) : base(network)
        {
            this.a = a;
            this.k = k;
            this.gamma = gamma;

            network.CreateGradients(3);
            //network.Normalization();
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
                DateTime start = DateTime.Now;
                double err = 0;
                for (int b = 0; b < batch; b++)
                {
                    var pair = data.GetRandom(ref network);
                    var res = network.WriteG(0, pair.Key, pair.Value, 1);
                    err += res.Key;
                    network.ActionTwoG(0, 1, 1, (v1, v2) => v1 + v2);
                }

                double norm = 0;
                int norm_count = 0;

                network.ActionG(1, 1, val => {
                    norm += val * val;
                    norm_count++;
                    return val;
                });

                norm = norm / norm_count;

                r = r * gamma + (1 - gamma) * norm;

                double k2 = k /(Math.Sqrt(r) + d) / batch;

                network.ActionG(1, 1, val => val * k2);
                network.ActionTwoG(1, 2, 2, (g, v) => a * v + g);

                network.GradWeights(2);
                
                network.ActionG(1, 1, val => 0);

                errors[c] = err / batch;
                
                
                Console.WriteLine("{0} : {1}", err / batch, Math.Sqrt(norm));
                //Console.WriteLine("A: {0}", (DateTime.Now - start).TotalSeconds);

                if (err / batch <= 1e-7)
                {
                    acc++;
                    if (acc > 10)
                    {
                        Console.WriteLine("Stopped at {0}", c);
                        return errors;
                    }
                }
                else acc = 0;

                //Console.WriteLine("Norm: {0}", network.layers[0].NormG(2));
            }

            return errors;
        }
    }
}
