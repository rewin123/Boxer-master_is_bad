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

        public KeyValuePair<double[,,], double[,,]> Current
        {
            get
            {
                throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public KeyValuePair<double[,,], double[,,]> GetRandom()
        {
            int i = r.Next(data.Count);
            return new KeyValuePair<double[,,], double[,,]>(data[i], output[i]);
        }

        public void AddSample(double[,,] data, double[,,] output)
        {
            this.data.Add(data);
            this.output.Add(output);
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<double[,,], double[,,]>> GetEnumerator()
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
