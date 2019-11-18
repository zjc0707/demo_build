using System.Collections.Generic;
namespace Jc.SqlTool.Core.Metadata.SqlInfo
{
    using Condition;
    public class OrderByInfo
    {
        public List<string> list = new List<string>();
        public void OrderBy(string column)
        {
            list.Add(string.Format(SqlCondition.ORDERBY, column));
        }
        public void OrderByDesc(string column)
        {
            list.Add(string.Format(SqlCondition.ORDERBY_DESC, column));
        }
        public override string ToString()
        {
            return list.Count == 0 ? "" : string.Join(", ", list);
        }
    }
}