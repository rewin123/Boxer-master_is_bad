using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Recursive
{
    public class RecursiveArrData : IDataEnumerator
    {
        Random r = new Random();
        public List<RecLine> lines = new List<RecLine>();

        public IOPair Current
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

        public IEnumerator<IOPair> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IOPair GetRandom(ref Network network)
        {
            RecLine line = lines[r.Next(lines.Count)];
            int randLength = r.Next(line.inputs.Count - 1);
            network.ClearBuffers();
            for (int i = 0; i < randLength; i++)
                network.GetOutput(line.inputs[i]);

            while(line.needOutputs[randLength] == false)
            {
                network.GetOutput(line.inputs[randLength]);
                randLength++;
            }

            return new IOPair(line.inputs[randLength], line.outputs[randLength]);
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
            throw new NotImplementedException();
        }
    }
}
