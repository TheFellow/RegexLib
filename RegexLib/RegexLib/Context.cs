using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib
{
    class Context
    {
        /// <summary>
        /// The string we're attempting to match
        /// </summary>
        public readonly string matchString;

        /// <summary>
        /// The current offset in the string
        /// </summary>
        public int offset;

        public Context(string matchString)
        {
            this.matchString = matchString;
        }

        #region Match string helper properties

        public int Length => matchString.Length;
        public char curr => (offset >= 0 && offset < matchString.Length) ? matchString[offset] : '\0';

        #endregion
    }
}
