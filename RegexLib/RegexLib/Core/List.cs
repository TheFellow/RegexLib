using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    class List : MatchManyBase
    {
        /// <summary>
        /// The direction to move in upon matching
        /// </summary>
        public int step => forward ? 1 : -1;

        /// <summary>
        /// Flag indicating that we're matching forwards
        /// </summary>
        public readonly bool forward;

        public List(IMatch[] matchList, bool forward = true)
            : base(matchList)
        {
            this.forward = forward;
        }

        public override bool Match(Context context)
        {
            return MatchFrom(context, forward ? 0 : Length - 1);
        }

        public override bool MatchNext(Context context)
        {
            // If this is called then the entire list has already matched
            // so we must start at the last index and backtrack.
            int index = forward ? Length - 1 : 0;
            int bol = forward ? -1 : Length;

            while (index != bol && !matchList[index].MatchNext(context))
                index -= step;

            // If nothing matched then we have failed
            if (index == bol)
                return false;

            // Otherwise, continue trying to match the rest from the next token
            return MatchFrom(context, index + step);
        }

        private bool MatchFrom(Context context, int index)
        {
            int eol = forward ? Length : -1; // (End of line) When we reach this index we've succeeded
            int bol = forward ? -1 : Length; // (Beg of line) If we reach this index we've failed

            while (index != eol)
            {
                // Try to match the token
                if(matchList[index].Match(context))
                {
                    // We matched, advance to the next token
                    index += step;
                }
                else
                {
                    // Go back to the last token that matched
                    index -= step;

                    // Backtrack through the list trying alternatives
                    while (index != bol && !matchList[index].MatchNext(context))
                        index -= step;

                    // If we fell off of the beginning then we have no alternatives, fail
                    if (index == bol)
                        return false;

                    // A token successfully MatchNext-ed, so try matching
                    // again from the next token in the list
                    index += step;
                }
            }

            // We've exited the loop with index == eol, so we've matched
            // every token in order. Success.
            return true;
        }
    }
}
