using NeuralNetwork.Losses;
using NeuralNetwork.Metrics;
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

        double[][][,,] cached_outputs;
        double[][][,,] cached_diffs;
        double[][][,,] cached_errors;

        public ILoss loss = new MeanSquareLoss();
        public Network()
        {

        }

        public Network(Network network)
        {

        }

        public virtual void AddLayer(Layer layer)
        {
            layers.Add(layer);
        }

        public virtual void Compile(Size input_size, bool console = false)
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

        public virtual void CompileOnlyError()
        {
            for (int i = 0; i < layers.Count; i++)
            {
                errors.Add(doubleArrayExtensions.CreateArray(layers[i].output_size));
            }
        }

        /// <summary>
        /// Получить вызод сети
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual double[,,] GetOutput(double[,,] input)
        {
            double[,,] output = (double[,,])input.Clone();
            for(int i = 0;i < layers.Count;i++)
            {
                output = layers[i].GetOutput(output);
            }
            return output;
        }

        public virtual List<IOPair> GetOutAndDiff(double[,,] input)
        {
            var list = new List<IOPair>();
            var output = input;
            for (int i = 0; i < layers.Count; i++)
            {
                var pair = layers[i].GetOutputAndDiff(output);
                output = pair.Key;
                list.Add(pair);
            }
            return list;
        }

        public virtual void GetOutAndDiff(double[,,] input, int pos)
        {
            var output = input;
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].GetOutputAndDiff(output, cached_outputs[pos][i], cached_diffs[pos][i]);
                output = cached_outputs[pos][i];
            }
        }

        /// <summary>
        /// Получить ошибку лосса
        /// </summary>
        /// <param name="input"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual double GetError(double[,,] input, double[,,] t)
        {
            double[,,] output = GetOutput(input);
            return loss.Metric(output, t);
        }
        
        /// <summary>
        /// Возращает ошибку с процентами угаданных максимумов
        /// </summary>
        /// <param name="input"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual KeyValuePair<double,double> GetErrorPair(double[,,] input, double[,,] t)
        {
            double[,,] output = GetOutput(input);

            int depth2 = output.GetLength(0);
            int height2 = output.GetLength(1);
            int width2 = output.GetLength(2);

            var loss_err = loss.Metric(output, t);
            

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

            return new KeyValuePair<double, double>(loss_err, max_arg == max_arg2 ? 1 : 0);
        }

        public virtual KeyValuePair<double,double> GetError(IDataEnumerator data)
        {
            data.Reset();
            double err = 0;
            double wins = 0;
            int count = 0;

            object lock_target = new object();
            Parallel.ForEach(data, (needed) =>
            {
                var pair = GetErrorPair(needed.input, needed.output);
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

        /// <summary>
        /// Возращает среднюю метрику metric по всему DataEnumerator
        /// </summary>
        /// <param name="data"></param>
        /// <param name="metric"></param>
        /// <returns></returns>
        public virtual double GetMeanMetric(IDataEnumerator data, IMetric metric)
        {
            data.Reset();
            double mean_metric = 0;
            int count = 0;

            object lock_target = new object();
            Parallel.ForEach(data, (needed) =>
            {
                double[,,] output = GetOutput(needed.input);
                var m = metric.Metric(output, needed.output);
                lock (lock_target)
                {
                    mean_metric += m;
                    count++;
                }
            });
            data.Reset();
            mean_metric /= count;
            return mean_metric;
        }

        public virtual double[] GetMeanMetrics(IDataEnumerator data, List<IMetric> metrics)
        {
            double[] vals = new double[metrics.Count];
            data.Reset();
            int count = 0;

            object lock_target = new object();
            Parallel.ForEach(data, (needed) =>
            {
                double[,,] output = GetOutput(needed.input);
                for (int i = 0; i < metrics.Count; i++)
                {

                    var m = metrics[i].Metric(output, needed.output);
                    lock (lock_target)
                    {
                        vals[i] += m;
                        count++;
                    }
                }
            });
            data.Reset();
            for(int i = 0;i < vals.Length;i++)
            {
                vals[i] /= count;
            }
            return vals;
        }

        public virtual KeyValuePair<double,double[,,]> Learn(double[,,] input, double[,,] t,double k)
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
            

            double[,,] out_err = errors.Last();

            loss.Error(output, t, out_err);

            double[,,] diff = layers.Last().GetCachedDiff();

            for (int z2 = 0; z2 < depth2; z2++)
            {
                for (int y2 = 0; y2 < height2; y2++)
                {
                    for (int x2 = 0; x2 < width2; x2++)
                    {
                        //temp = t[z2, y2, x2] - output[z2, y2, x2];
                        out_err[z2, y2, x2] *= -diff[z2, y2, x2];
                        
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

            return new KeyValuePair<double, double[,,]>(loss.Metric(output,t),output);
        }

        protected virtual double[][,,] GetBackwardErrors(double[,,] input, double[,,] t, out double err, out List<IOPair> getOutput)
        {
            throw new NotImplementedException();
            //double[][,,] errors = new double[layers.Count][,,];
            //var out_pairs = GetOutAndDiff(input);

            //double[,,] output = out_pairs.Last().Key;

            ////Console.WriteLine("{0} {1}", (float)output[0, 0, 0], (float)output[0, 0, 1]);

            //int depth2 = output.GetLength(0);
            //int height2 = output.GetLength(1);
            //int width2 = output.GetLength(2);

            //double val = 0;
            //double temp = 0;

            //double[,,] out_err = (double[,,])t.Clone();

            //double[,,] diff = out_pairs.Last().Value;

            //#region Расчет ошибки конечного слоя
            //for (int z2 = 0; z2 < depth2; z2++)
            //{
            //    for (int y2 = 0; y2 < height2; y2++)
            //    {
            //        for (int x2 = 0; x2 < width2; x2++)
            //        {
            //            temp = t[z2, y2, x2] - output[z2, y2, x2];
            //            out_err[z2, y2, x2] = -temp * diff[z2, y2, x2];
            //            val += temp * temp;

            //        }
            //    }
            //}
            //#endregion


            //#region Обратное распостронение ошибки
            //errors[errors.Length - 1] = out_err;

            //for (int i = layers.Count - 1; i >= 0; i--)
            //{
            //    Layer prev = null;
            //    if (i != 0)
            //    {
            //        prev = layers[i - 1];
            //        if (prev != null)
            //        {
            //            errors[i - 1] = layers[i].GetError(errors[i], out_pairs[i - 1].Value, out_pairs[i - 1].Key);
            //            errors[i - 1].MaxNorm();
            //        }
            //        else
            //        {
            //            double[,,] diff2 = (double[,,])input.Clone();
            //            diff2.ForEach(x => 1);
            //            errors[i - 1] = layers[i].GetError(errors[i], diff2, input);
            //            errors[i - 1].MaxNorm();
            //        }
            //    }
            //}
            //#endregion

            //err = val;
            //getOutput = out_pairs;

            //return errors;
        }

        protected virtual double[][,,] GetBackwardErrors(double[,,] input, double[,,] t, out double err, int pos)
        {
            GetOutAndDiff(input, pos);

            double[,,] output = cached_outputs[pos].Last();

            //Console.WriteLine("{0} {1}", (float)output[0, 0, 0], (float)output[0, 0, 1]);

            int depth2 = output.GetLength(0);
            int height2 = output.GetLength(1);
            int width2 = output.GetLength(2);
            

            double[,,] out_err = (double[,,])t.Clone();
            loss.Error(output, t, out_err);

            double[,,] diff = cached_diffs[pos].Last();

            #region Расчет ошибки конечного слоя
            for (int z2 = 0; z2 < depth2; z2++)
            {
                for (int y2 = 0; y2 < height2; y2++)
                {
                    for (int x2 = 0; x2 < width2; x2++)
                    {
                        out_err[z2, y2, x2] *= -diff[z2, y2, x2];

                    }
                }
            }
            #endregion


            #region Обратное распостронение ошибки
            cached_errors[pos][cached_errors[pos].Length - 1] = out_err;

            for (int i = layers.Count - 1; i >= 0; i--)
            {
                Layer prev = null;
                if (i != 0)
                {
                    prev = layers[i - 1];
                    if (prev != null)
                    {
                        layers[i].GetError(cached_errors[pos][i], cached_diffs[pos][i - 1], cached_outputs[pos][i - 1], cached_errors[pos][i - 1]);
                        //cached_errors[pos][i - 1].MaxNorm();
                    }
                    else
                    {
                        double[,,] diff2 = (double[,,])input.Clone();
                        diff2.ForEach(x => 1);
                        layers[i].GetError(errors[i], diff2, input, cached_errors[pos][i - 1]);
                        //cached_errors[pos][i - 1].MaxNorm();
                    }
                }
            }
            #endregion

            err = loss.Metric(output,t);

            return cached_errors[pos];
        }



        //List<double[][,,]> error_list = new List<double[][,,]>();
        public void CreateGradients(int count)
        {
            cached_outputs = new double[count][][,,];
            cached_diffs = new double[count][][,,];
            cached_errors = new double[count][][,,];

            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].CreateGradients(count);
            }

            for(int c = 0;c < count;c++)
            {
                cached_outputs[c] = new double[layers.Count][,,];
                cached_diffs[c] = new double[layers.Count][,,];
                cached_errors[c] = new double[layers.Count][,,];
                for (int i = 0; i < layers.Count; i++)
                {
                    cached_outputs[c][i] = layers[i].CreateOutput();
                    cached_diffs[c][i] = layers[i].CreateOutput();
                    cached_errors[c][i] = layers[i].CreateOutput();
                }

            }
        }

        public virtual KeyValuePair<double,double[,,]> WriteG(int pos, double[,,] input, double[,,] t, double k)
        {
            var val = SetBackwardErrors(input, t);

            layers[0].WriteG(pos,errors[0], input, k);

            Parallel.For(1, layers.Count, i => 
            {
                layers[i].WriteG(pos,errors[i], layers[i - 1].GetCachedOutpur(), k);
            });

            return val;
        }

        public virtual KeyValuePair<double,double[,,]> ParallelWriteG(int pos, double[,,] input, double[,,] t, double k, double lambda = 1e-4)
        {
            double err;
            double[][,,] errors = GetBackwardErrors(input, t, out err, pos);

            layers[0].WriteG(pos, errors[0], input, lambda);
            layers[0].ActionG(pos, pos, (x) => x * k);

            for (int i = 1; i < layers.Count; i++)
            {
                layers[i].WriteG(pos, errors[i], cached_outputs[pos][i - 1], lambda);
                layers[i].ActionG(pos, pos, (x) => x * k);
            }


            //NormalizationGradient(pos, k);

            return new KeyValuePair<double, double[,,]>(err,cached_outputs[pos].Last());
        }

        public void NormalizationGradient(int pos, double k)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                double current_max = 0;
                layers[i].ActionG(pos, pos, (x) =>
                {
                    current_max = Math.Max(current_max, Math.Abs(x));
                    return x;
                });

                current_max /= k;

                if(current_max != 0)
                    layers[i].ActionG(pos, pos, (x) => x / current_max);
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

        public virtual string SaveJSON()
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

        public virtual void LoadJSON(string data)
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

        public virtual void ClearBuffers()
        {

        }
    }
}
