using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class MaxAct : Activation
    {
        public double[,,] Diff(double[,,] val)
        {
            double max = double.MinValue;
            val.ForEach(x => { max = Math.Max(max, Math.Abs(x)); });
            return val.NewEach(x => x == max ? 0 : 1);
        }

        public double[,,] FastFunc(double[,,] val)
        {
            double max = double.MinValue;
            val.ForEach(x => { max = Math.Max(max, x); });
            val.ForEach(x => x / max);
            return val;
        }

        public double[,,] Func(double[,,] val)
        {
            double max = double.MinValue;
            val.ForEach(x => { max = Math.Max(max, Math.Abs(x)); });
            return val.NewEach(x => x / max);
        }

        public void SyncDiff(double[,,] val, double[,,] result)
        {
            double max = double.MinValue;
            val.ForEach(x => { max = Math.Max(max, Math.Abs(x)); });
            result.ForEach(val, (y, x) => x == max ? 0 : 1);
        }
    }
}
