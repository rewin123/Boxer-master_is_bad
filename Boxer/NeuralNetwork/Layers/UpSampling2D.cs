using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NeuralNetwork.Layers
{
    public class UpSampling2D : Layer
    {
        int dx;
        int dy;
        public UpSampling2D(Activation activation, int dy, int dx) : base(activation)
        {
            this.dx = dx;
            this.dy = dy;
        }

        public UpSampling2D(LayerData data) : base(data)
        {
            var d = JsonConvert.DeserializeObject<UpSampling2DData>(data.classData);
            if (d != null)
            {
                dx = d.dx;
                dy = d.dy;
            }
            else
            {
                dx = 3;
                dy = 2;
            }
        }

        public override Size Compile(Size input_size)
        {
            this.input_size = input_size;
            output_size = new Size(input_size[0], input_size[1] * dy, input_size[2] * dx);
            return new Size(input_size[0], input_size[1] * dy, input_size[2] * dx);
        }

        public override double[,,] GetOutput(double[,,] input)
        {
            double[,,] output = CreateOutput();
            int ch_size = output_size[0];
            int h_size = input_size[1];
            int w_size = input_size[2];
            for(int ch = 0;ch < ch_size;ch++)
            {
                for(int y = 0;y < h_size;y++)
                {
                    for(int x = 0;x < w_size;x++)
                    {
                        double val = input[ch, y, x];
                        for(int yi = 0;yi < dy;yi++)
                        {
                            for(int xi = 0;xi < dx;xi++)
                            {
                                output[ch, y * dy + yi, x * dx + xi] = val;
                            }
                        }
                    }
                }
            }

            return func.FastFunc(output);
        }

        public override void GetOutputAndDiff(double[,,] input, double[,,] output, double[,,] diff)
        {
            int ch_size = output_size[0];
            int h_size = input_size[1];
            int w_size = input_size[2];
            for (int ch = 0; ch < ch_size; ch++)
            {
                for (int y = 0; y < h_size; y++)
                {
                    for (int x = 0; x < w_size; x++)
                    {
                        double val = input[ch, y, x];
                        for (int yi = 0; yi < dy; yi++)
                        {
                            for (int xi = 0; xi < dx; xi++)
                            {
                                output[ch, y * dy + yi, x * dx + xi] = val;
                            }
                        }
                    }
                }
            }

            func.SyncDiff(output, diff);
            func.FastFunc(output);
        }

        public override bool ITrained => true;

        public override void GetError(double[,,] error_out, double[,,] diff, double[,,] input, double[,,] result)
        {
            int ch_size = output_size[0];
            int h_size = input_size[1];
            int w_size = input_size[2];
            for (int ch = 0; ch < ch_size; ch++)
            {
                for (int y = 0; y < h_size; y++)
                {
                    for (int x = 0; x < w_size; x++)
                    {
                        double val = input[ch, y, x];
                        for (int yi = 0; yi < dy; yi++)
                        {
                            for (int xi = 0; xi < dx; xi++)
                            {
                                result[ch, y, x] += error_out[ch, y * dy + yi, x * dx + xi];
                            }
                        }
                    }
                }
            }

            for (int ch = 0; ch < ch_size; ch++)
            {
                for (int y = 0; y < h_size; y++)
                {
                    for (int x = 0; x < w_size; x++)
                    {
                        result[ch, y, x] *= diff[ch,y,x] / 4.0;
                    }
                }
            }
        }

        public override LayerData SaveJSON()
        {
            var data = base.SaveJSON();
            UpSampling2DData dd = new UpSampling2DData();
            dd.dx = dx;
            dd.dy = dy;
            data.classData = JsonConvert.SerializeObject(dd);
            return data;
        }
    }

    class UpSampling2DData
    {
        public int dx;
        public int dy;
    }
}
