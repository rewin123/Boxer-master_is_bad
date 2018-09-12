using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Metrics
{
    public interface IMetric
    {
        /// <summary>
        /// Вычисляет метрику между предсказанной нейросетью значениемя и истинными
        /// </summary>
        /// <param name="prediction"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        double Metric(double[,,] prediction, double[,,] output);

        string Name { get; }
    }
}
