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
            var token = new CaptureGroup(testToken);

            if(token.Match(context))
            {
                WriteInfo("Match success.");
                WriteLine(context);

                while (token.MatchNext(context))
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

            if (context.offset != 0)
                WriteError("Warning: Context offset not reset to 0.");
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

        public static void test_greedy()
        {
            var context = new Context("aaaaa");
            var chara = new Character('a');
            var grd = new Greedy(chara, 2, 4);

            // (a{1,3})
            ExecTest(context, grd);
        }

        public static void test_lazy()
        {
            var context = new Context("aaaaa");
            var chara = new Character('a');
            var lazy = new Lazy(chara, 2, 4);

            // (a{2,4}?)
            ExecTest(context, lazy);
        }

        public static void test_greedyalternate()
        {
            var context = new Context("aaaaa");
            var chara = new Character('a');
            var list2a = new List(new IMatch[] { chara, chara });
            var alt = new Alternate(new IMatch[] { chara, list2a });
            var greedy = new Greedy(alt, 1, 2);

            // (a|aa){1,2}
            ExecTest(context, greedy);
        }

        public static void test_lazyalternate()
        {
            var context = new Context("aaaaa");
            var chara = new Character('a');
            var list2a = new List(new IMatch[] { chara, chara });
            var alt = new Alternate(new IMatch[] { chara, list2a });
            var lazy = new Lazy(alt, 1, 2);

            // (a|aa){1,2}?
            ExecTest(context, lazy);
        }

        public static void test_greedyaltcapture()
        {
            var context = new Context("aaaaa");
            var chara = new Character('a');
            var list2a = new List(new IMatch[] { chara, chara });

            var grp1 = new CaptureGroup(chara, 1);
            var grp2 = new CaptureGroup(list2a, 2);
            var alt = new Alternate(new IMatch[] { grp1, grp2 });
            var greedy = new Greedy(alt, 1, 2);

            // ((a)|(aa)){1,2}
            ExecTest(context, greedy);
        }

        public static void test_positive_lookahead()
        {
            var context = new Context("aaaa");
            var chara = new Character('a');
            var greedy = new Greedy(chara, 1);
            var list2a = new List(new IMatch[] { chara, chara });
            var assert = new Lookaround(new CaptureGroup(list2a, 1));
            var list = new List(new IMatch[] { greedy, assert });

            // (a+(?=(aa))
            ExecTest(context, list);
        }

        public static void test_negative_lookahead()
        {
            var context = new Context("aaaab");
            var chara = new Character('a');
            var greedy = new Greedy(chara, 1);
            var list2a = new List(new IMatch[] { chara, chara });
            var assert = new Lookaround(new CaptureGroup(list2a, 1), false);
            var list = new List(new IMatch[] { greedy, assert });

            // (a+(?!(aa))
            ExecTest(context, list);
        }

        public static void test_positive_lookbehind()
        {
            var context = new Context("abaab");
            var chara = new Character('a');
            var charb = new Character('b');
            var alt = new Alternate(new IMatch[] { chara, charb });
            var lazy = new Lazy(alt, 1);

            var backa = new Character('a', false);
            var back2a = new List(new IMatch[] { backa, backa }, false);
            var assert = new Lookaround(back2a);
            var list = new List(new IMatch[] { lazy, assert });

            // ((?:a|b)+?(?<=aa))
            ExecTest(context, list);
        }

        public static void test_negative_lookbehind()
        {
            var context = new Context("abaab");
            var chara = new Character('a');
            var charb = new Character('b');
            var alt = new Alternate(new IMatch[] { chara, charb });
            var lazy = new Lazy(alt, 1);

            var backa = new Character('a', false);
            var back2a = new List(new IMatch[] { backa, backa }, false);
            var assert = new Lookaround(back2a, false);
            var list = new List(new IMatch[] { lazy, assert });

            // ((?:a|b)+?(?<!aa))
            ExecTest(context, list);
        }

        public static void test_atomic()
        {
            var context = new Context("aaaab");
            var chara = new Character('a');
            var charb = new Character('b');
            var greedya = new Greedy(chara, 2);
            var atomic = new Atomic(greedya);
            var alt = new Alternate(new IMatch[] { chara, charb });
            var list = new List(new IMatch[] { new CaptureGroup(atomic, 1), alt });

            // (((?>a{2,}))(?:a|b))
            ExecTest(context, list);
        }

        public static void test_possessive()
        {
            var context = new Context("aaaab");
            var chara = new Character('a');
            var charb = new Character('b');
            var alt = new CaptureGroup(new Alternate(new IMatch[] { chara, charb }), 2);
            var posa = new Possessive(alt, 2);
            var list = new List(new IMatch[] { new CaptureGroup(posa, 1), alt });

            // (((a|b){2,}+)(a|b))
            ExecTest(context, list);
        }

        public static void test_backref()
        {
            var context = new Context("aaaaab");
            var chara = new Character('a');
            var lazya = new Lazy(chara, 1);
            var cap1 = new CaptureGroup(lazya, 1);
            var back1 = new Backreference(cap1);
            var list = new List(new IMatch[] { cap1, back1 });

            // ((a+?)\1)
            ExecTest(context, list);
        }

        public static void test_charset()
        {
            var context = new Context("a1b2c3d4e5f6");
            var cset = new Charset();
            cset.Include('a');
            cset.Include('b', 'e');
            cset.Include('0', '9');

            var pos = new Possessive(cset, 0);

            // ([ab-e0-9]*)
            ExecTest(context, pos);
        }

        public static void test_nested_charset()
        {
            var cset3 = new Charset();
            cset3.Include('c');

            var cset2 = new Charset();
            cset2.Include('b', 'd');
            cset2.Exclude(cset3);

            var cset1 = new Charset();
            cset1.Include('a', 'e');
            cset1.Exclude(cset2);

            // ([a-e-[b-d-[c]]])
            ExecTest(new Context("a"), cset1);  // Yes
            ExecTest(new Context("b"), cset1);  // No
            ExecTest(new Context("c"), cset1);  // Yes
            ExecTest(new Context("d"), cset1);  // No
            ExecTest(new Context("e"), cset1);  // Yes
        }

        public static void test_negated_nested_charset()
        {
            var cset3 = new Charset();
            cset3.Include('c');

            var cset2 = new Charset();
            cset2.Include('b', 'd');
            cset2.Exclude(cset3);

            var cset1 = new Charset(false);
            cset1.Include('a', 'e');
            cset1.Exclude(cset2);

            // ([^a-e-[b-d-[c]]])
            ExecTest(new Context("a"), cset1);  // No
            ExecTest(new Context("b"), cset1);  // Yes
            ExecTest(new Context("c"), cset1);  // No
            ExecTest(new Context("d"), cset1);  // Yes
            ExecTest(new Context("e"), cset1);  // No
        }

        #endregion
    }
}
