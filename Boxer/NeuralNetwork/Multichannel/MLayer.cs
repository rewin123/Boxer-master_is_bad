using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Multichannel
{
    /// <summary>
    /// Слой со множеством разноразмерных входов и выходов
    /// </summary>
    public abstract class MLayer
    {
        [NonSerialized]
        public Activation func;
        protected Size[] input_sizes;
        protected Size[] output_sizes;
        public MLayer(Activation activation, Size[] output_sizes)
        {
            func = activation;
            this.output_sizes = output_sizes;
        }

        public Size[] Input_Sizes
        {
            get
            {
                return input_sizes;
            }
        }

        public Size[] Output_Sizes
        {
            get
            {
                return output_sizes;
            }
        }

        public virtual void Compile(params Size[] input_sizes)
        {
            this.input_sizes = input_sizes;
        }
        public abstract void GetOutput(double[][,,] input, double[][,,] output);

        public abstract void GetOutputAndDiff(double[][,,] input, double[][,,] output, double[][,,] diff);

        /// <summary>
        /// Расчитывает ошибку на входе слоя
        /// </summary>
        /// <returns></returns>
        public abstract void GetError(double[][,,] error_out, double[][,,] diff, double[][,,] input, double[][,,] result_error);

        public abstract void CreateGradients(int count);

        public abstract void WriteG(int pos, double[][,,] error_out, double[][,,] input, double k);

        public abstract void ActionG(int pos, int to, Func<double, double> func);

        public abstract void ActionTwoG(int pos1, int pos2, int to, Func<double, double, double> func);

        public abstract void GradWeights(int pos);

        /// <summary>
        /// Создает входную матрицу
        /// </summary>
        /// <returns></returns>
        public double[][,,] CreateInput()
        {
            var result = new double[input_sizes.Length][,,];
            for (int i_pos = 0; i_pos < input_sizes.Length; i_pos++)
            {
                result[i_pos] = doubleArrayExtensions.CreateArray(input_sizes[i_pos]);
            }

            return result;
        }

        /// <summary>
        /// Создает выходную матрицу
        /// </summary>
        /// <returns></returns>
        public double[][,,] CreateOutput()
        {
            var result = new double[output_sizes.Length][,,];
            for (int o_pos = 0; o_pos < output_sizes.Length; o_pos++)
            {
                result[o_pos] = doubleArrayExtensions.CreateArray(output_sizes[o_pos]);
            }

            return result;
        }
    }
}
