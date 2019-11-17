namespace Jc.SqlTool.Core.Metadata.SqlInfo
{
    using Condition;
    public class LimitInfo
    {
        /// <summary>
        /// 数据库数据行下标
        /// </summary>
        /// <value></value>
        public int Start { get; set; }
        /// <summary>
        /// 查找数量
        /// </summary>
        /// <value></value>
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