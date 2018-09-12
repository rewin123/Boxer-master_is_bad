using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class ArrDataEnumerator : IDataEnumerator
    {
        List<double[,,]> data;
        List<double[,,]> output;
        Random r = new Random();
        int index = 0;

        public ArrDataEnumerator(List<double[,,]> data, List<double[,,]> output)
        {
            this.data = data;
            this.output = output;
        }

        public ArrDataEnumerator()
        {
            data = new List<double[,,]>();
            output = new List<double[,,]>();
        }

        public IOPair Current
        {
            get
            {
                return new IOPair(data[index], output[index]);
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
            int i = r.Next(data.Count);
            return new IOPair(data[i], output[i]);
        }

        public void AddSample(double[,,] data, double[,,] output)
        {
            this.data.Add(data);
            this.output.Add(output);
        }

        public bool MoveNext()
        {
            index++;
            return index < data.Count;
        }

        public void Reset()
        {
            index = 0;
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

        /// <summary>
        /// Разделяет данные на два набора
        /// </summary>
        /// <param name="k">Процент разбивания</param>
        /// <param name="train"></param>
        /// <param name="val"></param>
        public void SplitTrainVal(double k, out ArrDataEnumerator data1, out ArrDataEnumerator data2)
        {
            List<double[,,]> fake_data = new List<double[,,]>(data);
            List<double[,,]> fake_out = new List<double[,,]>(output);
            Random r = new Random();
            List<double[,,]> d1 = new List<double[,,]>();
            List<double[,,]> d2 = new List<double[,,]>();
            List<double[,,]> o1 = new List<double[,,]>();
            List<double[,,]> o2 = new List<double[,,]>();

            int pos = (int)(data.Count * k);
            for(int i = 0;i < pos;i++)
            {
                int el = r.Next(fake_data.Count);
                d1.Add(fake_data[el]);
                o1.Add(fake_out[el]);

                fake_data.RemoveAt(el);
                fake_out.RemoveAt(el);
            }

            d2.AddRange(fake_data);
            o2.AddRange(fake_out);
            //for(int i = pos;i < data.Count;i++)
            //{
            //    d2.Add(fake_data[i]);
            //    o2.Add(fake_data[i]);
            //}

            data1 = new ArrDataEnumerator(d1, o1);
            data2 = new ArrDataEnumerator(d2, o2);
        }
        

        public void ProcessChannel(int ch, int y, int x, Action<double> action)
        {
            foreach(var d in data)
            {
                action(d[ch, y, x]);
            }
        }

        public void Process2Channel(int ch1, int y1, int x1, int ch2, int y2, int x2, Action<double, double, int, int ,int> action)
        {
            foreach (var pair in this)
            {
                var d = pair.Key;
                pair.Value.ArgMax(out int c, out int y, out int x);
                action(d[ch1, y1, x1], d[ch2,y2,x2], c,y,x);
            }
        }
    }
}
