using NeuralNetwork.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Losses
{
    public interface ILoss : IMetric
    {
        /// <summary>
        /// Возращает ошибку для обратного распостранения ошибки от предсказанных значений и истинного результата
        /// </summary>
        /// <param name="prediction"></param>
        /// <param name="output"></param>
        /// <param name="result"></param>
        void Error(double[,,] prediction, double[,,] output, double[,,] result);

    }


}
