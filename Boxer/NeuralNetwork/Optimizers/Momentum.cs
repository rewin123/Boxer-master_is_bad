using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Momentum : Optimizer
    {
        double a;
        double k;

        int acc = 0;
        public Momentum(Network network, double a, double k) : base(network)
        {
            this.a = a;
            this.k = k;

            network.CreateGradients(3);
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
                int max_found = 0;
                double max = 0;

                var g_write = new TimeSpan();
                var a_time = new TimeSpan();
                DateTime start;
                double err = 0;
                for (int b = 0; b < batch; b++)
                {
                    var pair = data.GetRandom(ref network);
                    start = DateTime.Now;
                    var res = network.ParallelWriteG(0, pair.Key, pair.Value, k);
                    g_write += (DateTime.Now - start);
                    err += res.Key;
                    start = DateTime.Now;
                    network.ActionTwoG(0, 1, 1, (v1, v2) => v1 + v2);
                    a_time += (DateTime.Now - start);


                    max = -1;
                    int max_arg = -1;
                    pair.Value.ForEach((val, z, y, x) => {
                        if (val > max)
                        {
                            max_arg = x;
                            max = val;
                        }
                    });

                    max = -1;
                    int max_arg2 = -2;
                    
                    res.Value.ForEach((val, z, y, x) =>
                    {
                        if (val > max)
                        {
                            max_arg2 = x;
                            max = val;
                        }
                    });

                    if(max_arg == max_arg2)
                    {
                        max_found++;
                    }
                }
                
                start = DateTime.Now;
                network.ActionG(1, 1, val => val / batch);
                network.ActionTwoG(1, 2, 2, (g, v) => a*v + g*(1-a));
                a_time += (DateTime.Now - start);

                network.GradWeights(2);

                start = DateTime.Now;
                network.ActionG(1, 1, val => 0);
                a_time += (DateTime.Now - start);

                errors[c] = err / batch;

                double dw = 0;
                int w_count = 0;
                network.ActionG(2, 2, x =>
                {
                    dw += x * x;
                    w_count++;
                    return x;
                });
                Console.WriteLine();
                Console.WriteLine("{2}: {0} : {1}", err / batch, Math.Sqrt(dw / w_count), c);
                Console.WriteLine("A: {0} G:{1}", a_time.TotalSeconds, g_write.TotalSeconds);
                Console.WriteLine("Found {0} of {1} {2}%", max_found, batch, 100.0f * max_found / batch);
                
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
