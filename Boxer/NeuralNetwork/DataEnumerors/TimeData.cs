using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class TimeData : IDataEnumerator
    {
        Random r = new Random();
        public List<double[,,]> time_frames = new List<double[,,]>();
        int input_count = 10;
        int time_offset = 0;
        public TimeData(int input_count, int time_offset = 0)
        {
            this.input_count = input_count;
            this.time_offset = time_offset;
        }

        public void AddTimeFrame(double[,,] frame)
        {
            time_frames.Add(frame);
        }
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
            int height = time_frames[0].GetLength(1);
            int width = time_frames[0].GetLength(2);
            int pos = r.Next(time_frames.Count - input_count - 1 - time_offset);
            pos += input_count + 1;
            double[,,] output = new double[input_count, height, width];
            for(int i = 1;i <= input_count;i++)
            {
                double[,,] val = time_frames[pos - i];
                for(int y = 0;y < height;y++)
                {
                    for(int x = 0;x < width;x++)
                    {
                        output[i - 1, y, x] = val[0, y, x];
                    }
                }
            }

            return new IOPair(output, time_frames[pos + time_offset]);
        }

        public bool MoveNext()
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

        public double[,,] Process(Bitmap input)
        {
            throw new NotImplementedException();
        }
    }
}
