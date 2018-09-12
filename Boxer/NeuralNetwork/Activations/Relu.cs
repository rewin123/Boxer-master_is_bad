using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Relu : Activation
    {
        public double[,,] Diff(double[,,] val)
        {
            return val.NewEach(x => x >= 0 ? 1 : 0);
        }

        public double[,,] FastFunc(double[,,] val)
        {
            val.ForEach(x => x > 0 ? x : 0);
            return val;
        }

        public double[,,] Func(double[,,] val)
        {
            return val.NewEach(x => x > 0 ? x : 0);
        }

        public void SyncDiff(double[,,] val, double[,,] result)
        {
            result.ForEach(val, (y, x) => x >= 0 ? 1 : 0);
        }
    }
}
