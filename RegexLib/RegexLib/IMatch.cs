using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib
{
    /// <summary>
    /// The interface for a regular expression token
    /// </summary>
    interface IMatch
    {
        /// <summary>
        /// This method is the first called in the backtracking algorithm.
        /// </summary>
        bool Match(Context context);

        /// <summary>
        /// This method is called during backtracking, and should never be called unless a successful Match has already happened.
        /// </summary>
        bool MatchNext(Context context);
    }
}
