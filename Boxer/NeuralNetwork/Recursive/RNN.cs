using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    /// <summary>
    /// Рекурсивная нейронная сеть
    /// </summary>
    public class RNN : Network
    {
        double[,,] cached_last_layer;
        double[,,] cached_output;
        Layer memory_layer;
        int z_offset;

        public RNN(Layer memory_layer)
        {
            this.memory_layer = memory_layer;
        }

        public override void Compile(Size input_size, bool console = false)
        {
            z_offset = input_size[0];
            base.Compile(input_size, console);
            memory_layer.Compile(layers.Last().output_size);
            layers[0].Compile(new Size(input_size[0] + memory_layer.output_size[0], Math.Max(input_size[1], memory_layer.output_size[1]), Math.Max(input_size[2], memory_layer.output_size[2])));
            cached_output = doubleArrayExtensions.CreateArray(memory_layer.output_size);
            cached_last_layer = doubleArrayExtensions.CreateArray(memory_layer.input_size);
        }

        public override void ClearBuffers()
        {
            cached_last_layer.ForEach((val) => 0);
            cached_output.ForEach((val) => 0);
        }

        public override KeyValuePair<double, double[,,]> WriteG(int pos, double[,,] input, double[,,] t, double k)
        {
            List<IOPair> pairs;
            double err;
            double[][,,] errors = GetBackwardErrors(input, t, out err, out pairs);

            layers[0].WriteG(pos, errors[0], input, k);

            for (int i = 1; i < layers.Count; i++)
            {
                layers[i].WriteG(pos, errors[i], pairs[i - 1].Key, k);
            }

            int depth = memory_layer.output_size[0];
            int height = memory_layer.output_size[1];
            int width = memory_layer.output_size[2];

            #region Расчет ошибки для входа
            double[,,] diff = doubleArrayExtensions.CreateArray(layers[0].input_size);
            diff.ForEach((val) => 1);
            var true_diff = memory_layer.GetOutputAndDiff(cached_last_layer).Value;
            for (int z = 0; z < depth; z++)
            {
                int z2 = z + z_offset;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        diff[z2, y, x] = true_diff[z, y, x];
                    }
                }
            }
            var error = layers[0].CreateInpuut();
            layers[0].GetError(errors[0], diff, input, error);
            #endregion

            #region Запись градиента для рекурсивного слоя
            double[,,] new_err = doubleArrayExtensions.CreateArray(memory_layer.output_size);
            
            for (int z = 0; z < depth; z++)
            {
                int z2 = z + z_offset;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        new_err[z, y, x] = error[z2, y, x];
                    }
                }
            }
            memory_layer.WriteG(pos, new_err, cached_last_layer, k);
            #endregion

            return new KeyValuePair<double, double[,,]>(Math.Sqrt(err / t.Length), pairs.Last().Key);
        }

        public override List<IOPair> GetOutAndDiff(double[,,] input)
        {
            #region Копирование результатов в наибольший массив
            double[,,] newInput = doubleArrayExtensions.CreateArray(layers[0].input_size);
            int depth = input.GetLength(0);
            int height = input.GetLength(1);
            int width = input.GetLength(2);
            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        newInput[z, y, x] = input[z, y, x];
                    }
                }
            }

            int offset = z_offset;
            depth = cached_output.GetLength(0);
            height = cached_output.GetLength(1);
            width = cached_output.GetLength(2);
            for (int z = 0; z < depth; z++)
            {
                int z2 = z + offset;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        newInput[z2, y, x] = cached_output[z, y, x];
                    }
                }
            }
            #endregion
            var result = base.GetOutAndDiff(newInput);
            #region Кеширование
            cached_last_layer = result[result.Count - 2].Key;
            cached_output = memory_layer.GetOutput(cached_last_layer);
            #endregion
            return result;
        }
    }
}
