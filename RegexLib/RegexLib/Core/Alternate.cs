using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    class Alternate : MatchManyBase
    {
        public Alternate(IMatch[] matchList) : base(matchList) { }

        public override bool Match(Context context)
        {
            // Try matching from the beginning
            return MatchFrom(context, 0);
        }

        public override bool MatchNext(Context context)
        {
            // We've been backtracked into, get the token which previously
            // matched and try it for an alternative match.
            int index = context.Pop();
            if(matchList[index].MatchNext(context))
            {
                // Save the state in the context object and return success
                context.Push(index);
                return true;
            }

            // The token which previously matched has no other alternatives.
            // Try from the next one
            return MatchFrom(context, ++index);
        }

        private bool MatchFrom(Context context, int index)
        {
            for(; index<Length; index++)
                if(matchList[index].Match(context))
                {
                    // Save the state in the context object and return success
                    context.Push(index);
                    return true;
                }

            // Nothing has matched, fail
            return false;
        }
    }
}
