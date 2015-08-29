using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;
using static RegexLib.Console.RegexTests;

namespace RegexLib.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("RegexLib.Console test app.");

            //test_empty();
            test_char();

            ReadKey(true);
        }
    }
}
