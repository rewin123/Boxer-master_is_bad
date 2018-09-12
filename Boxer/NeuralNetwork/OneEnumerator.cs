using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class OneEnumerator : IDataEnumerator
    {
        public double[,,] input;
        public double[,,] output;
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

        public IEnumerator<KeyValuePair<double[,,], double[,,]>> GetEnumerator()
        {
            return this;
        }

        public KeyValuePair<double[,,], double[,,]> GetRandom()
        {
            return new KeyValuePair<double[,,], double[,,]>(input, output);
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public double[,,] Process(Bitmap input)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
