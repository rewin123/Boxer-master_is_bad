using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;

namespace BoxerDLL
{
    public class CutterEnumerator : IEnumerator<Bitmap>
    {
        RectangleF[] rects;
        Bitmap input;
        int index = 0;

        public CutterEnumerator(ref Bitmap input, RectangleF[] rects)
        {
            this.rects = rects;
            this.input = input;
        }
        
        Bitmap CutCurrent()
        {
            return input.Clone(rects[index], input.PixelFormat);
        }

        public Bitmap Current
        {
            get
            {
                if (index < rects.Length)
                    return CutCurrent();
                else return null;
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

        public bool MoveNext()
        {
            index++;
            return index < rects.Length;
        }

        public void Reset()
        {
            index = 0;
        }
    }
}
