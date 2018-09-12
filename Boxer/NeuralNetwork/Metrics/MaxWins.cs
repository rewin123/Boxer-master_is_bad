using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Metrics
{
    public class ArgMaxMetrics : IMetric
    {
        public string Name => "argmax";

        public double Metric(double[,,] prediction, double[,,] output)
        {
            prediction.ArgMax(out int cpred, out int ypred, out int xpred);
            output.ArgMax(out int cout, out int yout, out int xout);

            return cpred == cout && ypred == yout && xpred == xout ? 0 : 1;
        }
    }
}
