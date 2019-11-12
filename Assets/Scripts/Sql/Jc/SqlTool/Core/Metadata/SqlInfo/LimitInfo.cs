namespace Jc.SqlTool.Core.Metadata.SqlInfo
{
    using Condition;
    public class LimitInfo
    {
        public int Start { get; set; }
        public int Count { get; set; }
        public LimitInfo()
        {
            Start = 0;
            Count = 100;
        }
        public override string ToString()
        {
            return string.Format(SqlCondition.LIMIT, Start, Count);
        }
    }
}