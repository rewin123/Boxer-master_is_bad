using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using System.IO;

namespace BoxerDLL
{
    public class BitmapEnumerator : IEnumerator<Bitmap>
    {
        string[] paths;
        int index = 0;

        public BitmapEnumerator(string directory)
        {
            paths = Directory.GetFiles(directory);
        }

        public BitmapEnumerator(string[] paths)
        {
            this.paths = paths;
        }

        public Bitmap Current
        {
            get
            {
                Bitmap map = new Bitmap(paths[index]);
                Bitmap result = new Bitmap(map);
                map.Dispose();
                return result;
            }
        }

        public string SafeName
        {
            get
            {
                return paths[index].Split('\\').Last();
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
            do
            {
                index++;
                if (index > paths.Length)
                    return false;
            } while (paths[index].Split('.').Last() != "png");
            return true;
        }

        public void Reset()
        {
            index = 0;
        }
    }
}
