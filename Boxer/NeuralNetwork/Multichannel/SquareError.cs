using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork;

namespace NeuralNetwork.Multichannel
{
    public class SquareError : OutputError
    {
        public override double Error(double[][,,] neural, double[][,,] output)
        {
            double val = 0;
            int len = neural.Length;
            for(int i = 0;i < len; i++)
            {
                neural[i].ForEach(output[i], (n, o) =>
                {
                    val += (n - o) * (n - o);
                });
            }

            return val;
        }

        public override double[][,,] Gradient(double[][,,] neural, double[][,,] output)
        {
            int len = neural.Length;
            double[][,,] result = new double[len][,,];
            for(int i = 0;i < len;i++)
            {
                result[i] = neural[i].NewEach(output[i], (n, o) => n - o);
            }

            return result;
        }
    }
}
