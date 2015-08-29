using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RegexLib.Core;

using static System.Console;

namespace RegexLib.Console
{
    static class CoreTests
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

        public static void test_char()
        {
            var context = new Context("ab");
            var chara = new Character('a');
            var charb = new Character('b');

            ExecTest(context, chara);

            context.offset = 1;
            ExecTest(context, charb);

            chara = new Character('a', false);
            ExecTest(context, chara);

            context.offset = 2;
            charb = new Character('b', false);
            ExecTest(context, charb);
        }

        public static void test_list()
        {
            var context = new Context("abc");
            var chara = new Character('a');
            var charb = new Character('b');
            var charc = new Character('c');
            var list = new List(new IMatch[] { chara, charb, charc });

            // (abc)
            ExecTest(context, list);
        }

        public static void test_list_backward()
        {
            var context = new Context("abc");
            var chara = new Character('a', false);
            var charb = new Character('b', false);
            var charc = new Character('c', false);
            var list = new List(new IMatch[] { chara, charb, charc }, false);

            context.offset = context.Length;
            ExecTest(context, list);
        }

        public static void test_alternate()
        {
            var context = new Context("aaa");
            var chara = new Character('a');
            var list2a = new List(new IMatch[] { chara, chara });
            var list3a = new List(new IMatch[] { chara, chara, chara });
            var alt = new Alternate(new IMatch[] { chara, list2a, list3a });

            // (a|aa|aaa)
            ExecTest(context, alt);
        }

        public static void test_alternatelist()
        {
            var context = new Context("aaaa");
            var chara = new Character('a');
            var list2a = new List(new IMatch[] { chara, chara });
            var list3a = new List(new IMatch[] { chara, chara, chara });
            var alt = new Alternate(new IMatch[] { chara, list2a, list3a });
            var list = new List(new IMatch[] { alt, alt });

            // (a|aa|aaa)(a|aa|aaa)
            ExecTest(context, list);
        }

        public static void test_repeat()
        {
            var context = new Context("aaaa");
            var chara = new Character('a');
            var list2a = new List(new IMatch[] { chara, chara });
            var alt = new Alternate(new IMatch[] { chara, list2a });
            var rep = new Repeat(alt, 2);

            // (a|aa){2}
            ExecTest(context, rep);
        }

        #endregion
    }
}
