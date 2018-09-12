using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NeuralNetwork
{
    public class Conv2D : Layer
    {
        int width;
        int height;
        int count;
        int chanels;
        /// <summary>
        /// (sample,chanel,y,x)
        /// </summary>
        public double[,,,] weights;

        double[,,] cache_sum;
        
        public Conv2D(Activation activation, int width, int height, int count = 1) : base(activation)
        {
            this.width = width;
            this.height = height;
            this.count = count;
        }

        public Conv2D(LayerData data) : base(data)
        {
            Conv2DData cd = JsonConvert.DeserializeObject<Conv2DData>(data.classData);
            weights = cd.weights;
            width = cd.width;
            height = cd.height;
            count = cd.count;
            chanels = cd.chanels;
        }

        public override LayerData SaveJSON()
        {
            LayerData data = base.SaveJSON();

            var cd = new Conv2DData
            {
                weights = weights,
                width = width,
                height = height,
                count = count,
                chanels = chanels
            };
            data.classData = JsonConvert.SerializeObject(cd);

            return data;
        }

        [Serializable]
        internal class Conv2DData
        {
            public double[,,,] weights;
            public int width;
            public int height;
            public int count;
            public int chanels;
        }

        public override Size Compile(Size input_size)
        {
            this.input_size = input_size;
            chanels = input_size[0];
            weights = new double[count, chanels, height, width];

            AddNoise();

            output_size = new Size(count, input_size[1] - height + 1, input_size[2] - width + 1);

            return new Size(count, input_size[1] - height + 1, input_size[2] - width + 1);
        }

        public override void AddNoise(double width = 1)
        {
            Random r = new Random();
            double sum = 0;
            for(int c = 0;c < count;c++)
            {
                for(int ch = 0;ch < chanels;ch++)
                {
                    sum = 0;
                    for(int y = 0;y < height;y++)
                    {
                        for(int x = 0;x < this.width;x++)
                        {
                            weights[c, ch, y, x] += (r.NextDouble()) * width;
                            sum += weights[c, ch, y, x];
                        }
                    }

                    sum /= height * this.width;

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < this.width; x++)
                        {
                            weights[c, ch, y, x] -= sum;
                        }
                    }
                }
            }
        }

        void CreateMask()
        {

        }
        

        public override double[,,] GetOutput(double[,,] input)
        {
            int depth = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            int dh = height2 - height + 1;
            int dw = width2 - width + 1;

            double[,,] output = new double[count, dh, dw];

            for (int c = 0; c < count; c++)
            {
                for (int y0 = 0; y0 < dh; y0++)
                {
                    for (int x0 = 0; x0 < dw; x0++)
                    {
                        double val = 0;
                        for(int ch = 0;ch < depth;ch++)
                        {
                            for(int y = 0;y < height;y++)
                            {
                                for(int x = 0;x < width;x++)
                                {
                                    val += input[ch, y0 + y, x0 + x] * weights[c, ch, y, x];
                                }
                            }
                        }

                        output[c, y0, x0] = val;
                    }
                }
            }

            cache_sum = (double[,,])output.Clone();

            return func.Func(output);
        }

        public override void GetOutputAndDiff(double[,,] input, double[,,] output, double[,,] diff)
        {
            int depth = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            int dh = height2 - height + 1;
            int dw = width2 - width + 1;

            for(int c = 0;c < count; c++)
            {
                for (int y0 = 0; y0 < dh; y0++)
                {
                    for (int x0 = 0; x0 < dw; x0++)
                    {
                        double val = 0;
                        for (int ch = 0; ch < depth; ch++)
                        {
                            for (int y = 0; y < height; y++)
                            {
                                for (int x = 0; x < width; x++)
                                {
                                    val += input[ch, y0 + y, x0 + x] * weights[c, ch, y, x];
                                }
                            }
                        }

                        output[c, y0, x0] = val;
                    }
                }
            }

            func.SyncDiff(output, diff);
            output = func.FastFunc(output);
        }

        public override double[,,] GetCachedOutpur()
        {
            return func.Func(cache_sum);
        }

        public override double[,,] GetCachedDiff()
        {
            return func.Diff(cache_sum);
        }

        public override void GetError(double[,,] error_out, double[,,] diff, double[,,] indata, double[,,] input)
        {
            int depth = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            int dh = height2 - height + 1;
            int dw = width2 - width + 1;

            input.ForEach((x) => 0);

            for (int c = 0; c < count; c++)
            {
                for (int y0 = 0; y0 < dh; y0++)
                {
                    for (int x0 = 0; x0 < dw; x0++)
                    {
                        double val = 0;
                        for (int ch = 0; ch < depth; ch++)
                        {
                            for (int y = 0; y < height; y++)
                            {
                                for (int x = 0; x < width; x++)
                                {
                                    input[ch, y0 + y, x0 + x] += weights[c, ch, y, x] * error_out[c,y0,x0];
                                }
                            }
                        }
                        
                    }
                }
            }

            for(int z = 0;z < depth;z++)
            {
                for (int y = 0; y < height2; y++)
                {
                    for(int x = 0;x < width2;x++)
                    {
                        input[z, y, x] *= diff[z, y, x];
                    }
                }
            }
        }

        public override void ChangeWeights(double[,,] error_out, double[,,] input, double k)
        {
            int depth = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            int dh = height2 - height + 1;
            int dw = width2 - width + 1;


            for (int c = 0; c < count; c++)
            {
                for (int y0 = 0; y0 < dh; y0++)
                {
                    for (int x0 = 0; x0 < dw; x0++)
                    {
                        double val = 0;
                        for (int ch = 0; ch < depth; ch++)
                        {
                            for (int y = 0; y < height; y++)
                            {
                                for (int x = 0; x < width; x++)
                                {
                                    weights[c, ch, y, x] -= input[ch, y0 + y, x0 + x] * error_out[c, y0, x0] * k;
                                }
                            }
                        }
                    }
                }
            }
        }

        List<double[,,,]> grads = new List<double[,,,]>();

        public override void CreateGradients(int count)
        {
            grads.Clear();

            for (int i = 0; i < count; i++)
            {
                grads.Add(new double[this.count, chanels, height, width]);
            }
        }

        public override void ActionG(int pos, int to, Func<double, double> func)
        {
            double[,,,] from = grads[pos];
            double[,,,] to_g = grads[to];
            for(int c = 0;c < count;c++)
            {
                for(int ch = 0;ch < chanels;ch++)
                {
                    for(int y = 0;y < height;y++)
                    {
                        for(int x = 0;x < width;x++)
                        {
                            to_g[c, ch, y, x] = func(from[c, ch, y, x]);
                        }
                    }
                }
            }

            grads[to] = to_g;
        }

        public override void ActionTwoG(int pos1, int pos2, int to, Func<double, double, double> func)
        {
            double[,,,] from1 = grads[pos1];
            double[,,,] from2 = grads[pos2];
            double[,,,] to_g = grads[to];
            for (int c = 0; c < count; c++)
            {
                for (int ch = 0; ch < chanels; ch++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            to_g[c, ch, y, x] = func(from1[c, ch, y, x],from2[c, ch, y, x]);
                        }
                    }
                }
            }

            grads[to] = to_g;
        }

        public override void WriteG(int pos, double[,,] error_out, double[,,] input, double lambda)
        {
            int depth = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            int dh = height2 - height + 1;
            int dw = width2 - width + 1;

            double[,,,] to = grads[pos];

            for (int c = 0; c < count; c++)
            {
                for (int ch = 0; ch < chanels; ch++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            to[c, ch, y, x] = lambda * weights[c,ch,y,x];
                        }
                    }
                }
            }

            for (int c = 0; c < count; c++)
            {
                for (int y0 = 0; y0 < dh; y0++)
                {
                    for (int x0 = 0; x0 < dw; x0++)
                    {
                        double val = 0;
                        for (int ch = 0; ch < depth; ch++)
                        {
                            for (int y = 0; y < height; y++)
                            {
                                for (int x = 0; x < width; x++)
                                {
                                    to[c, ch, y, x] += input[ch, y0 + y, x0 + x] * error_out[c, y0, x0];
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
            double[,,,] to = grads[pos];

            for (int c = 0; c < count; c++)
            {
                for (int ch = 0; ch < chanels; ch++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            weights[c,ch,y,x] -= to[c, ch, y, x];
                        }
                    }
                }
            }
        }

        public override double NormG(int pos)
        {
            double val = 0;

            var to = grads[pos];

            for (int c = 0; c < count; c++)
            {
                for (int ch = 0; ch < chanels; ch++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            val += to[c, ch, y, x] * to[c, ch, y, x];
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

            for (int c = 0; c < count; c++)
            {
                for (int ch = 0; ch < chanels; ch++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            val += to[c, ch, y, x] * to[c, ch, y, x];
                        }
                    }
                }
            }

            val = 1 / val;
            for (int c = 0; c < count; c++)
            {
                for (int ch = 0; ch < chanels; ch++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            to[c, ch, y, x] *= val;
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
                TSConv2D mirror = new TSConv2D(func, 1, 1, chanels, width, height);

                mirror.Compile(new Size(count, input_size[1] - height + 1, input_size[2] - width + 1));

                for (int c = 0; c < count; c++)
                {
                    for (int ch = 0; ch < chanels; ch++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                mirror.weights[ch, y, x, c, 0, 0] = weights[c, ch, y, x];
                            }
                        }
                    }
                }


                return mirror;
            }
        }
        
        
    }
}
