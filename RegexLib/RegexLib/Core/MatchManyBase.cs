using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    /// <summary>
    /// Abstract base class for all token types which operate on multiple tokens
    /// </summary>
    abstract class MatchManyBase : IMatch
    {
        /// <summary>
        /// List of tokens
        /// </summary>
        public readonly IMatch[] matchList;

        public MatchManyBase(IMatch[] matchList)
        {
            this.matchList = matchList;
        }

        /// <summary>
        /// Length of the list of tokens
        /// </summary>
        public int Length => matchList.Length;

        public abstract bool Match(Context context);
        public abstract bool MatchNext(Context context);
    }
}
