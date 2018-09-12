using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork;

namespace NeuralNetwork.Losses
{
    /// <summary>
    /// Лосс который в градиент на вызоде ставит везде минус единицы, кроме правильного класса
    /// </summary>
    public class ClassLoss : ILoss
    {
        public string Name => "cls";

        public void Error(double[,,] prediction, double[,,] output, double[,,] result)
        {
            output.ArgMax(out int cout, out int yout, out int xout);
            result.ForEach((v) => -1);
            result[cout, yout, xout] = 1;
        }

        public double Metric(double[,,] prediction, double[,,] output)
        {
            output.ArgMax(out int cout, out int yout, out int xout);

            int depth = output.GetLength(0);
            int height = output.GetLength(1);
            int width = output.GetLength(2);

            double metric = 0;
            foreach(double v in prediction)
            {
                metric += v;
            }

            metric -= 2 * prediction[cout, yout, xout];
            return metric;
        }
    }
}
