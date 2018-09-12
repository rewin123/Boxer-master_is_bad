using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Network
    {
        public List<Layer> layers = new List<Layer>();
        List<double[,,]> errors = new List<double[,,]>();

        public Network()
        {

        }

        public Network(Network network)
        {

        }

        public void AddLayer(Layer layer)
        {
            layers.Add(layer);
        }

        public void Compile(Size input_size, bool console = false)
        {
            for(int i = 0;i < layers.Count;i++)
            {
                input_size = layers[i].Compile(input_size);
                errors.Add(doubleArrayExtensions.CreateArray(input_size));

                if(console)
                {
                    Console.WriteLine("{0} {1} {2}", input_size[0], input_size[1], input_size[2]);
                }
            }
        }

        public void CompileOnlyError()
        {
            for (int i = 0; i < layers.Count; i++)
            {
                errors.Add(doubleArrayExtensions.CreateArray(layers[i].output_size));
            }
        }

        public double[,,] GetOutput(double[,,] input)
        {
            double[,,] output = (double[,,])input.Clone();
            for(int i = 0;i < layers.Count;i++)
            {
                output = layers[i].GetOutput(ref output);
            }
            return output;
        }

        public List<KeyValuePair<double[,,],double[,,]>> GetOutAndDiff(double[,,] input)
        {
            List<KeyValuePair<double[,,], double[,,]>> pairs = new List<KeyValuePair<double[,,], double[,,]>>();
            double[,,] output = (double[,,])input.Clone();
            for (int i = 0; i < layers.Count; i++)
            {
                
                KeyValuePair<double[,,],double[,,]> pair = layers[i].GetOutputAndDiff(output);
                output = pair.Key;
                pairs.Add(pair);
            }
            return pairs;
        }

        public double GetError(double[,,] input, double[,,] t)
        {
            double[,,] output = GetOutput(input);

            int depth2 = output.GetLength(0);
            int height2 = output.GetLength(1);
            int width2 = output.GetLength(2);

            double val = 0;
            double temp = 0;
            for (int z2 = 0; z2 < depth2; z2++)
            {
                for (int y2 = 0; y2 < height2; y2++)
                {
                    for (int x2 = 0; x2 < width2; x2++)
                    {
                        temp = output[z2, y2, x2] - t[z2, y2, x2];
                        val += temp * temp;
                    }
                }
            }

            return val;
        }

        public KeyValuePair<double,double> GetErrorPair(double[,,] input, double[,,] t)
        {
            double[,,] output = GetOutput(input);

            int depth2 = output.GetLength(0);
            int height2 = output.GetLength(1);
            int width2 = output.GetLength(2);

            double err = 0;
            double temp = 0;
            for (int z2 = 0; z2 < depth2; z2++)
            {
                for (int y2 = 0; y2 < height2; y2++)
                {
                    for (int x2 = 0; x2 < width2; x2++)
                    {
                        temp = output[z2, y2, x2] - t[z2, y2, x2];
                        err += temp * temp;
                    }
                }
            }

            double max = -1;
            int max_arg = -1;
            t.ForEach((val, z, y, x) =>
            {
                if (val > max)
                {
                    max_arg = x;
                    max = val;
                }
            });

            max = -1;
            int max_arg2 = -2;

            output.ForEach((val, z, y, x) =>
            {
                if (val > max)
                {
                    max_arg2 = x;
                    max = val;
                }
            });

            return new KeyValuePair<double, double>(err, max_arg == max_arg2 ? 1 : 0);
        }

        public KeyValuePair<double,double> GetError(IDataEnumerator data)
        {
            data.Reset();
            double err = 0;
            double wins = 0;
            int count = 0;

            object lock_target = new object();
            Parallel.ForEach(data, (needed) =>
            {
                //var needed = data.Current;
                var pair = GetErrorPair(needed.Key, needed.Value);
                lock (lock_target)
                {
                    err += pair.Key;
                    wins += pair.Value;
                    count++;
                }
            });
            data.Reset();
            return new KeyValuePair<double, double>(err / count, wins / count);
        }

        public KeyValuePair<double,double[,,]> Learn(double[,,] input, double[,,] t,double k)
        {
            var val = SetBackwardErrors(input, t);

            layers[0].ChangeWeights(errors[0], input, k);

            for (int i = 1;i < layers.Count;i++)
            {
                layers[i].ChangeWeights(errors[i], layers[i - 1].GetCachedOutpur(), k);
            }

            return val;
        }

        KeyValuePair<double,double[,,]> SetBackwardErrors(double[,,] input, double[,,] t)
        {
            double[,,] output = GetOutput(input);

            //Console.WriteLine("{0} {1}", (float)output[0, 0, 0], (float)output[0, 0, 1]);

            int depth2 = output.GetLength(0);
            int height2 = output.GetLength(1);
            int width2 = output.GetLength(2);

            double val = 0;
            double temp = 0;

            double[,,] out_err = errors.Last();

            double[,,] diff = layers.Last().GetCachedDiff();

            for (int z2 = 0; z2 < depth2; z2++)
            {
                for (int y2 = 0; y2 < height2; y2++)
                {
                    for (int x2 = 0; x2 < width2; x2++)
                    {
                        temp = t[z2, y2, x2] - output[z2, y2, x2];
                        out_err[z2, y2, x2] = -temp * diff[z2, y2, x2];
                        val += temp * temp;
                        
                    }
                }
            }

            errors[errors.Count - 1] = out_err;

            for (int i = layers.Count - 1; i >= 0; i--)
            {
                Layer prev = null;
                if (i != 0)
                {
                    prev = layers[i - 1];
                    if(prev != null)
                        errors[i - 1] = layers[i].GetError(errors[i], prev.GetCachedDiff(), prev.GetCachedOutpur());
                    else
                    {
                        double[,,] diff2 = (double[,,])input.Clone();
                        diff2.ForEach(x => 1);
                        errors[i - 1] = layers[i].GetError(errors[i], diff2, input);
                    }
                }
            }

            return new KeyValuePair<double, double[,,]>(val,output);
        }

        double[][,,] GetBackwardErrors(double[,,] input, double[,,] t, out double err, out List<KeyValuePair<double[,,],double[,,]>> getOutput)
        {
            double[][,,] errors = new double[layers.Count][,,];
            var out_pairs = GetOutAndDiff(input);

            double[,,] output = out_pairs.Last().Key;

            //Console.WriteLine("{0} {1}", (float)output[0, 0, 0], (float)output[0, 0, 1]);

            int depth2 = output.GetLength(0);
            int height2 = output.GetLength(1);
            int width2 = output.GetLength(2);

            double val = 0;
            double temp = 0;

            double[,,] out_err = (double[,,])t.Clone();

            double[,,] diff = out_pairs.Last().Value;

            for (int z2 = 0; z2 < depth2; z2++)
            {
                for (int y2 = 0; y2 < height2; y2++)
                {
                    for (int x2 = 0; x2 < width2; x2++)
                    {
                        temp = t[z2, y2, x2] - output[z2, y2, x2];
                        out_err[z2, y2, x2] = -temp * diff[z2, y2, x2];
                        val += temp * temp;

                    }
                }
            }

            errors[errors.Length - 1] = out_err;

            for (int i = layers.Count - 1; i >= 0; i--)
            {
                Layer prev = null;
                if (i != 0)
                {
                    prev = layers[i - 1];
                    if (prev != null)
                    {
                        errors[i - 1] = layers[i].GetError(errors[i], out_pairs[i - 1].Value, out_pairs[i - 1].Key);
                    }
                    else
                    {
                        double[,,] diff2 = (double[,,])input.Clone();
                        diff2.ForEach(x => 1);
                        errors[i - 1] = layers[i].GetError(errors[i], diff2, input);
                    }
                }
            }

            err = val;
            getOutput = out_pairs;

            return errors;
        }

        public void CreateGradients(int count)
        {
            for (int i = 0; i < layers.Count; i++)
                layers[i].CreateGradients(count);
        }

        public KeyValuePair<double,double[,,]> WriteG(int pos, double[,,] input, double[,,] t, double k)
        {
            var val = SetBackwardErrors(input, t);

            layers[0].WriteG(pos,errors[0], input, k);

            Parallel.For(1, layers.Count, i => 
            {
                layers[i].WriteG(pos,errors[i], layers[i - 1].GetCachedOutpur(), k);
            });

            return val;
        }

        public KeyValuePair<double,double[,,]> ParallelWriteG(int pos, double[,,] input, double[,,] t, double k)
        {
            List<KeyValuePair<double[,,], double[,,]>> pairs;
            double err;
            double[][,,] errors = GetBackwardErrors(input, t, out err, out pairs);

            layers[0].WriteG(pos, errors[0], input, k);

            for (int i = 1; i < layers.Count; i++)
            {
                layers[i].WriteG(pos, errors[i], pairs[i - 1].Key, k);
            }

            return new KeyValuePair<double, double[,,]>(Math.Sqrt(err/t.Length),pairs.Last().Key);
        }

        public void NormalizationGradient(int pos)
        {
            double max_last = 0;
            layers.Last().ActionG(pos, pos, (x) =>
            {
                max_last = Math.Max(max_last, Math.Abs(x));
                return x;
            });

            if(max_last != 0)
            {
                for(int i = 0;i < layers.Count - 1;i++)
                {
                    double current_max = 0;
                    layers[i].ActionG(pos, pos, (x) =>
                    {
                        current_max = Math.Max(current_max, Math.Abs(x));
                        return x;
                    });

                    layers[i].ActionG(pos, pos, (x) => x * max_last / current_max);
                }
            }
        }

        public void ActionG(int pos, int to, Func<double, double> func)
        {
            for (int i = 0; i < layers.Count; i++)
                layers[i].ActionG(pos, to, func);
        }

        public void ActionTwoG(int pos1, int pos2, int to, Func<double, double, double> func)
        {
            for (int i = 0; i < layers.Count; i++)
                layers[i].ActionTwoG(pos1, pos2, to, func);
        }

        public void GradWeights(int pos)
        {
            for (int i = 0; i < layers.Count; i++)
                layers[i].GradWeights(pos);
        }

        public void AddNoise(float width = 0.1f)
        {
            for(int i = 0;i < layers.Count;i++)
            {
                layers[i].AddNoise(width);
            }

        }

        public void Normalization()
        {
            for (int i = 0; i < layers.Count; i++)
                layers[i].NormalizationW();
        }

        public string SaveJSON()
        {
            LayerData[] datas = new LayerData[layers.Count];
            for (int i = 0; i < datas.Length; i++)
                datas[i] = layers[i].SaveJSON();

            LDArray ld = new LDArray
            {
                datas = datas
            };

            return JsonConvert.SerializeObject(ld);
        }

        public void LoadJSON(string data)
        {
            layers.Clear();

            var ld = JsonConvert.DeserializeObject<LDArray>(data);
            var datas = ld.datas;

            for(int i = 0;i < datas.Length;i++)
            {
                layers.Add(Layer.CreateFromJSON(datas[i]));
            }
        }

        internal class LDArray
        {
            public LayerData[] datas;
        }
    }
}
