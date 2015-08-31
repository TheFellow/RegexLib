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
    abstract class MatchManyBase : MatchBase
    {
        /// <summary>
        /// List of tokens
        /// </summary>
        public readonly IMatch[] matchList;

        public MatchManyBase(IMatch[] matchList)
            : base()
        {
            this.matchList = matchList;
        }

        /// <summary>
        /// Length of the list of tokens
        /// </summary>
        public int Length => matchList.Length;
    }
}
