using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork;
using NeuralNetwork.Multichannel;
using NeuralNetwork.DataEnumerors;
using NeuralNetwork.Recursive;
using NeuralNetwork.Multichannel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace TextPrediction
{
    class Program
    {
        static void Main(string[] args)
        {
            var words = GetWords(File.ReadAllText("data.txt"));
        }

        static void CharToDoubles(char ch, double[,,] array)
        {
            int val = ch;
            int size = 16;
            for(int i = 0;i < size;i++)
            {
                array[0,0,i] = val % 2;
                val /= 2;
            }
        }

        static char DoubleToChar(double[,,] array)
        {
            int val = 0;
            int pow = 1;
            int size = 16;
            for(int i = 0;i < size;i++)
            {
                val += (int)(array[0,0,i] * pow);
                pow *= 2;
            }

            return (char)val;
        }

        static List<string> GetWords(string path)
        {
            List<string> list = new List<string>();
            using (StreamReader streamReader = new StreamReader(path))
            {
                while(!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();
                    string[] loc_arr = line.Split(' ');

                    for (int i = 0; i < loc_arr.Length; i++)
                        if (loc_arr[i].Length != 0)
                            list.Add(loc_arr[i]);
                }
            }

            return list;
        }

        static string GetDialog(string line)
        {
            if (line[0] == '-' || line[0] == '—' || line[0] == '–')
            {
                string data = "";
                for (int i = 1; i < line.Length; i++)
                {
                    char ch = line[i];
                    if (ch == '–' || ch == '-' || ch == '—')
                        if (line[i - 1] == ' ')
                            break;

                    data += ch;
                }
                //data = data.Replace('.', ' ');
                data = data.Trim(' ');
                data = data.Trim(',');
                data = data.Trim(':');
                return data;
            }
            return "";
        }

        static List<string> GetDictionary(string path)
        {
            Regex regex = new Regex(@"([йцукенгшщзхъфывапролджэячсмитьбюЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ-]+)");
            List<string> list = new List<string>();
            StreamReader streamReader = new StreamReader(path);
            while(!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();

                var words = regex.Matches(line);
                for(int i = 0;i < words.Count;i++)
                {
                    var data = words[i].Value.ToLower();
                    if(list.FindIndex((str) => data == str) == -1)
                    {
                        list.Add(data);
                        //Console.WriteLine(data);
                    }
                }

            }

            return list;
        }

       

        
    }
}
