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
    abstract class MatchSingleBase : MatchBase
    {
        public readonly IMatch matchItem;

        public MatchSingleBase(IMatch matchItem)
            : base()
        {
            this.matchItem = matchItem;
        }
    }
}
