using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    /// <summary>
    /// The Empty token is a zero-width token which will always match, but has no alternatives
    /// </summary>
    class Empty : MatchBase
    {
        public override bool Match(Context context) => true;
        public override bool MatchNext(Context context) => false;
    }
}
