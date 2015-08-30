namespace RegexLib
{
    struct CaptureInfo
    {
        public readonly int beg, end, groupId, key;

        public int low => beg < end ? beg : end;
        public int high => beg < end ? end : beg;

        public CaptureInfo(int beg, int end, int groupId, int key)
        {
            this.beg = beg;
            this.end = end;
            this.groupId = groupId;
            this.key = key;
        }
    }
}
