using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NeuralNetwork
{
    public class TSConv2D : Layer
    {
        int width_in;
        int height_in;
        int ch_in;
        int width_out;
        int height_out;
        int ch_out;
        /// <summary>
        /// (sample,chanel,y,x)
        /// </summary>
        public double[,,,,,] weights;

        double[,,] cache_sum;
        
        public TSConv2D(Activation activation, int width_in, int height_in, int ch_out, int width_out, int height_out) : base(activation)
        {
            this.width_in = width_in;
            this.height_in = height_in;
            this.ch_out = ch_out;
            this.width_out = width_out;
            this.height_out = height_out;
        }

        public TSConv2D(LayerData data) : base(data)
        {
            var ts = JsonConvert.DeserializeObject<TSData>(data.classData);
            this.width_in = ts.width_in;
            this.height_in = ts.height_in;
            this.ch_out = ts.ch_out;
            this.width_out = ts.width_out;
            this.height_out = ts.height_out;
            weights = ts.weights;
        }

        public override LayerData SaveJSON()
        {
            LayerData dd = base.SaveJSON();

            TSData ts = new TSData
            {
                width_in = width_in,
                height_in = height_in,
                ch_out = ch_out,
                width_out = width_out,
                height_out = height_out,
                weights = weights
            };

            dd.classData = JsonConvert.SerializeObject(ts);

            return dd;
        }

        internal class TSData
        {
            public double[,,,,,] weights;
            public int width_in;
            public int height_in;
            public int ch_in;
            public int width_out;
            public int height_out;
            public int ch_out;
        }

        public override Size Compile(Size input_size)
        {
            this.input_size = input_size;
            ch_in = input_size[0];
            weights = new double[ch_out, height_out, width_out, ch_in, height_in, width_in];

            AddNoise();

            this.input_size = input_size;
            output_size = new Size(ch_out, input_size[1] - height_in + height_out, input_size[2] - width_in + width_out);
            return new Size(ch_out, input_size[1] - height_in + height_out, input_size[2] - width_in + width_out);
        }

        public override void AddNoise(double width = 1)
        {
            Random r = new Random();
            for (int c1 = 0; c1 < ch_out; c1++)
            {
                for (int y1 = 0; y1 < height_out; y1++)
                {
                    for (int x1 = 0; x1 < width_out; x1++)
                    {
                        for(int c2 = 0;c2 < ch_in;c2++)
                        {
                            for(int y2 = 0;y2 < height_in;y2++)
                            {
                                for(int x2 = 0;x2 < width_in;x2++)
                                {
                                    weights[c1, y1, x1, c2, y2, x2] = (r.NextDouble() - 0.5) * width;
                                }
                            }
                        }
                    }
                }
            }
        }

        void CreateMask()
        {

        }


        public override double[,,] GetOutput(ref double[,,] input)
        {

            int dh = input.GetLength(1) - height_in + 1;
            int dw = input.GetLength(2) - width_in + 1;

            double[,,] output = new double[ch_out, dh + height_out, dw + width_out];

            double val = 0;
            for (int c1 = 0; c1 < ch_out; c1++)
            {
                for (int c2 = 0; c2 < ch_in; c2++)
                {
                    for (int y0 = 0; y0 < dh; y0++)
                    {
                        for (int x0 = 0; x0 < dw; x0++)
                        {
                            for (int y1 = 0; y1 < height_out; y1++)
                            {
                                for (int x1 = 0; x1 < width_out; x1++)
                                {
                                    val = 0;
                                    for (int y2 = 0; y2 < height_in; y2++)
                                    {
                                        for (int x2 = 0; x2 < width_in; x2++)
                                        {
                                            val += weights[c1, y1, x1, c2, y2, x2] * input[c2, y0 + y2, x0 + x2];
                                        }
                                    }

                                    output[c1, y0 + y1, x0 + x1] += val;
                                }
                            }
                        }
                    }
                }
            }
            cache_sum = (double[,,])output.Clone();

            return func.Func(output);
        }

        public override KeyValuePair<double[,,], double[,,]> GetOutputAndDiff(double[,,] input)
        {
            int dh = input.GetLength(1) - height_in + 1;
            int dw = input.GetLength(2) - width_in + 1;

            double[,,] output = new double[ch_out, dh + height_out - 1, dw + width_out - 1];

            double val = 0;
            for (int c1 = 0; c1 < ch_out; c1++)
            {
                for (int c2 = 0; c2 < ch_in; c2++)
                {
                    for (int y0 = 0; y0 < dh; y0++)
                    {
                        for (int x0 = 0; x0 < dw; x0++)
                        {
                            for (int y1 = 0; y1 < height_out; y1++)
                            {
                                for (int x1 = 0; x1 < width_out; x1++)
                                {
                                    val = 0;
                                    for (int y2 = 0; y2 < height_in; y2++)
                                    {
                                        for (int x2 = 0; x2 < width_in; x2++)
                                        {
                                            val += weights[c1, y1, x1, c2, y2, x2] * input[c2, y0 + y2, x0 + x2];
                                        }
                                    }

                                    output[c1, y0 + y1, x0 + x1] += val;
                                }
                            }
                        }
                    }
                }
            }

            return new KeyValuePair<double[,,], double[,,]>( func.Func(output), func.Diff(output));
        }

        public override double[,,] GetCachedOutpur()
        {
            return func.Func(cache_sum);
        }

        public override double[,,] GetCachedDiff()
        {
            return func.Diff(cache_sum);
        }

        public override double[,,] GetError(double[,,] error_out, double[,,] diff, double[,,] indata)
        {

            double[,,] input = doubleArrayExtensions.CreateArray(input_size);

            int dh = input.GetLength(1) - height_in + 1;
            int dw = input.GetLength(2) - width_in + 1;

            double val = 0;
            for (int c1 = 0; c1 < ch_out; c1++)
            {
                for (int c2 = 0; c2 < ch_in; c2++)
                {
                    for (int y0 = 0; y0 < dh; y0++)
                    {
                        for (int x0 = 0; x0 < dw; x0++)
                        {
                            for (int y1 = 0; y1 < height_out; y1++)
                            {
                                for (int x1 = 0; x1 < width_out; x1++)
                                {
                                    for (int y2 = 0; y2 < height_in; y2++)
                                    {
                                        for (int x2 = 0; x2 < width_in; x2++)
                                        {
                                            input[c2,y0 + y2,x0 + x2] += weights[c1, y1, x1, c2, y2, x2] * error_out[c1, y0 + y1, x0 + x1];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            for(int c = 0;c < ch_in;c++)
            {
                for(int y = 0;y < input_size[1];y++)
                {
                    for(int x = 0;x < input_size[2];x++)
                    {
                        input[c, y, x] *= diff[c, y, x];
                    }
                }
            }

            return input;
        }

        public override void ChangeWeights(double[,,] error_out, double[,,] input, double k)
        {
            //int depth = input_size[0];
            //int height2 = input_size[1];
            //int width2 = input_size[2];

            //int dh = height2 - height + 1;
            //int dw = width2 - width + 1;


            //for (int c = 0; c < count; c++)
            //{
            //    for (int y0 = 0; y0 < dh; y0++)
            //    {
            //        for (int x0 = 0; x0 < dw; x0++)
            //        {
            //            double val = 0;
            //            for (int ch = 0; ch < depth; ch++)
            //            {
            //                for (int y = 0; y < height; y++)
            //                {
            //                    for (int x = 0; x < width; x++)
            //                    {
            //                        weights[c, ch, y, x] -= input[ch, y0 + y, x0 + x] * error_out[c, y0, x0] * k;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            throw new NotImplementedException();
        }

        List<double[,,,,,]> grads = new List<double[,,,,,]>();

        public override void CreateGradients(int count)
        {
            grads.Clear();

            for (int i = 0; i < count; i++)
            {
                grads.Add(new double[ch_out,height_out,width_out,ch_in,height_in,width_in]);
            }
        }

        public override void ActionG(int pos, int to, Func<double, double> func)
        {
            var from = grads[pos];
            var to_g = grads[to];
            for (int c1 = 0; c1 < ch_out; c1++)
            {
                for (int c2 = 0; c2 < ch_in; c2++)
                {
                    for (int y1 = 0; y1 < height_out; y1++)
                    {
                        for (int x1 = 0; x1 < width_out; x1++)
                        {
                            for (int y2 = 0; y2 < height_in; y2++)
                            {
                                for (int x2 = 0; x2 < width_in; x2++)
                                {
                                    to_g[c1, y1, x1, c2, y2, x2] = func(from[c1, y1, x1, c2, y2, x2]);
                                }
                            }
                        }
                    }
                }
            }

            grads[to] = to_g;
        }

        public override void ActionTwoG(int pos1, int pos2, int to, Func<double, double, double> func)
        {
            var from1 = grads[pos1];
            var from2 = grads[pos2];
            var to_g = grads[to];
            for (int c1 = 0; c1 < ch_out; c1++)
            {
                for (int c2 = 0; c2 < ch_in; c2++)
                {
                    for (int y1 = 0; y1 < height_out; y1++)
                    {
                        for (int x1 = 0; x1 < width_out; x1++)
                        {
                            for (int y2 = 0; y2 < height_in; y2++)
                            {
                                for (int x2 = 0; x2 < width_in; x2++)
                                {
                                    to_g[c1, y1, x1, c2, y2, x2] = func(from1[c1, y1, x1, c2, y2, x2], from2[c1, y1, x1, c2, y2, x2]);
                                }
                            }
                        }
                    }
                }
            }

            grads[to] = to_g;
        }

        public override void WriteG(int pos, double[,,] error_out, double[,,] input, double k)
        {
            int depth = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            int dh = height2 - height_in + 1;
            int dw = width2 - width_in + 1;

            var to = grads[pos];

            for (int c1 = 0; c1 < ch_out; c1++)
            {
                for (int c2 = 0; c2 < ch_in; c2++)
                {
                    for (int y1 = 0; y1 < height_out; y1++)
                    {
                        for (int x1 = 0; x1 < width_out; x1++)
                        {
                            for (int y2 = 0; y2 < height_in; y2++)
                            {
                                for (int x2 = 0; x2 < width_in; x2++)
                                {
                                    to[c1, y1, x1, c2, y2, x2] = 0;
                                }
                            }
                        }
                    }
                }
            }

            for (int c1 = 0; c1 < ch_out; c1++)
            {
                for (int c2 = 0; c2 < ch_in; c2++)
                {
                    for (int y0 = 0; y0 < dh; y0++)
                    {
                        for (int x0 = 0; x0 < dw; x0++)
                        {
                            for (int y1 = 0; y1 < height_out; y1++)
                            {
                                for (int x1 = 0; x1 < width_out; x1++)
                                {
                                    for (int y2 = 0; y2 < height_in; y2++)
                                    {
                                        for (int x2 = 0; x2 < width_in; x2++)
                                        {
                                            to[c1, y1, x1, c2, y2, x2] += input[c2, y0 + y2, x0 + x2] * error_out[c1, y0 + y1, x0 + x1] * k;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            

            grads[pos] = to;
        }

        public override void GradWeights(int pos)
        {
            var to = grads[pos];

            for (int c1 = 0; c1 < ch_out; c1++)
            {
                for (int c2 = 0; c2 < ch_in; c2++)
                {
                    for (int y1 = 0; y1 < height_out; y1++)
                    {
                        for (int x1 = 0; x1 < width_out; x1++)
                        {
                            for (int y2 = 0; y2 < height_in; y2++)
                            {
                                for (int x2 = 0; x2 < width_in; x2++)
                                {
                                    weights[c1, y1, x1, c2, y2, x2] -= to[c1, y1, x1, c2, y2, x2];
                                }
                            }
                        }
                    }
                }
            }
        }

        public override double NormG(int pos)
        {
            double val = 0;

            var to = grads[pos];

            for (int c1 = 0; c1 < ch_out; c1++)
            {
                for (int c2 = 0; c2 < ch_in; c2++)
                {
                    for (int y1 = 0; y1 < height_out; y1++)
                    {
                        for (int x1 = 0; x1 < width_out; x1++)
                        {
                            for (int y2 = 0; y2 < height_in; y2++)
                            {
                                for (int x2 = 0; x2 < width_in; x2++)
                                {
                                   val += to[c1, y1, x1, c2, y2, x2] * to[c1, y1, x1, c2, y2, x2];
                                }
                            }
                        }
                    }
                }
            }

            return Math.Sqrt(val / to.Length);
        }

        public override void NormalizationW()
        {
            double val = 0;

            var to = weights;

            for (int c1 = 0; c1 < ch_out; c1++)
            {
                for (int c2 = 0; c2 < ch_in; c2++)
                {
                    for (int y1 = 0; y1 < height_out; y1++)
                    {
                        for (int x1 = 0; x1 < width_out; x1++)
                        {
                            for (int y2 = 0; y2 < height_in; y2++)
                            {
                                for (int x2 = 0; x2 < width_in; x2++)
                                {
                                    val += to[c1, y1, x1, c2, y2, x2] * to[c1, y1, x1, c2, y2, x2];
                                }
                            }
                        }
                    }
                }
            }

            val = 1 / val;
            for (int c1 = 0; c1 < ch_out; c1++)
            {
                for (int c2 = 0; c2 < ch_in; c2++)
                {
                    for (int y1 = 0; y1 < height_out; y1++)
                    {
                        for (int x1 = 0; x1 < width_out; x1++)
                        {
                            for (int y2 = 0; y2 < height_in; y2++)
                            {
                                for (int x2 = 0; x2 < width_in; x2++)
                                {
                                    to[c1, y1, x1, c2, y2, x2] *= val;
                                }
                            }
                        }
                    }
                }
            }
        }

        public override bool ITrained
        {
            get
            {
                return true;
            }
        }

        public override Layer Mirror
        {
            get
            {
                TSConv2D mirror = new TSConv2D(func, width_out, height_out, ch_in, width_in, height_in);
                for (int c1 = 0; c1 < ch_out; c1++)
                {
                    for (int c2 = 0; c2 < ch_in; c2++)
                    {
                        for (int y1 = 0; y1 < height_out; y1++)
                        {
                            for (int x1 = 0; x1 < width_out; x1++)
                            {
                                for (int y2 = 0; y2 < height_in; y2++)
                                {
                                    for (int x2 = 0; x2 < width_in; x2++)
                                    {
                                        mirror.weights[c2, y2, x2, c1, y1, x1] = weights[c1, y1, x1, c2, y2, x2];
                                    }
                                }
                            }
                        }
                    }
                }

                return mirror;
            }
        }
    }
}
