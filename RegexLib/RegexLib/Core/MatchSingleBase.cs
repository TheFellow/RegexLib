using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    /// <summary>
    /// Abstract base class for all token types which operate on a single token
    /// </summary>
    abstract class MatchSingleBase : IMatch
    {
        public readonly IMatch matchItem;

        public MatchSingleBase(IMatch matchItem)
        {
            this.matchItem = matchItem;
        }

        public abstract bool Match(Context context);
        public abstract bool MatchNext(Context context);
    }
}
