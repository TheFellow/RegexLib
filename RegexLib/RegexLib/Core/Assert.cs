using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    class Assert : MatchSingleBase
    {
        public Assert(IMatch matchItem) : base(matchItem) { }

        public override bool Match(Context context)
        {
            // To assert a token is to save the state of the context
            // and restore it afterwards. However, the behavior is
            // different for the stack and for capture groups.
            // If a token in an assertion saves state information
            // it will be removed after matching. If it captures
            // this will remain unless this assertion needs to backtrack.

            int startingOffset = context.offset;
            int stackSize = context.stackSize;
            int currentKey = context.Key;

            // Try to match the token
            bool result = matchItem.Match(context);

            // Reset the offset and remove any
            // backtracking info for the token
            context.offset = startingOffset;
            context.RestoreStack(stackSize);

            // If we've succeeded save off the largest capture key
            // so we can restore it if we need to during a MatchNext.
            // If not then remove anything captured during the assertion.
            if (result)
                context.Push(currentKey);
            else
                context.RestoreCaptures(currentKey);

            return result;
        }

        public override bool MatchNext(Context context)
        {
            // Restore the captures (removing anything we captured
            // during the assertion) and fail
            int maxKey = context.Pop();
            context.RestoreCaptures(maxKey);

            return false;
        }
    }
}
