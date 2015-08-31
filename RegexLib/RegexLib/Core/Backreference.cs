using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    class Backreference : MatchBase
    {
        public readonly CaptureGroup captureGroup;

        public int groupId => captureGroup.groupId;

        public Backreference(CaptureGroup captureGroup)
            : base()
        {
            this.captureGroup = captureGroup;
        }

        public override bool Match(Context context)
        {
            if (!context.GroupHasValue(groupId))
                return false;

            int offset = context.offset;
            string str = context.GroupValue(groupId);
            int len = str.Length;

            if (offset + len > context.Length)
                return false;

            for (int i = 0; i < len; i++)
                if (context.matchString[offset + i] != str[i])
                    return false;

            // Advance the offset to cover the matched string
            context.offset += len;

            // Save the original offset to restore in a MatchNext
            context.Push(offset);
            return true;
        }

        public override bool MatchNext(Context context)
        {
            // Restore the previous offset
            int offset = context.Pop();
            context.offset = offset;

            return false;
        }
    }
}
