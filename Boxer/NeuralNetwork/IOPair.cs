using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class IOPair
    {
        public double[,,] input;
        public double[,,] output;

        public IOPair() { }
        public IOPair(double[,,] input, double[,,] output)
        {
            this.input = input;
            this.output = output;
        }

        public double[,,] Key { get => input; }
        public double[,,] Value { get => output; }
    }
}
