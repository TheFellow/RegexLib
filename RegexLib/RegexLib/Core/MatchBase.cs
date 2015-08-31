using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    /// <summary>
    /// Abstract base class for all tokens
    /// </summary>
    abstract class MatchBase : IMatch
    {
        public abstract bool Match(Context context);
        public abstract bool MatchNext(Context context);
    }
}
