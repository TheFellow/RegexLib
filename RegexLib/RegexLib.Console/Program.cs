using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;
using static RegexLib.Console.CoreTests;

namespace RegexLib.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("RegexLib.Console test app.");

            //test_empty();
            //test_char();
            //test_list();
            //test_list_backward();
            //test_alternate();
            //test_alternatelist();
            //test_repeat();
            //test_greedy();
            test_lazy();

            ReadKey(true);
        }
    }
}
