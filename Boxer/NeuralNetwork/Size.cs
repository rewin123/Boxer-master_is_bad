using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    [Serializable]
    public class Size
    {
        public int[] sizes;

        public Size(params int[] sizes)
        {
            this.sizes = sizes;
        }

        public int this[int i]
        {
            get
            {
                return sizes[i];
            }
            set
            {
                sizes[i] = value;
            }
        }

        public int Length
        {
            get
            {
                int val = 1;
                for(int i = 0;i < sizes.Length;i++)
                {
                    val *= sizes[i];
                }
                return val;
            }
        }

        public Size CloneMe()
        {
            Size s = new Size();
            s.sizes = new int[sizes.Length];
            for(int i = 0;i < sizes.Length;i++)
            {
                s.sizes[i] = sizes[i];
            }
            return s;
        }

        public override string ToString()
        {
            string s = "";
            for(int i = 0;i < sizes.Length;i++)
            {
                s += sizes[i] + " ";
            }
            return s;
        }
    }
}
