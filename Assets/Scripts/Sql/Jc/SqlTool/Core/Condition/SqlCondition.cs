namespace Jc.SqlTool.Core.Condition
{
    public static class SqlCondition
    {
        public const string WHERE = "{0} {1} @{0}";
        public const string WHERE_EQ = "{0} = @{0}";
        public const string LIMIT = "LIMIT {0},{1} ";
        public const string ORDERBY = "ORDER BY {0} ";
        public const string SELECT = "SELECT {0} FROM {1} WHERE {2} ";
        public const string INSERT = "INSERT INTO {0}({1}) VALUES({2}) ";
        public const string UPDATE = "UPDATE {0} SET ({1}) WHERE {2} ";
        public const string DELETE = "DELETE FROM {0} WHERE {1} ";
        public const string COUNT = "SELECT COUNT({0}) FROM {1}";
    }
}