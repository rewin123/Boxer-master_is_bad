using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NeuralNetwork
{
    public class Vector3
    {
        public double alpha;
        public double x; //r
        public double y; //g
        public double z; //b

        public Vector3(double x = 0, double y = 0, double z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3 operator *(Vector3 v1, double val)
        {
            return new Vector3(v1.x * val, v1.y * val, v1.z * val);
        }

        public static Vector3 operator /(Vector3 v1, double val)
        {
            return new Vector3(v1.x / val, v1.y / val, v1.z / val);
        }

        public void Add(Vector3 v)
        {
            x += v.x;
            y += v.y;
            z += v.z;
        }

        public void Retract(Vector3 v)
        {
            x -= v.x;
            y -= v.y;
            z -= v.z;
        }

        public void Multiply(double val)
        {
            x *= val;
            y *= val;
            z *= val;
        }

        public void Divide(double val)
        {
            x /= val;
            y /= val;
            z /= val;
        }

        public Color GetColor()
        {
            return Color.FromArgb((int)(x * 255), (int)(y * 255), (int)(z * 255));
        }

        public SolidBrush GetBrush()
        {
            return new SolidBrush(GetColor());
        }

        public double Magnitude()
        {
            return x * x + y * y + z * z;
        }

        public override string ToString()
        {
            return x + " " + y + " " + z;
        }

        public void Normilize()
        {
            double amp = (double)Math.Sqrt(Magnitude());
            if (amp != 0)
            {
                x /= amp;
                y /= amp;
                z /= amp;
            }
        }

        public static Vector3[,] GetArray(double[,] array)
        {
            int width = array.GetLength(0);
            int height = array.GetLength(1);
            Vector3[,] map = new Vector3[width, height];
            double val;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    val = array[x, y];
                    map[x, y] = new Vector3(val, val, val);
                }
            }
            return map;
        }

        public static Vector3[] GetArray(double[] array)
        {
            int width = array.GetLength(0);
            Vector3[] map = new Vector3[width];
            double val;
            for (int x = 0; x < width; x++)
            {
                val = array[x];
                map[x] = new Vector3(val, val, val);
            }
            return map;
        }
    }
}
