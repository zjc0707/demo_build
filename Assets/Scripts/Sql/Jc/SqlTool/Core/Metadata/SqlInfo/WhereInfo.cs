namespace Jc.SqlTool.Core.Metadata.SqlInfo
{
    using Condition;
    public class WhereInfo
    {
        public string Column { get; set; }
        public string Operate { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return string.Format(SqlCondition.WHERE, Column, Operate);
        }
    }
}