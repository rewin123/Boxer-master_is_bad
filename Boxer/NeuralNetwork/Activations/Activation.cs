using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public interface Activation
    {
        /// <summary>
        /// Возращает, не зависящую от val, матрицу, на которую применилась функция Активации
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        double[,,] Func(double[,,] val);
        double[,,] Diff(double[,,] val);
        void SyncDiff(double[,,] val, double[,,] result);

        /// <summary>
        /// Напрямую изменят val
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        double[,,] FastFunc(double[,,] val);
    }
}
