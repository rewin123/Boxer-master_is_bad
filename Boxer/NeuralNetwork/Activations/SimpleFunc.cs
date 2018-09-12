using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class SimpleFunc : Activation
    {
        public double[,,] Diff(double[,,] val)
        {
            return val.NewEach((x) => 1);
        }

        public double[,,] Func(double[,,] val)
        {
            return (double[,,])val.Clone();
        }

        public double[,,] FastFunc(double[,,] val)
        {
            return val;
        }

        public void SyncDiff(double[,,] val, double[,,] result)
        {
            result.ForEach((x) => 1);
        }
    }

}
