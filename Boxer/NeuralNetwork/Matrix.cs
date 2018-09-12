using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Matrix
    {
        public double[] array;
        int[] sizes; //width, height, depth, etc...
        int[] cum_sizes;

        public Matrix(params int[] sizes)
        {
            this.sizes = sizes;

            SetupCumSizes();
            array = new double[cum_sizes.Last() * sizes.Last()];

        }

        public Matrix(Matrix m)
        {
            sizes = (int[])m.sizes.Clone();
            SetupCumSizes();
            array = (double[])m.array.Clone();
        }

        void SetupCumSizes()
        {
            cum_sizes[0] = 1;
            for (int i = 1; i < sizes.Length; i++)
            {
                cum_sizes[i] = sizes[i - 1] * cum_sizes[i - 1];
            }
        }

        int GetPos(int[] coordinates)
        {
            int pos = 0;
            for(int i = 0;i < coordinates.Length;i++)
            {
                pos += coordinates[i] * cum_sizes[i];
            }
            return pos;
        }

        public double this[params int[] coordinates]
        {
            get
            {
                return array[GetPos(coordinates)];
            }
            set
            {
                array[GetPos(coordinates)] = value;
            }
        }

        public static Matrix Resize(Matrix matrix, params int[] new_sizes)
        {
            Matrix new_m = new Matrix(new_sizes);

            int[] sizes = matrix.Sizes;
            int[] coordinates = new int[sizes.Length];
            while(coordinates[sizes.Length - 1] < sizes[sizes.Length - 1])
            {
                new_m[coordinates] = matrix[coordinates];
                coordinates[0]++;
                for(int i = 1;i < coordinates.Length;i++)
                {
                    if (coordinates[i - 1] == sizes[i - 1])
                    {
                        coordinates[i]++;
                        coordinates[i - 1] = 0;
                    }
                }
            }

            return new_m;
        }

        public int[] Sizes
        {
            get
            {
                return (int[])sizes.Clone();
            }
        }

        public int Length
        {
            get
            {
                return array.Length;
            }
        }

        

        static bool EqualSize(int[] sizes1, int[] sizes2)
        {
            if (sizes1.Length != sizes2.Length)
                return false;

            int len = sizes2.Length;
            for(int i = 0;i < len;i++)
            {
                if (sizes1[i] != sizes2[i])
                    return false;
            }
            return true;
        }

        public Matrix FastAdd(ref Matrix m)
        {
            if (!EqualSize(sizes, m.sizes))
                throw new Exception("Not equal size");

            int len = array.Length;

            double[] arr2 = m.array;

            for (int i = 0; i < len; i++)
                array[i] += arr2[i];

            return this;
        }

        public Matrix FastRetract(ref Matrix m)
        {
            if (!EqualSize(sizes, m.sizes))
                throw new Exception("Not equal size");

            int len = array.Length;

            double[] arr2 = m.array;

            for (int i = 0; i < len; i++)
                array[i] -= arr2[i];

            return this;
        }

        public Matrix FastDivide(double val)
        {
            int len = array.Length;

            for (int i = 0; i < len; i++)
                array[i] /= val;

            return this;
        }

        public Matrix FastMultiply(double val)
        {
            int len = array.Length;

            for (int i = 0; i < len; i++)
                array[i] *= val;

            return this;
        }

        public static Matrix operator+(Matrix m1, Matrix m2)
        {
            return new Matrix(m1).FastAdd(ref m2);
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            return new Matrix(m1).FastRetract(ref m2);
        }

        public static Matrix operator *(Matrix m, double val)
        {
            return new Matrix(m).FastMultiply(val);
        }

        public static Matrix operator /(Matrix m, double val)
        {
            return new Matrix(m).FastDivide(val);
        }

        public void ExecForEach(Func<double, double> func)
        {
            int len = array.Length;
            for (int i = 0; i < len; i++)
                array[i] = func(array[i]);
        }

        public void ExecForEach(Action<double> func)
        {
            int len = array.Length;
            for (int i = 0; i < len; i++)
                func(array[i]);
        }

        public double Sum()
        {
            int len = array.Length;
            double sum = 0;
            for (int i = 0; i < len; i++)
                sum += array[i];

            return sum;
        }

        public double Medium()
        {
            return Sum() / array.Length;
        }

        public double Dispertion()
        {
            double medium = Medium();

            double medium2 = 0;
            ExecForEach(x => { medium2 += x * x; });
            return medium2 / array.Length - medium;
            
        }
    }
}
