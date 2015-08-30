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

        public int stackSize => stack.Count;

        public void RestoreStack(int length)
        {
            while (stack.Count > 0 && stack.Count > length)
                stack.Pop();
        }

        #endregion

        #region Sorted dictionary of capture info stacks

        private readonly SortedDictionary<int, Stack<CaptureInfo>> capture = new SortedDictionary<int, Stack<CaptureInfo>>();

        // This is used to "check out" keys for capture groups
        // Since this is always increasing it will be easy to
        // restore the state of the context by tracking the
        // largest key
        private int key = 0;
        public int Key => key; // Used to save the state

        public int Push(int beg, int end, int groupId)
        {
            // Ensure there is a stack present for the groupId
            if (!capture.ContainsKey(groupId))
                capture[groupId] = new Stack<CaptureInfo>();

            // Create the capture info struct and add it to the stack
            var info = new CaptureInfo(beg, end, groupId, ++key);
            capture[groupId].Push(info);

            // Return the new key as a convenience
            return info.key;
        }

        public CaptureInfo Peek(int groupId) => capture[groupId].Peek();
        public CaptureInfo Pop(int groupId) => capture[groupId].Pop();

        public bool GroupHasValue(int groupId) => capture.ContainsKey(groupId) && capture[groupId].Count > 0;
        public string GroupValue(int groupId)
        {
            if (!GroupHasValue(groupId))
                return string.Empty;

            var ci = Peek(groupId);
            return matchString.Substring(ci.low, ci.high - ci.low);
        }

        public void RestoreCaptures(int maxKey)
        {
            foreach(int groupId in capture.Keys)
            {
                while (GroupHasValue(groupId) && Peek(groupId).key > maxKey)
                    Pop(groupId);
            }
        }

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

            // If there are captures display them
            if(capture.Count > 0)
            {
                foreach (int groupId in capture.Keys)
                {
                    if (!GroupHasValue(groupId))
                        continue;

                    var ci = Peek(groupId);
                    str += $"\n  {(groupId != 0 ? $"Grp {groupId}" : "RegEx")}: Index {ci.beg} - {ci.end} Value: {matchString.Substring(ci.low, ci.high - ci.low)}";

                    // Display extended capture info if the group has matched multiple times
                    if(capture[groupId].Count > 1)
                    {
                        // Display preceding captures in reverse order looking back from current capture
                        var captures = capture[groupId].ToArray();
                        for (int i = 1; i < captures.Length; i++)
                        {
                            var prev = captures[i];
                            str += $"\n   Prev: {prev.beg} - {prev.end} Value: {matchString.Substring(prev.low, prev.high - prev.low)}";
                        }
                    }
                }
            }

            return str;
        }

        #endregion
    }
}
