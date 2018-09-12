using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Sigmoid : Activation
    {
        public double[,,] Diff(double[,,] val)
        {
            //double[,,] output = val.NewEach(x => (double)(1.0 / (1.0 + Math.Exp(-x))));
            //output.ForEach(x => x * (1 - x) + 0.01);
            //return output;
            return val.NewEach(x => 1);
        }

        public double[,,] FastFunc(double[,,] val)
        {
            val.ForEach(x => (double)(1.0 / (1.0 + Math.Exp(-x))));
            return val;
        }

        public double[,,] Func(double[,,] val)
        {
            double[,,] output = val.NewEach(x => (double)(1.0 / (1.0 + Math.Exp(-x))));
            return output;
        }

        public void SyncDiff(double[,,] val, double[,,] result)
        {
            result.ForEach((x) => 1);
        }
    }
}
