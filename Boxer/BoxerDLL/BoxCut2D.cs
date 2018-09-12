using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BoxerDLL
{
    public class BoxCut2D
    {
        int countX;
        int countY;
        float width;
        float height;
        float offsetX;
        float offsetY;
        float stepX;
        float stepY;

        public BoxCut2D(int countX, int countY, float width, float height, float offsetX = 0, float offsetY = 0, float stepX = 0, float stepY = 0)
        {
            this.countX = countX;
            this.countY = countY;
            this.width = width;
            this.height = height;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.stepX = stepX;
            this.stepY = stepY;
        }

        public RectangleF[] Rects1D(int imgWidth, int imgHeight)
        {
            List<RectangleF> rects = new List<RectangleF>();
            float loc_width = imgWidth * width;
            float loc_height = imgHeight * height; 
            for(int x = 0;x < countX;x++)
            {
                float x0 = (offsetX + x * (width + stepX)) * imgWidth;
                for (int y = 0; y < countY;y++)
                {
                    float y0 = (offsetY + y * (height + stepY)) * imgHeight;
                    rects.Add(new RectangleF(x0, y0,
                        x0 + loc_width < imgWidth ? loc_width : (imgWidth - 1 - x0),
                        y0 + loc_height < imgHeight ? loc_height : (imgHeight - 1 - y0)));
                    
                }
            }

            return rects.ToArray();
        }

        public RectangleF[,] Rects2D(int imgWidth, int imgHeight)
        {
            RectangleF[,] rects = new RectangleF[countX, countY];
            float loc_width = imgWidth * width;
            float loc_height = imgHeight * height;
            for (int x = 0; x < countX; x++)
            {
                float x0 = offsetX + x * (width + stepX);
                for (int y = 0; y < countY; y++)
                {
                    float y0 = offsetY + y * (height + stepY);
                    rects[x,y] = new RectangleF(x0, y0, loc_width, loc_height);
                    
                }
            }

            return rects;
        }

        public List<Bitmap> Cut(Bitmap input)
        {
            RectangleF[] rects = Rects1D(input.Width, input.Height);
            List<Bitmap> bitmaps = new List<Bitmap>();
            for(int i = 0;i < rects.Length;i++)
            {
                bitmaps.Add(input.Clone(rects[i], input.PixelFormat));
            }
            return bitmaps;
        }

        public CutterEnumerator Enumerator(ref Bitmap input)
        {
            RectangleF[] rects = Rects1D(input.Width, input.Height);
            return new CutterEnumerator(ref input, rects);
        }

        public Bitmap DrawRectangles(Bitmap input, Pen pen)
        {
            Bitmap drawen = new Bitmap(input);
            RectangleF[] rects = Rects1D(input.Width, input.Height);
            Graphics gr = Graphics.FromImage(drawen);
            gr.DrawRectangles(pen, rects);
            return drawen;
        }
    }
}
