using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    class Atomic : MatchSingleBase
    {
        public Atomic(IMatch matchItem) : base(matchItem) { }

        public override bool Match(Context context)
        {
            // Get context state information
            int stackSize = context.stackSize;
            int key = context.Key;
            int offset = context.offset;

            // Try to match the token
            bool result = matchItem.Match(context);

            // Atomic matches throw away all backtracking information
            context.RestoreStack(stackSize);

            if (result)
            {
                // If we matched we need to store the capture key
                // and the starting offset to restor to if MatchNext
                // is called.
                context.Push(offset);
                context.Push(key);
            }

            return result;
        }

        public override bool MatchNext(Context context)
        {
            // Pull the starting state back off the stack and restore
            // the state of the matching context
            int key = context.Pop();
            int offset = context.Pop();

            context.RestoreCaptures(key);
            context.offset = offset;

            return false;
        }
    }
}
