using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class AdamParallel : Optimizer
    {
        double e;
        double a;
        double ro;
        double k;
        public AdamParallel(Network network, double e, double a, double ro) : base(network)
        {
            this.e = e;
            this.a = a;
            this.ro = ro;
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
                //Parallel.For(0, batch, b =>
                for(int b = 0;b < batch;b++)
                {
                    var pair = data.GetRandom();
                    var res = network.ParallelWriteG(b, pair.Key, pair.Value, 1);

                    loc_errors[b] = res.Key;

                    double max = -1;
                    int max_arg = -1;
                    pair.Value.ForEach((val, z, y, x) =>
                    {
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

                    if (max_arg == max_arg2)
                    {
                        loc_found[b] = 1;
                    }
                }//);

                for (int b = 0; b < batch; b++)
                {
                    network.ActionTwoG(b, batch, batch, (v1, v2) => v1 + v2);
                    err += loc_errors[b];
                    max_found += loc_found[b];
                }

                network.ActionG(batch, batch, val => val / batch);
                double dw = 0;
                int w_count = 0;
                network.ActionG(batch, batch, x =>
                {
                    dw += x * x;
                    w_count++;
                    return x;
                });
                dw /= w_count;
                dw *= 1000;
                k = ro * k + (1 - ro) * e / Math.Sqrt(1 + dw);
                network.ActionTwoG(batch, batch + 1, batch + 1, (g, v) => a * v + g * k);

                network.GradWeights(batch + 1);

                network.ActionG(batch, batch, val => 0);

                errors[c] = err / batch;


                Console.WriteLine();
                Console.WriteLine("{2}: {0} : {1}", err / batch, k, c);
                Console.WriteLine("A: {0} ", (float)(DateTime.Now - start).TotalSeconds);
                Console.WriteLine("Found {0} of {1} {2}%", max_found, batch, 100.0f * max_found / batch);

                //Console.WriteLine("Norm: {0}", network.layers[0].NormG(2));
            }

            return errors;
        }

        public override double[] TrainBatchContinue (IDataEnumerator data, int batch, int count)
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
                    var pair = data.GetRandom();
                    var res = network.ParallelWriteG(b, pair.Key, pair.Value, 1);

                    loc_errors[b] = res.Key;

                    double max = -1;
                    int max_arg = -1;
                    pair.Value.ForEach((val, z, y, x) =>
                    {
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

                    if (max_arg == max_arg2)
                    {
                        loc_found[b] = 1;
                    }
                });

                for (int b = 0; b < batch; b++)
                {
                    network.ActionTwoG(b, batch, batch, (v1, v2) => v1 + v2);
                    err += loc_errors[b];
                    max_found += loc_found[b];
                }

                network.ActionG(batch, batch, val => val / batch);
                double dw = 0;
                int w_count = 0;
                network.ActionG(batch, batch, x =>
                {
                    dw += x * x;
                    w_count++;
                    return x;
                });
                dw /= w_count;
                dw *= 1000;
                k = ro * k + (1 - ro) * e / Math.Sqrt(1 + dw);
                network.ActionTwoG(batch, batch + 1, batch + 1, (g, v) => a * v + g * k);

                network.GradWeights(batch + 1);

                network.ActionG(batch, batch, val => 0);

                errors[c] = err / batch;


                Console.WriteLine();
                Console.WriteLine("{2}: {0} : {1}", err / batch, k, c);
                Console.WriteLine("A: {0} ", (float)(DateTime.Now - start).TotalSeconds);
                Console.WriteLine("Found {0} of {1} {2}%", max_found, batch, 100.0f * max_found / batch);

                //Console.WriteLine("Norm: {0}", network.layers[0].NormG(2));
            }

            return errors;
        }


        public List<KeyValuePair<double, double>> TrainBatchPercent(IDataEnumerator data, int batch, int count)
        {
            List<KeyValuePair<double, double>> pairs = new List<KeyValuePair<double, double>>();
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
                    var pair = data.GetRandom();
                    var res = network.ParallelWriteG(b, pair.Key, pair.Value, 1);

                    loc_errors[b] = res.Key;

                    double max = -1;
                    int max_arg = -1;
                    pair.Value.ForEach((val, z, y, x) =>
                    {
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

                    if (max_arg == max_arg2)
                    {
                        loc_found[b] = 1;
                    }
                });

                for (int b = 0; b < batch; b++)
                {
                    network.ActionTwoG(b, batch, batch, (v1, v2) => v1 + v2);
                    err += loc_errors[b];
                    max_found += loc_found[b];
                }

                network.ActionG(batch, batch, val => val / batch);
                double dw = 0;
                int w_count = 0;
                network.ActionG(batch, batch, x =>
                {
                    dw += x * x;
                    w_count++;
                    return x;
                });
                dw /= w_count;
                dw *= 1000;
                k = ro * k + (1 - ro) * e / Math.Sqrt(1 + dw);
                network.ActionTwoG(batch, batch + 1, batch + 1, (g, v) => a * v + g * k);

                network.GradWeights(batch + 1);

                network.ActionG(batch, batch, val => 0);

                pairs.Add(new KeyValuePair<double, double>(err / batch, 100.0f * max_found / batch));


                Console.WriteLine();
                Console.WriteLine("{2}: {0} : {1}", err / batch, k, c);
                Console.WriteLine("A: {0} ", (float)(DateTime.Now - start).TotalSeconds);
                Console.WriteLine("Found {0} of {1} {2}%", max_found, batch, 100.0f * max_found / batch);

                //Console.WriteLine("Norm: {0}", network.layers[0].NormG(2));
            }

            return pairs;
        }
    }

}
