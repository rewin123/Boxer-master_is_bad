using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Losses
{
    public class BinaryCrossentropy : ILoss
    {
        public string Name => throw new NotImplementedException();

        public void Error(double[,,] prediction, double[,,] output, double[,,] result)
        {
            int channels = output.GetLength(0);
            int heights = output.GetLength(1);
            int widths = output.GetLength(2);

            for(int c =0;c < channels;c++)
            {
                for(int y = 0;y < heights;y++)
                {
                    for(int x = 0;x < widths;x++)
                    {
                        result[c, y, x] = output[c, y, x] == 0 ? -1.0 / (1 - prediction[c, y, x] + 1e-30) : 1.0 / (prediction[c, y, x] + 1e-30);
                    }
                }
            }
        }

        public double Metric(double[,,] prediction, double[,,] output)
        {
            int channels = output.GetLength(0);
            int heights = output.GetLength(1);
            int widths = output.GetLength(2);
            double metric = 0;
            for (int c = 0; c < channels; c++)
            {
                for (int y = 0; y < heights; y++)
                {
                    for (int x = 0; x < widths; x++)
                    {
                        metric += output[c, y, x] == 0 ? -Math.Log(1 - prediction[c,y,x] + 1e-30) : -Math.Log(prediction[c, y, x] + 1e-30);
                    }
                }
            }
            return metric;
        }
    }
}
