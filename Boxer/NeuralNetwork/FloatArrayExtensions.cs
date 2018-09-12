using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public static class doubleArrayExtensions
    {
        public static double[,,] CreateArray(Size size)
        {
            return new double[size[0], size[1], size[2]];
        }

        public static void ForEach(this double[,,] array, Func<double,double> func)
        {
            int depth = array.GetLength(0);
            int height = array.GetLength(1);
            int width = array.GetLength(2);

            for(int z = 0;z < depth;z++)
            {
                for(int y = 0;y < height;y++)
                {
                    for(int x = 0;x < width;x++)
                    {
                        array[z,y,x] = func(array[z,y,x]);
                    }
                }
            }
        }

        public static void ForEachParallel(this double[,,] array, Func<double, double> func)
        {
            int depth = array.GetLength(0);
            int height = array.GetLength(1);
            int width = array.GetLength(2);

            Parallel.For(0, depth, z =>
              {
                  for (int y = 0; y < height; y++)
                  {
                      for (int x = 0; x < width; x++)
                      {
                          array[z, y, x] = func(array[z, y, x]);
                      }
                  }
              });
        }

        public static void ForEach(this double[,,] array, Action<double> func)
        {
            int depth = array.GetLength(0);
            int height = array.GetLength(1);
            int width = array.GetLength(2);

            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        func(array[z, y, x]);
                    }
                }
            }
        }

        public static void ForEach(this double[,,] array, Action<double,int,int,int> func)
        {
            int depth = array.GetLength(0);
            int height = array.GetLength(1);
            int width = array.GetLength(2);

            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        func(array[z, y, x],z,y,x);
                    }
                }
            }
        }

        public static double[,,] NewEach(this double[,,] array, Func<double, double> func)
        {
            
            int depth = array.GetLength(0);
            int height = array.GetLength(1);
            int width = array.GetLength(2);

            double[,,] output = new double[depth, height, width];

            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        output[z,y,x] = func(array[z,y,x]);
                    }
                }
            }

            return output;
        }

        public static double[,,] NewEachParallel(this double[,,] array, Func<double, double> func)
        {

            int depth = array.GetLength(0);
            int height = array.GetLength(1);
            int width = array.GetLength(2);

            double[,,] output = new double[depth, height, width];

            Parallel.For(0, depth, z =>
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        output[z, y, x] = func(array[z, y, x]);
                    }
                }
            });

            return output;
        }

        /// <summary>
        /// target[x,y,z] = target[x,y,z] + array[x,y,z]
        /// </summary>
        /// <param name="target"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double[,,] Sum(this double[,,] target, double[,,] array)
        {
            int width = target.GetLength(0);
            int height = target.GetLength(1);
            int depth = target.GetLength(2);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    for (int z = 0; z < depth; z++)
                        target[x, y, z] += array[x, y, z];

            return target;
        }
        
        /// <summary>
        /// target[x,y,z] = action(target[x,y,z],array[x,y,z])
        /// </summary>
        /// <param name="target"></param>
        /// <param name="array"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static double[,,] ForEach(this double[,,] target, double[,,] array, Func<double,double,double> action)
        {
            int width = target.GetLength(0);
            int height = target.GetLength(1);
            int depth = target.GetLength(2);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    for (int z = 0; z < depth; z++)
                        target[x, y, z] = action(target[x, y, z], array[x, y, z]);

            return target;
        }

        /// <summary>
        /// result = func(target,array)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="array"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static double[,,] NewEach(this double[,,] target, double[,,] array, Func<double, double, double> action)
        {
            int width = target.GetLength(0);
            int height = target.GetLength(1);
            int depth = target.GetLength(2);

            double[,,] result = new double[width, height, depth];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    for (int z = 0; z < depth; z++)
                        result[x, y, z] = action(target[x, y, z], array[x, y, z]);

            return result;
        }

        public static double[,,] ForEach(this double[,,] target, double[,,] array, Action<double, double> action)
        {
            int width = target.GetLength(0);
            int height = target.GetLength(1);
            int depth = target.GetLength(2);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    for (int z = 0; z < depth; z++)
                         action(target[x, y, z], array[x, y, z]);

            return target;
        }

        public static double MaxNorm(this double[,,] target)
        {
            int width = target.GetLength(0);
            int height = target.GetLength(1);
            int depth = target.GetLength(2);

            double max = double.MinValue;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    for (int z = 0; z < depth; z++)
                        max = Math.Max(max, Math.Abs(target[x, y, z]));

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    for (int z = 0; z < depth; z++)
                        target[x, y, z] /= max;

            return max;
        }

        public static double MaxNorm(this double[,,] target, double upper)
        {
            int width = target.GetLength(0);
            int height = target.GetLength(1);
            int depth = target.GetLength(2);

            double max = double.MinValue;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    for (int z = 0; z < depth; z++)
                        max = Math.Max(max, Math.Abs(target[x, y, z]));

            max = max == 0 ? 1 : upper / max;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    for (int z = 0; z < depth; z++)
                        target[x, y, z] *= max;

            return max;

        }

        public static double MaxVal(this double[,,] target)
        {
            int width = target.GetLength(0);
            int height = target.GetLength(1);
            int depth = target.GetLength(2);

            double max = double.MinValue;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    for (int z = 0; z < depth; z++)
                        max = Math.Max(max, Math.Abs(target[x, y, z]));
            return max;

        }

        /// <summary>
        /// Возражаешь координаты положения максимального числа в массиве
        /// </summary>
        /// <param name="array"></param>
        /// <param name="c"></param>
        /// <param name="y"></param>
        /// <param name="x"></param>
        public static void ArgMax(this double[,,] array, out int c_res, out int y_res, out int x_res)
        {
            double max = double.MinValue;
            int max_arg_x = -1;
            int max_arg_y = -1;
            int max_arg_c = -1;
            array.ForEach((val, c, y, x) =>
            {
                if (val > max)
                {
                    max_arg_x = x;
                    max_arg_y = y;
                    max_arg_c = c;
                    max = val;
                }
            });
            c_res = max_arg_c;
            y_res = max_arg_y;
            x_res = max_arg_x;
        }
    }
}
