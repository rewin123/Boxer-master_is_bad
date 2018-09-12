using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Recursive
{
    /// <summary>
    /// Одинвзаимосвязанный список входов и выходов для рекурсивной нейронной сети
    /// </summary>
    public class RecLine
    {
        public List<double[,,]> inputs = new List<double[,,]>();
        public List<double[,,]> outputs = new List<double[,,]>();
        public List<bool> needOutputs = new List<bool>();

        public void AddSample(double[,,] input, double[,,] output, bool needOutput)
        {
            inputs.Add(input);
            outputs.Add(output);
            needOutputs.Add(needOutput);
        }
    }
}
