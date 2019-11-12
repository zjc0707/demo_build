namespace Jc.SqlTool.Core.Metadata.SqlInfo
{
    using Condition;
    public class OrderByInfo
    {
        public string Param { get; set; }
        public override string ToString()
        {
            return string.IsNullOrEmpty(Param) ? "" : string.Format(SqlCondition.ORDERBY, Param);
        }
    }
}