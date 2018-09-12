using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Layers
{
    class MergeLayer : Layer
    {
        public MergeLayer(Activation activation) : base(activation)
        {
        }

        public MergeLayer(LayerData data) : base(data)
        {
        }

        public override bool ITrained => base.ITrained;

        public override Layer Mirror => base.Mirror;

        public override void ActionG(int pos, int to, Func<double, double> func)
        {
            base.ActionG(pos, to, func);
        }

        public override void ActionTwoG(int pos1, int pos2, int to, Func<double, double, double> func)
        {
            base.ActionTwoG(pos1, pos2, to, func);
        }

        public override void AddNoise(double width)
        {
            base.AddNoise(width);
        }

        public override void ChangeWeights(double[,,] error_out, double[,,] input, double k)
        {
            base.ChangeWeights(error_out, input, k);
        }

        public override Layer CloneMe()
        {
            return base.CloneMe();
        }

        public override Size Compile(Size input_size)
        {
            return base.Compile(input_size);
        }

        public override void CreateGradients(int count)
        {
            base.CreateGradients(count);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override double[,,] GetCachedDiff()
        {
            return base.GetCachedDiff();
        }

        public override double[,,] GetCachedOutpur()
        {
            return base.GetCachedOutpur();
        }

        public override void GetError(double[,,] error_out, double[,,] diff, double[,,] input, double[,,] result)
        {
            base.GetError(error_out, diff, input, result);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override double[,,] GetOutput(double[,,] input)
        {
            return base.GetOutput(input);
        }
        

        public override void GradWeights(int pos)
        {
            base.GradWeights(pos);
        }

        public override void NormalizationW()
        {
            base.NormalizationW();
        }

        public override double NormG(int pos)
        {
            return base.NormG(pos);
        }

        public override LayerData SaveJSON()
        {
            return base.SaveJSON();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void WriteG(int pos, double[,,] error_out, double[,,] input, double k)
        {
            base.WriteG(pos, error_out, input, k);
        }
    }
}
