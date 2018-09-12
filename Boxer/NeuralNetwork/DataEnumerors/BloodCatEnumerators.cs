using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class BloodCatEnumerators : IDataEnumerator
    {
        List<List<string>> paths = new List<List<string>>();
        public List<string> cat_names = new List<string>();
        List<double[,,]> results = new List<double[,,]>();
        Random r = new Random();
        System.Drawing.Size target_size;
        int pos = 0;
        int class_index = 0;

        double resize = 0;
        double rand_move = 0;
        public BloodCatEnumerators(string root_dir, System.Drawing.Size target_size)
        {
            this.target_size = target_size;
            cat_names.AddRange(Directory.GetDirectories(root_dir));
            Console.WriteLine("Finded {0} classes", cat_names.Count);
            for (int i = 0; i < cat_names.Count; i++)
            {
                List<string> p_class = new List<string>();
                p_class.AddRange(Directory.GetFiles(cat_names[i]));
                paths.Add(p_class);
                Console.WriteLine("{0}: {1}", cat_names[i].Split('\\').Last(), p_class.Count);
                double[,,] output = new double[1, 1, cat_names.Count];
                output[0, 0, i] = 1;
                results.Add(output);
            }
        }

        public IOPair Current
        {
            get
            {
                Bitmap map;
                try
                {
                    map = new Bitmap(paths[class_index][pos]);
                }
                catch
                {
                    MoveNext();
                    return Current;
                }
                map = new Bitmap(map, target_size);

                return new IOPair(map.GetRArray(), results[class_index]);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {

        }

        public IOPair GetRandom(ref Network network)
        {
            try
            {
                int class_index = r.Next(cat_names.Count);
                int pos = r.Next(paths[class_index].Count);

                Bitmap map = new Bitmap(paths[class_index][pos]);
                //map = new Bitmap(map, target_size);

                Bitmap result = new Bitmap(target_size.Width, target_size.Height);

                double k_size = (r.NextDouble() - 0.5) * 2 * resize;
                double xmove = (r.NextDouble() - 0.2) * 2 * rand_move;
                double ymove = (r.NextDouble() - 0.2) * 2 * rand_move;

                Graphics gr = Graphics.FromImage(result);
                gr.DrawImage(map, (float)xmove * target_size.Width, (float)ymove * target_size.Height, (float)(1 + k_size) * target_size.Width, (float)(1 + k_size) * target_size.Height);
                gr.Flush();
                gr.Dispose();

                return new IOPair(result.GetRArray(),
                    results[class_index]);

            }
            catch
            {
                return GetRandom(ref network);
            }
        }

        public bool MoveNext()
        {
            pos++;
            if (paths[class_index].Count <= pos)
            {
                pos = 0;
                class_index++;
            }

            return class_index < paths.Count;
        }

        public void Reset()
        {
            class_index = 0;
            pos = 0;
        }

        public IEnumerator<IOPair> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public double[,,] Process(Bitmap input)
        {
            throw new NotImplementedException();
        }
    }
}
