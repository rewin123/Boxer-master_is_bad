using NeuralNetwork.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Callbacks
{
    /// <summary>
    /// Вызываемые события между эпохами
    /// </summary>
    public interface ICallback
    {
        /// <summary>
        /// Вызывается действие коллбека
        /// </summary>
        /// <param name="learner"></param>
        void Action(Learner learner);
    }
}
