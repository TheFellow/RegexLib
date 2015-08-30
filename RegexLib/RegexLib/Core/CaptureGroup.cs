using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexLib.Core
{
    class CaptureGroup : MatchSingleBase
    {
        public readonly int groupId;

        public CaptureGroup(IMatch matchItem, int groupId = 0)
            : base(matchItem)
        {
            this.groupId = groupId;
        }

        public override bool Match(Context context)
        {
            int startingOffset = context.offset;

            bool result = matchItem.Match(context);

            // If we've matched then Push a new capture info to store this offset
            if (result)
                context.Push(startingOffset, context.offset, groupId);

            return result;
        }

        public override bool MatchNext(Context context)
        {
            var ci = context.Pop(groupId);

            bool result = matchItem.MatchNext(context);

            if (result)
                context.Push(ci.beg, context.offset, groupId);

            return result;
        }
    }
}
