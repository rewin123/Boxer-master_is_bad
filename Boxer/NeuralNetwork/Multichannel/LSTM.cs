using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Multichannel
{
    class LSTM : MLayer
    {
        public LSTM(Activation activation, Size[] input_sizes) : base(activation, input_sizes)
        {

        }
        public override void ActionG(int pos, int to, Func<double, double> func)
        {
            throw new NotImplementedException();
        }

        public override void ActionTwoG(int pos1, int pos2, int to, Func<double, double, double> func)
        {
            throw new NotImplementedException();
        }

        public override void CreateGradients(int count)
        {
            throw new NotImplementedException();
        }

        public override void GetError(double[][,,] error_out, double[][,,] diff, double[][,,] input, double[][,,] result_error)
        {
            throw new NotImplementedException();
        }

        public override void GetOutput(double[][,,] input, double[][,,] output)
        {
            throw new NotImplementedException();
        }

        public override void GetOutputAndDiff(double[][,,] input, double[][,,] output, double[][,,] diff)
        {
            throw new NotImplementedException();
        }

        public override void GradWeights(int pos)
        {
            throw new NotImplementedException();
        }

        public override void WriteG(int pos, double[][,,] error_out, double[][,,] input, double k)
        {
            throw new NotImplementedException();
        }
    }
}
