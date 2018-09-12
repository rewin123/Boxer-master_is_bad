using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Losses
{
    public class MeanSquareLoss : ILoss
    {
        public string Name => "meansquare";

        public void Error(double[,,] prediction, double[,,] output, double[,,] result)
        {
            int depth2 = output.GetLength(0);
            int height2 = output.GetLength(1);
            int width2 = output.GetLength(2);

            double err = 0;
            double temp;
            for (int z2 = 0; z2 < depth2; z2++)
            {
                for (int y2 = 0; y2 < height2; y2++)
                {
                    for (int x2 = 0; x2 < width2; x2++)
                    {
                        result[z2,y2,x2] =  output[z2,y2,x2] - prediction[z2, y2, x2];
                    }
                }
            }
        }

        public double Metric(double[,,] prediction, double[,,] output)
        {
            int depth2 = output.GetLength(0);
            int height2 = output.GetLength(1);
            int width2 = output.GetLength(2);

            double err = 0;
            double temp;
            for (int z2 = 0; z2 < depth2; z2++)
            {
                for (int y2 = 0; y2 < height2; y2++)
                {
                    for (int x2 = 0; x2 < width2; x2++)
                    {
                        temp = output[z2, y2, x2] - prediction[z2, y2, x2];
                        err += temp * temp;
                    }
                }
            }

            err /= depth2 * height2 * width2;
            return err;
        }
    }
}
