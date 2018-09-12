using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace NeuralNetwork
{
    public class NetworkVisualizer : IEnumerator<Bitmap>
    {
        Network network;
        double[,,] input;

        public int layer = 0;
        public int channel = 0;

        public Bitmap Current
        {
            get
            {
                var pairs = network.GetOutAndDiff(input);
                double[,,] arr = pairs[layer].Key;
                double max = -1;
                arr.ForEach((x) => { max = Math.Max(max, x); });
                arr.ForEach((x) => x / max);
                return ImageDataConverter.GetImage(arr.ConvertToColoredGray(channel));
            }
        }

        public double[,,] CurrentWeights
        {
            get
            {
                if (network.layers[layer] is Conv2D)
                {
                    var conv = network.layers[layer] as Conv2D;
                    double[,,] sample = new double[conv.weights.GetLength(1), conv.weights.GetLength(2), conv.weights.GetLength(3)];
                    for(int c = 0;c < sample.GetLength(0);c++)
                    {
                        for (int y = 0; y < sample.GetLength(1); y++)
                        {
                            for (int x = 0; x < sample.GetLength(2); x++)
                            {
                                sample[c, y, x] = conv.weights[channel, c, y, x];
                            }
                        }
                    }
                    for (int c = 0; c < sample.GetLength(0); c++)
                    {
                        string s = "";
                        for (int y = 0; y < sample.GetLength(1); y++)
                        {
                            for (int x = 0; x < sample.GetLength(2); x++)
                            {
                                s += sample[c, y, x].ToString("F") + " ";
                            }
                            s += "\n";
                        }
                        MessageBox.Show(s);
                    }

                    return sample;
                }
                else return null;
            }
        }

        public void SaveWeights(string path)
        {
            var ws = CurrentWeights;
            if(ws != null)
            {
                File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(ws));
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public NetworkVisualizer(Network network, double[,,] input)
        {
            this.network = network;
            this.input = input;
        }

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            channel++;
            if(network.layers[layer].output_size[0] <= channel)
            {
                channel = 0;
                layer++;
            }

            return layer < network.layers.Count;
        }

        public void Reset()
        {
            layer = 0;
            channel = 0;
        }
    }
}
