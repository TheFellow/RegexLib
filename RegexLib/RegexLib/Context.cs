using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib
{
    sealed class Context
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
        public char prev => (offset > 0 && offset <= matchString.Length) ? matchString[offset - 1] : '\0';

        #endregion

        #region Stack for storing state information

        private readonly Stack<int> stack = new Stack<int>();

        public void Push(int state) => stack.Push(state);
        public int Peek() => stack.Peek();
        public int Pop() => stack.Pop();

        #endregion

        #region Use ToString() to dump the current state of the matching context

        public override string ToString()
        {
            // Display the match string and a pointer to the current offset
            string str = $"{matchString}\n{new string('-', offset)}^\n";

            // If the stack is non-empty display it
            if(stack.Count > 0)
            {
                int[] stackState = stack.ToArray();
                Array.Reverse(stackState);
                str += "  Stack: " + string.Join(", ", stackState);
            }

            return str;
        }

        #endregion
    }
}
