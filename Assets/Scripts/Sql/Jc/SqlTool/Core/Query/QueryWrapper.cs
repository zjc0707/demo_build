namespace Jc.SqlTool.Core.Query
{
    using Helper;
    using Page;
    public class QueryWrapper<T> : AbstractWrapper<T>
    {
        public QueryWrapper(T entity)
        {
            base.entity = entity;
            base.entityType = entity.GetType();
            base.tableInfo = TableInfoHelper.GetTableInfo(base.entityType);
        }
        public QueryWrapper<T> Page(Page<T> page)
        {
            return this.Limit(page.StartIndex * page.PageSize, page.PageSize);
        }
        public QueryWrapper<T> Select(params string[] columns)
        {
            if (columns.Length > 0)
            {
                base.selectStr = string.Join(",", columns);
            }
            return this;
        }
        public QueryWrapper<T> Eq(string column, string value)
        {
            base.whereInfos.Add(WhereInfoHelper.Eq(column, value));
            return this;
        }
        public QueryWrapper<T> Le(string column, string value)
        {
            base.whereInfos.Add(WhereInfoHelper.Le(column, value));
            return this;
        }
        public QueryWrapper<T> Ge(string column, string value)
        {
            base.whereInfos.Add(WhereInfoHelper.Ge(column, value));
            return this;
        }
        public QueryWrapper<T> Limit(int start, int count)
        {
            base.limitInfo.Start = start;
            base.limitInfo.Count = count;
            return this;
        }
        public QueryWrapper<T> OrderBy(string param)
        {
            base.orderByInfo.Param = param;
            return this;
        }
    }
}