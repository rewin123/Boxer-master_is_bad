using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NeuralNetwork
{
    public class MaxPool2D : Layer
    {
        int width;
        int height;

        double[,,] cache_sum;
        double[,,] cache_diff;
        double[,,] cache_input;

        public MaxPool2D(Activation activation, int width, int height) : base(activation)
        {
            this.width = width;
            this.height = height;
        }

        public MaxPool2D(LayerData data) : base(data)
        {
            MP mp = JsonConvert.DeserializeObject<MP>(data.classData);
            width = mp.width;
            height = mp.height;
        }

        public override LayerData SaveJSON()
        {
            LayerData data = base.SaveJSON();

            var mp = new MP
            {
                width = width,
                height = height
            };

            data.classData = JsonConvert.SerializeObject(mp);

            return data;
        }

        [Serializable]
        internal class MP
        {
            public int width;
            public int height;
        }

        public override Size Compile(Size input_size)
        {
            this.input_size = input_size;
            output_size = new Size(input_size[0],
                input_size[1] / height + (input_size[1] % height == 0 ? 0 : 1),
                input_size[2] / width + (input_size[2] % width == 0 ? 0 : 1));
            return output_size;
        }

        public override double[,,] GetOutput(double[,,] input)
        {
            cache_input = (double[,,])input.Clone();
            double[,,] output = new double[output_size[0], output_size[1], output_size[2]];

            int depth = input_size[0];
            int height = input_size[1];
            int width = input_size[2];

            for(int z = 0;z < depth;z++)
            {
                for(int y= 0;y < height;y++)
                {
                    for(int x = 0;x < width;x++)
                    {
                        output[z, y / this.height, x / this.width] = Math.Max(output[z, y / this.height, x / this.width], input[z, y, x]);
                    }
                }
            }

            cache_diff = func.Diff(output);
            output = func.FastFunc(output);
            
            cache_sum = (double[,,])output.Clone();

            return output;
        }

        public override void GetOutputAndDiff(double[,,] input, double[,,] output, double[,,] diff)
        {

            int depth = input_size[0];
            int height = input_size[1];
            int width = input_size[2];

            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        output[z, y / this.height, x / this.width] = Math.Max(output[z, y / this.height, x / this.width], input[z, y, x]);
                    }
                }
            }

            func.SyncDiff(output, diff);
            func.FastFunc(output);
        }

        public override double[,,] GetCachedDiff()
        {
            return cache_diff;
        }

        public override double[,,] GetCachedOutpur()
        {
            return cache_sum;
        }

        public override void GetError(double[,,] error_out, double[,,] diff, double[,,] inarray, double[,,] input)
        {
            int depth = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];


            double[,,] output = GetOutput(inarray);
            
            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height2; y++)
                {
                    for (int x = 0; x < width2; x++)
                    {
                        input[z, y, x] = output[z,y / height,x / width] == inarray[z,y,x] ? error_out[z, y / height, x / width] : 0;
                    }
                }
            }
        }

        public override void ChangeWeights(double[,,] error_out, double[,,] input, double k)
        {
            
        }

        public override bool ITrained
        {
            get
            {
                return false;
            }
        }
    }
}
