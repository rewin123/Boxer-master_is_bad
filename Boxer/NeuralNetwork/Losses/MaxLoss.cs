using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Losses
{
    public class MaxLoss : ILoss
    {
        public string Name => "maxloss";

        public void Error(double[,,] prediction, double[,,] output, double[,,] result)
        {
            prediction.ArgMax(out int cpred, out int ypred, out int xpred);
            output.ArgMax(out int cout, out int yout, out int xout);

            result[cpred, ypred, xpred] = -prediction[cpred,ypred,xpred];
            result[cout, yout, xout] = 1 - prediction[cout,yout,xout];
        }

        public double Metric(double[,,] prediction, double[,,] output)
        {
            prediction.ArgMax(out int cpred, out int ypred, out int xpred);
            output.ArgMax(out int cout, out int yout, out int xout);

            

            return cpred == cout && ypred == yout && xpred == xout ? 0 : 1;
        }
    }
}
