using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    class Possessive : Greedy
    {
        public Possessive(IMatch matchItem, int min, int max = int.MaxValue) : base(matchItem, min, max) { }

        public override bool Match(Context context)
        {
            // Possessive matches behave like atomic greedy matching
            int stackSize = context.stackSize;
            int key = context.Key;
            int offset = context.offset;

            bool result = base.Match(context);

            // See Atomic.cs for more descriptions of this, it's the same.
            context.RestoreStack(stackSize);

            if (result)
            {
                context.Push(offset);
                context.Push(key);
            }

            return result;
        }

        public override bool MatchNext(Context context)
        {
            // See Atomic.cs for more descriptions of this, it's the same.
            int key = context.Pop();
            int offset = context.Pop();

            context.RestoreCaptures(key);
            context.offset = offset;

            return false;
        }
    }
}
