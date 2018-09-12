using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork;

namespace NetworkLab
{
    static class NetworkData
    {
        public static string tag = ".neural";
        public static IDataEnumerator train;
        public static IDataEnumerator val;
        public static Network network;
        public static System.Drawing.Size image_size = new System.Drawing.Size(24, 12);
        public static Optimizer optimizer;

        public static void SaveNetwork(string name)
        {
            System.IO.File.WriteAllText(name + tag, network.SaveJSON());
        }

        public static void LoadNetwork(string path)
        {
            network = new Network();
            network.LoadJSON(System.IO.File.ReadAllText(path));
            network.CompileOnlyError();
        }
    }
}
