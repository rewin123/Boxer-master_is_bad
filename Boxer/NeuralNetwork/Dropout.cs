using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NeuralNetwork
{
    public class Dropout : Layer
    {
        double width;
        Random rand = new Random();

        double[,,] cached;

        public Dropout(Activation activation, double width = 0.25f) : base(activation)
        {
            this.width = width;
        }

        public Dropout(LayerData data) : base(data)
        {
            var ddata = JsonConvert.DeserializeObject<DData>(data.classData);
            width = ddata.width;
        }

        public override LayerData SaveJSON()
        {
            var data = base.SaveJSON();

            var ddats = new DData
            {
                width = width
            };

            data.classData = JsonConvert.SerializeObject(ddats);

            return data;
        }

        internal class DData
        {
            public double width;
        }

        public override Size Compile(Size input_size)
        {
            this.input_size = input_size;
            output_size = input_size;
            return input_size;
        }

        public override double[,,] GetOutput(ref double[,,] input)
        {
            int depth2 = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            double[,,] output = new double[depth2, height2, width2];

            for (int z2 = 0; z2 < depth2; z2++)
            {
                for (int y2 = 0; y2 < height2; y2++)
                {
                    for (int x2 = 0; x2 < width2; x2++)
                    {
                        output[z2, y2, x2] = input[z2, y2, x2] + (rand.NextDouble() - 0.5) * 2 * width;
                    }
                }
            }

            cached = (double[,,])output.Clone();

            return func.FastFunc(output);
        }

        public override KeyValuePair<double[,,], double[,,]> GetOutputAndDiff(double[,,] input)
        {
            int depth2 = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            double[,,] output = new double[depth2, height2, width2];

            for (int z2 = 0; z2 < depth2; z2++)
            {
                for (int y2 = 0; y2 < height2; y2++)
                {
                    for (int x2 = 0; x2 < width2; x2++)
                    {
                        output[z2, y2, x2] = input[z2, y2, x2] + (rand.NextDouble() - 0.5) * 2 * width;
                    }
                }
            }


            return new KeyValuePair<double[,,], double[,,]>(func.Func(output), func.Diff(output));
        }

        public override double[,,] GetCachedDiff()
        {
            return func.Diff(cached);
        }

        public override double[,,] GetCachedOutpur()
        {
            return func.Func(cached);
        }

        public override double[,,] GetError(double[,,] error_out, double[,,] diff, double[,,] input)
        {

            int depth2 = input_size[0];
            int height2 = input_size[1];
            int width2 = input_size[2];

            double[,,] output = new double[depth2, height2, width2];

            for (int z2 = 0; z2 < depth2; z2++)
            {
                for (int y2 = 0; y2 < height2; y2++)
                {
                    for (int x2 = 0; x2 < width2; x2++)
                    {
                        output[z2, y2, x2] = error_out[z2, y2, x2] * diff[z2, y2, x2];
                    }
                }
            }

            return output;
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
