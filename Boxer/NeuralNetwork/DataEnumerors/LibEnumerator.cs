using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class LibEnumerator : IDataEnumerator
    {
        List<List<string>> paths = new List<List<string>>();
        List<string> cat_names = new List<string>();
        List<double[,,]> results = new List<double[,,]>();

        System.Drawing.Size target_size;
        int pos = 0;
        int class_index = 0;
        Random r = new Random();
        
        public LibEnumerator(string root_dir, System.Drawing.Size target_size)
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
                return new IOPair(null, null);
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
            lock (lock_obj)
            {
                
                    int class_index = r.Next(cat_names.Count);
                    int pos = r.Next(paths[class_index].Count);

                    CELLDATA data = new CELLDATA();
                    GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

                    string path = paths[class_index][pos];

                    //IntPtr ptr_data = Marshal.AllocHGlobal(Marshal.SizeOf(data));
                    //Marshal.StructureToPtr(data, ptr_data, true);

                    //IntPtr ptr_string = Marshal.StringToBSTR(path);

                    int val = evalThisCellFileInput(path, out data, 0);

                    return new IOPair(null,null);
                    //return new IOPair(result, results[class_index]);
                
            }
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

        [DllImport("\\\\Mac\\Home\\Documents\\GitHub\\Boxer\\Boxer\\NetworkLab\\bin\\Debug\\liba\\evaluate.dll",
            EntryPoint = "evalThisCellFileInput", CallingConvention = CallingConvention.Cdecl)]
        static extern int evalThisCellFileInput(string fnameIn, out CELLDATA cf_out, int debugFlag);

        public double[,,] Process(Bitmap input)
        {
            throw new NotImplementedException();
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CELLDATA
        {
            public int tag;
            public RECT R;
            public double average;
            public double area;
            public double area1;
            public int ngrain;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int v1;
            public int v2;
            public int v3;
            public int v4;
        }
    }
}
