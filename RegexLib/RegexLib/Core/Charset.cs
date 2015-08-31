using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    interface IContains
    {
        bool Contains(char c);
    }

    struct CharRange : IContains
    {
        public char low, high;
        public bool Contains(char c) => c >= low && c <= high;
    }

    class Charset : IMatch, IContains
    {
        public bool positive, forward;

        private int step => forward ? 1 : -1;

        public Charset(bool positive = true, bool forward = true)
        {
            this.positive = positive;
            this.forward = forward;
        }

        // Collections to store the types of things matched
        private readonly HashSet<char> chars = new HashSet<char>();             // The set of characters listed in the character class
        private readonly List<IContains> inclusions = new List<IContains>();    // The set of inclusions
        private readonly List<IContains> exclusions = new List<IContains>();    // The set of exclusions

        #region IMatch

        public bool Match(Context context)
        {
            // We have no chance of matching if we're not on a valid index
            if (forward)
            {
                if (context.offset < 0 || context.offset >= context.Length)
                    return false;
            }
            else
            {
                if (context.offset < 1 || context.offset > context.Length)
                    return false;
            }

            // Check if we contain the current character
            bool result = Contains(forward ? context.curr : context.prev);

            // If we do then advance the offset
            if (result)
                context.offset += step;

            return result;
        }

        public bool MatchNext(Context context)
        {
            // No alternatives, backtrack the offset and fail
            context.offset -= step;
            return false;
        }

        #endregion

        #region IContains

        public bool Contains(char c)
        {
            // Check in the list of chars
            bool result = chars.Contains(c);

            // Check inclusions if we need to
            if (!result)
                foreach (var inclusion in inclusions)
                    if (inclusion.Contains(c))
                    {
                        result = true;
                        break;
                    }

            // Check exclusions if we have to
            if (result)
                foreach (var exclusion in exclusions)
                    if (exclusion.Contains(c))
                    {
                        result = false;
                        break;
                    }

            // Negate the result for negative charsets
            if (!positive)
                result = !result;

            return result;
        }

        #endregion

        #region Include / Exclude helper methods

        public void Include(char c) => chars.Add(c);
        public void Include(IContains include) => inclusions.Add(include);
        public void Include(char low, char high) => inclusions.Add(new CharRange() { low = low, high = high });
        public void Exclude(IContains exclude) => exclusions.Add(exclude);

        #endregion
    }


}
