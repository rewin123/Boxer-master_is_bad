using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace NeuralNetwork
{
    public class ProcessCatEnumerator : IDataEnumerator
    {
        List<List<string>> paths = new List<List<string>>();
        List<string> cat_names = new List<string>();
        List<double[,,]> results = new List<double[,,]>();
        
        System.Drawing.Size target_size;
        int pos = 0;
        int class_index = 0;
        string process_name;
        int param_count;
        Random r = new Random();

        ArrDataEnumerator arrays = new ArrDataEnumerator();

        public ProcessCatEnumerator(string root_dir, string process_name, int param_count)
        {
            this.process_name = process_name;
            this.param_count = param_count;
            cat_names.AddRange(Directory.GetDirectories(root_dir));
            Console.WriteLine("Finded {0} classes", cat_names.Count);
            for (int i = 0; i < cat_names.Count; i++)
            {
                List<string> p_class = new List<string>();
                p_class.AddRange(Directory.GetFiles(cat_names[i]));
                p_class.RemoveAt(p_class.FindIndex((name) => name.Split('.').Last() == "db"));
                paths.Add(p_class);
                Console.WriteLine("{0}: {1}", cat_names[i].Split('\\').Last(), p_class.Count);
                double[,,] output = new double[1, 1, cat_names.Count];
                output[0, 0, i] = 1;
                results.Add(output);
            }

            if (File.Exists("Output_proc.txt"))
                File.Delete("Output_proc.txt");

            for (int class_index = 0; class_index < cat_names.Count; class_index++)
            {
                for(int pos = 0;pos < paths[class_index].Count;pos++)
                {

                    Process proc = new Process();
                    proc.StartInfo.FileName = process_name;
                    proc.StartInfo.Arguments = "\"" + paths[class_index][pos] + "\" \"Output_proc.txt\"";
                    proc.StartInfo.UseShellExecute = false;

                    bool bool_key = proc.Start();
                    proc.WaitForExit();
                    
                }
            }

            var read = new StreamReader("Output_proc.txt");
            for (int class_index = 0; class_index < cat_names.Count; class_index++)
            {
                for (int pos = 0; pos < paths[class_index].Count; pos++)
                {
                    read.ReadLine();
                    double[,,] result = new double[1, 1, param_count];
                    for (int i = 0; i < param_count; i++)
                    {
                        string data = read.ReadLine().Replace('.', ',');
                        if (data == null)
                        {
                            i--;
                            continue;
                        }
                        result[0, 0, i] = double.Parse(data);
                    }

                    read.ReadLine();

                    arrays.AddSample(result, results[class_index]);
                }
            }

            read.Close();
        }

        public IOPair Current
        {
            get
            {
                return arrays.Current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {

        }

        public IEnumerator<IOPair> GetEnumerator()
        {
            return this;
        }

        object lock_obj = new object();

        public IOPair GetRandom(ref Network network)
        {
            return arrays.GetRandom(ref network);
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            pos = 0;
            class_index = 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public double[,,] Process(Bitmap input)
        {
            if (File.Exists("Test_proc.txt"))
                File.Delete("Test_proc.txt");
            input.Save("proc_data.png");

            Process proc = new Process();
            proc.StartInfo.FileName = process_name;
            proc.StartInfo.Arguments = "\"" + paths[class_index][pos] + "\" \"Test_proc.txt\"";
            proc.StartInfo.UseShellExecute = false;

            bool bool_key = proc.Start();
            proc.WaitForExit();

            var read = new StreamReader("Test_proc.txt");

            read.ReadLine();
            double[,,] result = new double[1, 1, param_count];
            for (int i = 0; i < param_count; i++)
            {
                string data = read.ReadLine().Replace('.', ',');
                if (data == null)
                {
                    i--;
                    continue;
                }
                result[0, 0, i] = double.Parse(data);
            }

            read.ReadLine();
            read.Close();

            return result;
        }
    }
}
