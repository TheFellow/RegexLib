using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    /// <summary>
    /// The Empty token is a zero-width token which will always match, but has not alternatives
    /// </summary>
    class Empty : IMatch
    {
        public bool Match(Context context) => true;
        public bool MatchNext(Context context) => false;
    }
}
