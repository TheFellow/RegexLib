using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    class Greedy : Repeat
    {
        public readonly int max;

        public Greedy(IMatch matchItem, int min, int max=int.MaxValue)
            : base(matchItem, min)
        {
            // NOTE: 'max' stores the number of optional matches
            this.max = max - min;
        }

        public override bool Match(Context context)
        {
            // If we can't match the minimum number then we've failed
            if (!base.Match(context))
                return false;

            // We matched the minimum number, so try and match the rest
            return MatchGreedily(context, 0);
        }

        public override bool MatchNext(Context context)
        {
            // Pop off how many optional matches we have made
            int index = context.Pop();

            // If we've made 0 optional matches we have to
            // backtrack into the required iterations
            if (index == 0)
            {
                // We failed to find another match at the minimum level
                // so we must fail here
                if (!base.MatchNext(context))
                    return false;

                // We actually did find a match, so try to match
                // greedily again from 0
                return MatchGreedily(context, 0);
            }

            // We've made a match optionally, so see if it has an alternative
            if (matchItem.MatchNext(context))
            {
                // If the most recent iteration of the token found an alternative
                // then we've succeeded here. Save the state and return.
                context.Push(index);
                return true;
            }

            // We've failed to match alternatively for the most revent Match
            // of our token, but these were optional, so decrement the
            // count of our matches, save the state, and exir
            context.Push(--index);
            return true;
        }

        private bool MatchGreedily(Context context, int index)
        {
            while (index < max && matchItem.Match(context))
                index++;

            // Save the number of optional matches
            // we've made in the context
            context.Push(index);

            return true;
        }
    }
}
