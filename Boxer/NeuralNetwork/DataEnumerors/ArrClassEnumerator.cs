using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.DataEnumerors
{
    public class ClassEnumerator : IDataEnumerator
    {
        Random r = new Random();
        IDataEnumerator[] classes;

        int class_pos = 0;

        public ClassEnumerator(IDataEnumerator[] classes)
        {
            this.classes = classes;
        }
        public IOPair Current => classes[class_pos].Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }
        
        public IEnumerator<IOPair> GetEnumerator()
        {
            return this;
        }

        public IOPair GetRandom(ref Network network)
        {
            return classes[r.Next(classes.Length)].GetRandom(ref network);
        }

        public bool MoveNext()
        {
            bool next = classes[class_pos].MoveNext();
            if (next)
                return next;
            else
            {
                class_pos++;
                if (class_pos >= classes.Length)
                {
                    return false;
                }
                else return true;
            }
        }


        public double[,,] Process(Bitmap input)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            class_pos = 0;
            for (int i = 0; i < classes.Length; i++)
                classes[i].Reset();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
