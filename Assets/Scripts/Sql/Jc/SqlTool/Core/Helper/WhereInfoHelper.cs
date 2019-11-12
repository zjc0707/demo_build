namespace Jc.SqlTool.Core.Helper
{
    using Metadata.SqlInfo;
    public static class WhereInfoHelper
    {
        public static WhereInfo Eq(string column, object value)
        {
            return Get(column, "=", value);
        }
        public static WhereInfo Le(string column, object value)
        {
            return Get(column, "<=", value);
        }
        public static WhereInfo Ge(string column, object value)
        {
            return Get(column, ">=", value);
        }
        public static WhereInfo Get(string column, string operate, object value)
        {
            return new WhereInfo()
            {
                Column = column,
                Operate = operate,
                Value = value
            };
        }
    }
}