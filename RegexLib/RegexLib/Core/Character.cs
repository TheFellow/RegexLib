using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    class Character : MatchBase
    {
        /// <summary>
        /// The character we're trying to match
        /// </summary>
        public readonly char matchChar;

        /// <summary>
        /// The direction to move in upon matching
        /// </summary>
        public int step => forward ? 1 : -1;

        /// <summary>
        /// Flag indicating that we're matching forwards
        /// </summary>
        public readonly bool forward;

        public Character(char matchChar, bool forward = true)
        {
            this.matchChar = matchChar;
            this.forward = forward;
        }

        public override bool Match(Context context)
        {
            // We have no chance of matching if we're not on a valid index
            if (forward)
            {
                if (context.offset < 0 || context.offset >= context.Length) return false;
            }
            else
            {
                if (context.offset < 1 || context.offset > context.Length) return false;
            }

            // Check if we can match this character
            bool result = matchChar == (forward ? context.curr : context.prev);

            // If we've matched then advance the offset in the match string
            if (result)
                context.offset += step;

            return result;
        }

        public override bool MatchNext(Context context)
        {
            // There are no alternatives we can try, so backtrack our offset and fail
            context.offset -= step;
            return false;
        }
    }
}
