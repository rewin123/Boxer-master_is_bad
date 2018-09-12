using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Multichannel
{
    /// <summary>
    /// Многоканальный полносвязный слой
    /// </summary>
    public class MFCLayer : MLayer
    {
        //[in, out] indexes
        FullyConnLayar[,] layers;

        public MFCLayer(Activation activation, Size[] output_sizes) : base(activation, output_sizes)
        {

        }



        public override void GetOutput(double[][,,] input, double[][,,] result)
        {
            for (int o_pos = 0; o_pos < output_sizes.Length; o_pos++)
            {
                var array = result[o_pos];
                int dz = array.GetLength(0);
                int dh = array.GetLength(1);
                int dw = array.GetLength(2);
                for (int i_pos = 1; i_pos < input_sizes.Length; i_pos++)
                {
                    var loc_arr = layers[i_pos, o_pos].GetOutput(input[i_pos]);
                    for (int z = 0; z < dz; z++)
                        for (int y = 0; y < dh; y++)
                            for (int x = 0; x < dw; x++)
                                array[z, y, x] += loc_arr[z, y, x];
                }


                result[o_pos] = func.FastFunc(array);
            }
        }

        public override void GetOutputAndDiff(double[][,,] input, double[][,,] output, double[][,,] diff)
        {
            for (int o_pos = 0; o_pos < output_sizes.Length; o_pos++)
            {
                var array = output[o_pos];
                int dz = array.GetLength(0);
                int dh = array.GetLength(1);
                int dw = array.GetLength(2);
                for (int i_pos = 1; i_pos < input_sizes.Length; i_pos++)
                {
                    var loc_arr = layers[i_pos, o_pos].GetOutput(input[i_pos]);

                    for (int z = 0; z < dz; z++)
                        for (int y = 0; y < dh; y++)
                            for (int x = 0; x < dw; x++)
                                array[z, y, x] += loc_arr[z, y, x];
                }


                diff[o_pos] = func.Diff(array);
                output[o_pos] = func.FastFunc(array);
            }
        }

        public override void Compile(Size[] input_sizes)
        {
            base.Compile(input_sizes);

            layers = new FullyConnLayar[input_sizes.Length, output_sizes.Length];
            for (int i = 0; i < input_sizes.Length; i++)
            {
                for (int j = 0; j < output_sizes.Length; j++)
                {
                    layers[i, j] = new FullyConnLayar(new SimpleFunc(), output_sizes[j]);
                    layers[i, j].Compile(input_sizes[i]);
                    layers[i, j].AddNoise();
                }
            }
        }

        public override void GetError(double[][,,] error_out, double[][,,] diff, double[][,,] input, double[][,,] result)
        {
            for (int i = 0; i < input_sizes.Length; i++)
            {
                int depth1 = input_sizes[i][0];
                int height1 = input_sizes[i][1];
                int width1 = input_sizes[i][2];

                var loc_err = layers[i, 0].CreateInpuut();
                layers[i, 0].GetError(error_out[0], diff[i], input[i], result[i]);

                for (int j = 1; j < output_sizes.Length; j++)
                {
                    layers[i, j].GetError(error_out[j], diff[i], input[i], loc_err);
                    result[i].Sum(loc_err);
                }
            }
        }

        public override void WriteG(int pos, double[][,,] error_out, double[][,,] input, double k)
        {
            for(int i = 0; i < input_sizes.Length;i++)
            {
                for(int j = 0;j < output_sizes.Length;j++)
                {
                    layers[i, j].WriteG(pos, error_out[j], input[i], k);
                }
            }
        }

        public override void ActionG(int pos, int to, Func<double, double> func)
        {
            ActionLayers((l, i, j) => l.ActionG(pos, to, func));
        }

        public override void ActionTwoG(int pos1, int pos2, int to, Func<double, double, double> func)
        {
            ActionLayers((l, i, j) => l.ActionTwoG(pos1, pos2, to, func));
        }

        public override void GradWeights(int pos)
        {
            ActionLayers((l, i, j) => l.GradWeights(pos));
        }

        public override void CreateGradients(int count)
        {
            for (int i = 0; i < input_sizes.Length; i++)
                for (int j = 0; j < output_sizes.Length; j++)
                    layers[i, j].CreateGradients(count);
        }

        /// <summary>
        /// Совершает action над каждым слоем в мультиканальном полносвязном слое
        /// </summary>
        /// <param name="action">Вход action = (layer, input_index, output_index)</param>
        void ActionLayers(Action<FullyConnLayar, int, int> action)
        {
            int in_count = input_sizes.Length;
            int out_count = output_sizes.Length;
            for (int i = 0; i < in_count; i++)
                for (int j = 0; j < out_count; j++)
                    action(layers[i, j], i, j);
        }

        T[,] ActionLayers<T>(Func<FullyConnLayar, int, int, T> action)
        {
            int in_count = input_sizes.Length;
            int out_count = output_sizes.Length;

            T[,] arr = new T[in_count, out_count];
            for (int i = 0; i < in_count; i++)
                for (int j = 0; j < out_count; j++)
                   arr[i,j] = action(layers[i, j], i, j);

            return arr;
        }

        void ActionLayers(Action<FullyConnLayar> action)
        {
            int in_count = input_sizes.Length;
            int out_count = output_sizes.Length;
            for (int i = 0; i < in_count; i++)
                for (int j = 0; j < out_count; j++)
                    action(layers[i, j]);
        }
    }
}
