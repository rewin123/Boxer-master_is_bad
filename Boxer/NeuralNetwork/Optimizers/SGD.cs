using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class SGD : Optimizer
    {
        double k;
        public SGD(Network network, double learning_rate) : base(network)
        {
            k = learning_rate;
            network.CreateGradients(2);
            network.Normalization();
        }

        public override void Init(int batch)
        {
            ;
        }

        public override double[] TrainBatch(IDataEnumerator data, int batch, int count)
        {
            double[] errors = new double[count];
            for(int c = 0;c < count;c++)
            {
                int max_found = 0;
                double max = 0;
                double err = 0;
                for (int j = 0; j < batch; j++)
                {
                    max = -1;
                    var pair = data.GetRandom(ref network);
                    int max_arg = -1;
                    pair.Value.ForEach((val, z, y, x) => {
                        if(val > max)
                        {
                            max_arg = x;
                            max = val;
                        }
                    });

                    max = 0;
                    int max_arg2 = -2;
                    
                    var res = network.WriteG(0,pair.Key, pair.Value, k);
                    res.Value.ForEach((val, z, y, x) =>
                    {
                        if (val > max)
                        {
                            max_arg2 = x;
                            max = val;
                        }
                    });
                    
                    err += res.Key;
                    network.ActionTwoG(0, 1, 1, (x, y) => x + y);
                    
                    if(max_arg == max_arg2)
                    {
                        max_found++;
                    }
                }

                network.ActionG(1, 1, val => val / batch);
                network.GradWeights(1);
                network.ActionG(1,1, x => 0);

                errors[c] = err / batch;
                Console.WriteLine(err / batch);
                
            }

            return errors;
        }
    }
}
