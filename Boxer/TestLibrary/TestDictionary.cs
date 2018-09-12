using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary
{
    public class TestDictionary
    {
        public Dictionary<string, Func<bool>> tests = new Dictionary<string, Func<bool>>();

        public void ConsoleTestAll()
        {
            foreach(KeyValuePair<string,Func<bool>> pair in tests)
            {
                Console.Write(pair.Key + ": ");
                bool result = pair.Value();
                if(result)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("OK");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FAIL");
                }
                Console.ResetColor();
            }
        }
    }
}
