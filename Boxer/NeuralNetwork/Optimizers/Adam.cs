using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork;

namespace NeuralNetwork.Optimizers
{
    class Adam : Optimizer
    {
        Network network;
        public Adam(Network network) : base(network)
        {
            this.network = network;
        }

        public override void Init(int batch)
        {
            throw new NotImplementedException();
        }

        public override double[] TrainBatch(IDataEnumerator data, int batch, int count)
        {
            return base.TrainBatch(data, batch, count);
        }

        public override double[] TrainBatchContinue(IDataEnumerator data, int batch, int count)
        {
            return base.TrainBatchContinue(data, batch, count);
        }
    }
}
