namespace Jc.SqlTool.Core.Toolkit
{
    using Query;
    public class Wrappers
    {
        public static QueryWrapper<T> Query<T>(T entity)
        {
            return new QueryWrapper<T>(entity);
        }
    }
}