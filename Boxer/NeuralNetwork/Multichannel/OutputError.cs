using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork;

namespace NeuralNetwork.Multichannel
{
    public abstract class OutputError
    {
        public abstract double Error(double[][,,] neural, double[][,,] output);
        public abstract double[][,,] Gradient(double[][,,] neural, double[][,,] output);

        public double[][,,] BatchGradient(List<double[][,,]> neural, List<double[][,,]> output)
        {
            int len = neural.Count;
            var grad = Gradient(neural[0], output[0]);
            for(int i = 0;i < len;i++)
            {
                var loc_grad = Gradient(neural[i], output[i]);
                for (int j = 0; j < grad.Length; j++)
                    grad[j].Sum(loc_grad[j]);
            }

            for (int j = 0; j < grad.Length; j++)
                grad[j].ForEach((v) => v / len);

            return grad;
        }
    }
}
