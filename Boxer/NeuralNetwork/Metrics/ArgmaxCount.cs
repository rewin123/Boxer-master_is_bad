using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Metrics
{
    public class ArgmaxCount : IMetric
    {
        int c, y, x;
        public ArgmaxCount(int c, int y, int x)
        {
            this.c = c;
            this.x = x;
            this.y = y;
        }
        public string Name => "argmax_count";

        public double Metric(double[,,] prediction, double[,,] output)
        {
            prediction.ArgMax(out int cpred, out int ypred, out int xpred);
            return cpred == c && ypred == y && xpred == x ? 1 : 0;
        }
    }

    public class ArgmaxTrainCount : IMetric
    {
        int c, y, x;
        public ArgmaxTrainCount(int c, int y, int x)
        {
            this.c = c;
            this.x = x;
            this.y = y;
        }
        public string Name => "argmax_train_count";

        public double Metric(double[,,] prediction, double[,,] output)
        {
            output.ArgMax(out int cpred, out int ypred, out int xpred);
            return cpred == c && ypred == y && xpred == x ? 1 : 0;
        }
    }
}
