using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Activations
{
    public class LeackyRelu : Activation
    {
        double alpha = 0.2;
        internal LeackyRelu()
        {

        }

        public LeackyRelu(double alpha)
        {
            this.alpha = alpha;
        }
        public double[,,] Diff(double[,,] val)
        {
            return val.NewEach(x => x >= 0 ? 1 : alpha);
        }

        public double[,,] FastFunc(double[,,] val)
        {
            val.ForEach(x => x > 0 ? x : alpha*x);
            return val;
        }

        public double[,,] Func(double[,,] val)
        {
            return val.NewEach(x => x > 0 ? x : alpha*x);
        }

        public void SyncDiff(double[,,] val, double[,,] result)
        {
            result.ForEach(val, (y, x) => x >= 0 ? 1 : alpha);
        }
    }
}
