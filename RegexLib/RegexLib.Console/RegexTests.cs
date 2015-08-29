using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RegexLib.Core;

using static System.Console;

namespace RegexLib.Console
{
    static class RegexTests
    {
        static void WriteError(string errorString)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine(errorString);
            ResetColor();
        }
        static void WriteInfo(string infoString)
        {
            ForegroundColor = ConsoleColor.Cyan;
            WriteLine(infoString);
            ResetColor();
        }

        static void ExecTest(Context context, IMatch testToken)
        {
            if(testToken.Match(context))
            {
                WriteInfo("Match success.");
                WriteLine(context);

                while (testToken.MatchNext(context))
                {
                    WriteInfo("MatchNext success.");
                    WriteLine(context);
                }
                WriteError("MatchNext failed.");
                WriteLine(context);
            }
            else
            {
                WriteError("Match failed.");
                WriteLine(context);
            }
        }

        #region Test Methods

        public static void test_empty()
        {
            var context = new Context("abc");
            var empty = new Empty();

            ExecTest(context, empty);
        }

        #endregion
    }
}
