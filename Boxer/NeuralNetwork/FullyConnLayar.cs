using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NeuralNetwork
{
    public class FullyConnLayar : Layer
    {
        
        public double[,,,,,] weights;

        double[,,] cache_sum;
        
        public FullyConnLayar(Activation activation, Size output) : base(activation)
        {
            this.output_size = output;
        }

        public FullyConnLayar(LayerData data) : base(data)
        {
            FullConData fd = JsonConvert.DeserializeObject<FullConData>(data.classData);
            weights = fd.weights;
        }

        public override Size Compile(Size input_size)
        {
            this.input_size = input_size;
            weights = new double[output_size[0], output_size[1], output_size[2], input_size[0], input_size[1], input_size[2]];
            AddNoise(1e-3);
            return output_size;
        }

        public override void AddNoise(double noise_width = 1)
        {
            Random r = new Random();
            int depth1 = output_size[0];
            int height1 = output_size[1];
            int width1 = output_size[2];

            int depth2 = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            for (int z1 = 0; z1 < depth1; z1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    for (int x1 = 0; x1 < width1; x1++)
                    {
                        double val = 0;
                        for (int z2 = 0; z2 < depth2; z2++)
                        {
                            for (int y2 = 0; y2 < height2; y2++)
                            {
                                for (int x2 = 0; x2 < width2; x2++)
                                {
                                    weights[z1, y1, x1, z2, y2, x2] += (r.NextDouble()) * noise_width;
                                }
                            }
                        }
                        
                    }
                }
            }
        }

        public override double[,,] GetOutput(ref double[,,] input)
        {
            double[,,] output = doubleArrayExtensions.CreateArray(output_size);

            int depth1 = output_size[0];
            int height1 = output_size[1];
            int width1 = output_size[2];

            int depth2 = input.GetLength(0);
            int height2 = input.GetLength(1);
            int width2 = input.GetLength(2);
            
            for(int z1 = 0;z1 < depth1;z1++)
            {
                for(int y1 = 0;y1 < height1;y1++)
                {
                    for (int x1 = 0;x1 < width1;x1++)
                    {
                        double val = 0;
                        for (int z2 = 0; z2 < depth2; z2++)
                        {
                            for (int y2 = 0; y2 < height2; y2++)
                            {
                                for (int x2 = 0; x2 < width2; x2++)
                                {
                                    val += weights[z1, y1, x1, z2, y2, x2] * input[z2, y2, x2];
                                }
                            }
                        }

                        output[z1, y1, x1] = val;
                    }
                }
            }

            cache_sum = (double[,,])output.Clone();

            return func.Func(output);
        }

        public override KeyValuePair<double[,,], double[,,]> GetOutputAndDiff(double[,,] input)
        {
            double[,,] output = doubleArrayExtensions.CreateArray(output_size);

            int depth1 = output_size[0];
            int height1 = output_size[1];
            int width1 = output_size[2];

            int depth2 = input.GetLength(0);
            int height2 = input.GetLength(1);
            int width2 = input.GetLength(2);

            for (int z1 = 0; z1 < depth1; z1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    for (int x1 = 0; x1 < width1; x1++)
                    {
                        double val = 0;
                        for (int z2 = 0; z2 < depth2; z2++)
                        {
                            for (int y2 = 0; y2 < height2; y2++)
                            {
                                for (int x2 = 0; x2 < width2; x2++)
                                {
                                    val += weights[z1, y1, x1, z2, y2, x2] * input[z2, y2, x2];
                                }
                            }
                        }

                        output[z1, y1, x1] = val;
                    }
                }
            }


            return new KeyValuePair<double[,,], double[,,]>(func.Func(output), func.Diff(output));
        }

        public override double[,,] GetError(double[,,] error_out, double[,,] diff, double[,,] inarray)
        {
            

            double[,,] input = doubleArrayExtensions.CreateArray(input_size);

            int depth1 = input_size[0];
            int height1 = input_size[1];
            int width1 = input_size[2];

            int depth2 = error_out.GetLength(0);
            int height2 = error_out.GetLength(1);
            int width2 = error_out.GetLength(2);

            for (int z1 = 0; z1 < depth1; z1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    for (int x1 = 0; x1 < width1; x1++)
                    {
                        double val = 0;
                        for (int z2 = 0; z2 < depth2; z2++)
                        {
                            for (int y2 = 0; y2 < height2; y2++)
                            {
                                for (int x2 = 0; x2 < width2; x2++)
                                {
                                    val += weights[z2, y2, x2, z1, y1, x1] * error_out[z2, y2, x2];
                                }
                            }
                        }

                        input[z1, y1, x1] = val * diff[z1,y1,x1];
                    }
                }
            }

            return input;
        }

        public override void ChangeWeights(double[,,] error_out, double[,,] input, double k)
        {
            int depth1 = output_size[0];
            int height1 = output_size[1];
            int width1 = output_size[2];

            int depth2 = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            for (int z1 = 0; z1 < depth1; z1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    for (int x1 = 0; x1 < width1; x1++)
                    {
                        for (int z2 = 0; z2 < depth2; z2++)
                        {
                            for (int y2 = 0; y2 < height2; y2++)
                            {
                                for (int x2 = 0; x2 < width2; x2++)
                                {
                                    weights[z1, y1, x1, z2, y2, x2] -= input[z2, y2, x2] * error_out[z1, y1, x1] * k;
                                }
                            }
                        }
                    }
                }
            }
        }

        public override double[,,] GetCachedDiff()
        {
            return func.Diff(cache_sum);
        }

        public override double[,,] GetCachedOutpur()
        {
            return func.Func(cache_sum);
        }

        [NonSerialized]
        List<double[,,,,,]> grads = new List<double[,,,,,]>();

        public override void CreateGradients(int count)
        {
            grads.Clear();

            for(int i = 0;i < count; i++)
            {
                grads.Add(new double[output_size[0], output_size[1], output_size[2], input_size[0], input_size[1], input_size[2]]);
            }
        }

        public override void ActionG(int pos, int to, Func<double, double> func)
        {
            int depth1 = output_size[0];
            int height1 = output_size[1];
            int width1 = output_size[2];

            int depth2 = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            var from = grads[pos];
            var to_g = grads[to];

            for (int z1 = 0; z1 < depth1; z1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    for (int x1 = 0; x1 < width1; x1++)
                    {
                        for (int z2 = 0; z2 < depth2; z2++)
                        {
                            for (int y2 = 0; y2 < height2; y2++)
                            {
                                for (int x2 = 0; x2 < width2; x2++)
                                {
                                    to_g[z1, y1, x1, z2, y2, x2] = func(from[z1, y1, x1, z2, y2, x2]);
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
            int depth1 = output_size[0];
            int height1 = output_size[1];
            int width1 = output_size[2];

            int depth2 = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            var from1 = grads[pos1];
            var from2 = grads[pos2];
            var to_g = grads[to];

            for (int z1 = 0; z1 < depth1; z1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    for (int x1 = 0; x1 < width1; x1++)
                    {
                        double val = 0;
                        for (int z2 = 0; z2 < depth2; z2++)
                        {
                            for (int y2 = 0; y2 < height2; y2++)
                            {
                                for (int x2 = 0; x2 < width2; x2++)
                                {
                                    to_g[z1, y1, x1, z2, y2, x2] = func(from1[z1, y1, x1, z2, y2, x2], from2[z1, y1, x1, z2, y2, x2]);
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
            int depth1 = output_size[0];
            int height1 = output_size[1];
            int width1 = output_size[2];

            int depth2 = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            var to = grads[pos];

            Parallel.For(0, depth1, z1 => 
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    for (int x1 = 0; x1 < width1; x1++)
                    {
                        double val = 0;
                        for (int z2 = 0; z2 < depth2; z2++)
                        {
                            for (int y2 = 0; y2 < height2; y2++)
                            {
                                for (int x2 = 0; x2 < width2; x2++)
                                {
                                    to[z1, y1, x1, z2, y2, x2] = input[z2, y2, x2] * error_out[z1, y1, x1] * k;
                                }
                            }
                        }
                    }
                }
            });

            grads[pos] = to;
        }

        public override void GradWeights(int pos)
        {
            int depth1 = output_size[0];
            int height1 = output_size[1];
            int width1 = output_size[2];

            int depth2 = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            var from = grads[pos];

            for (int z1 = 0; z1 < depth1; z1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    for (int x1 = 0; x1 < width1; x1++)
                    {
                        for (int z2 = 0; z2 < depth2; z2++)
                        {
                            for (int y2 = 0; y2 < height2; y2++)
                            {
                                for (int x2 = 0; x2 < width2; x2++)
                                {
                                    weights[z1, y1, x1, z2, y2, x2] -= from[z1, y1, x1, z2, y2, x2];
                                }
                            }
                        }
                    }
                }
            }
        }

        public override double NormG(int pos)
        {
            int depth1 = output_size[0];
            int height1 = output_size[1];
            int width1 = output_size[2];

            int depth2 = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            var from = grads[pos];

            double val = 0;

            for (int z1 = 0; z1 < depth1; z1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    for (int x1 = 0; x1 < width1; x1++)
                    {
                        for (int z2 = 0; z2 < depth2; z2++)
                        {
                            for (int y2 = 0; y2 < height2; y2++)
                            {
                                for (int x2 = 0; x2 < width2; x2++)
                                {
                                   val += from[z1, y1, x1, z2, y2, x2] * from[z1, y1, x1, z2, y2, x2];
                                }
                            }
                        }
                    }
                }
            }

            return Math.Sqrt(val);
        }

        public override void NormalizationW()
        {
            int depth1 = output_size[0];
            int height1 = output_size[1];
            int width1 = output_size[2];

            int depth2 = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            var from = weights;

            double val = 0;

            for (int z1 = 0; z1 < depth1; z1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    for (int x1 = 0; x1 < width1; x1++)
                    {
                        for (int z2 = 0; z2 < depth2; z2++)
                        {
                            for (int y2 = 0; y2 < height2; y2++)
                            {
                                for (int x2 = 0; x2 < width2; x2++)
                                {
                                    val += from[z1, y1, x1, z2, y2, x2] * from[z1, y1, x1, z2, y2, x2];
                                }
                            }
                        }
                    }
                }
            }

            val = weights.Length / val;

            for (int z1 = 0; z1 < depth1; z1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    for (int x1 = 0; x1 < width1; x1++)
                    {
                        for (int z2 = 0; z2 < depth2; z2++)
                        {
                            for (int y2 = 0; y2 < height2; y2++)
                            {
                                for (int x2 = 0; x2 < width2; x2++)
                                {
                                    from[z1, y1, x1, z2, y2, x2] *= val;
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
                int depth1 = output_size[0];
                int height1 = output_size[1];
                int width1 = output_size[2];

                int depth2 = input_size[0];
                int height2 = input_size[1];
                int width2 = input_size[2];

                FullyConnLayar mirror = new FullyConnLayar(func, input_size.CloneMe());
                mirror.Compile(output_size.CloneMe());

                for (int z1 = 0; z1 < depth1; z1++)
                {
                    for (int y1 = 0; y1 < height1; y1++)
                    {
                        for (int x1 = 0; x1 < width1; x1++)
                        {
                            for (int z2 = 0; z2 < depth2; z2++)
                            {
                                for (int y2 = 0; y2 < height2; y2++)
                                {
                                    for (int x2 = 0; x2 < width2; x2++)
                                    {
                                        mirror.weights[z2, y2, x2, z1, y1, x1] = weights[z1, y1, x1, z2, y2, x2];
                                    }
                                }
                            }
                        }
                    }
                }

                return mirror;
            }
        }

        public override LayerData SaveJSON()
        {
            LayerData data = base.SaveJSON();
            FullConData fd = new FullConData();
            fd.weights = weights;
            data.classData = JsonConvert.SerializeObject(fd);
            return data;
        }

        [Serializable]
        internal class FullConData
        {
            public double[,,,,,] weights;
        }
    }
}
