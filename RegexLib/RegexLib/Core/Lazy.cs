using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    class Lazy : Repeat
    {
        public readonly int max;

        public Lazy(IMatch matchItem, int min, int max = int.MaxValue)
            : base(matchItem, min)
        {
            // NOTE: 'max' is the number of additional optional tokens
            this.max = max - min;
        }

        public override bool Match(Context context)
        {
            // If we can't match the minimum number of times we've failed
            if (!base.Match(context))
                return false;
            
            // Matching lazily, so no more is fine.
            context.Push(0);
            return true;
        }

        public override bool MatchNext(Context context)
        {
            // Pop off how many matches we've actually made.
            int count = context.Pop();

            // If we have not reached the maximum iterations and
            // can match again, that's what we'll do.
            if (count < max && matchItem.Match(context))
            {
                // Save state and succeed
                context.Push(++count);
                return true;
            }

            // Current iteration of the token failed, try alternatives
            // backwards through the previously matched iterations
            while (count > 0)
            {
                // If we find an alternative match then save state and exit
                if (matchItem.MatchNext(context))
                {
                    context.Push(count);
                    return true;
                }

                // Alternative failed, go back to a previous iteration
                count--;
            }

            // At this point count == 0 so we have no other options
            // We have to try backtracking into the base behavior.
            // If that fails we're out of options.
            if (!base.MatchNext(context))
                return false;

            // We've successfully matched at the base level.
            // Behave lazily by saving this state and succeeding
            context.Push(0);
            return true;
        }
    }
}
