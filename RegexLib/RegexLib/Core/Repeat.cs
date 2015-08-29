using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    class Repeat : MatchSingleBase
    {
        protected int min;

        public int Count => min;

        public Repeat(IMatch matchItem, int count)
            : base(matchItem)
        {
            min = count;
        }

        public override bool Match(Context context)
        {
            // Match from the beginning
            return MatchFrom(context, 0);
        }

        public override bool MatchNext(Context context)
        {
            // We've been back tracked into, so start trying
            // alternatives backwards from the end
            int index = min - 1;

            while (index >= 0 && !matchItem.MatchNext(context))
                index--;

            // If we've run out of options then fail
            if (index < 0)
                return false;

            // Otherwise, try matching forward from the next iteration
            return MatchFrom(context, ++index);
        }

        private bool MatchFrom(Context context, int index)
        {
            // Attempt to match until we've matched the minimum number of times
            while(index < min)
            {
                if (matchItem.Match(context))
                {
                    // The token matched, so advance the index and try again
                    index++;
                }
                else
                {
                    // The token failed to match so we'll try again from the previous token
                    index--;

                    // Attempt an alternative match, backtracking through the
                    // iterations of Match-ing
                    while (index >= 0 && !matchItem.MatchNext(context))
                        index--;

                    // If the loop exited by backtracking through all
                    // previous matches then we've failed
                    if (index < 0)
                        return false;

                    // Otherwise, try from the next one
                    index++;
                }
            }

            // We've matched all the times that were required, success.
            return true;
        }
    }
}
