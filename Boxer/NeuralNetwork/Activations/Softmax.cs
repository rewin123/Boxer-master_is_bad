using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Softmax : Activation
    {
        double c = double.Epsilon * 2;
        public double[,,] Diff(double[,,] val)
        {
            double sum = 0;
            val.ForEach(x => { sum += x * x; });
            sum += c;
            double[,,] result = (double[,,])val.Clone();
            result.ForEach(x => x*(1-x / sum) / sum );
            return result;
        }

        public double[,,] FastFunc(double[,,] val)
        {
            double sum = 0;
            val.ForEach(x => { sum += x * x; });
            val.ForEach(x => x*x / sum);
            return val;
        }

        public double[,,] Func(double[,,] val)
        {
            double sum = 0;
            val.ForEach(x => { sum += x*x; });
            sum += c;
            double[,,] result = (double[,,])val.Clone();
            result.ForEach(x => x*x / sum);
            return result;
        }

        public void SyncDiff(double[,,] val, double[,,] result)
        {
            double sum = 0;
            val.ForEach(x => { sum += x * x; });
            sum += c;
            result.ForEach(x => x * (1 - x / sum) / sum);
        }
    }
}
