using System;
namespace Jc.SqlTool.Core.Mapper
{
    using System.Collections.Generic;
    using Toolkit;
    using Query;
    using Helper;
    using Page;

    public class BaseMapper<T> where T : class
    {
        public string Test()
        {
            return "mapper";
        }
        public T SelectOne(T entity)
        {
            return SelectOne(Wrappers.Query(entity));
        }
        public T SelectOne(QueryWrapper<T> queryWrapper)
        {
            List<T> rs = Select(queryWrapper);
            return rs.Count == 0 ? null : rs[0];
        }
        public List<T> Select()
        {
            return Select(System.Activator.CreateInstance<T>());
        }
        public List<T> Select(T entity)
        {
            return Select(Wrappers.Query(entity));
        }
        public List<T> Select(QueryWrapper<T> queryWrapper)
        {
            return SqlHelper.GetResult<T>(queryWrapper.ToSelect());
        }
        public Page<T> Select(Page<T> page)
        {
            return Select(System.Activator.CreateInstance<T>(), page);
        }
        public Page<T> Select(T entity, Page<T> page)
        {
            return Select(Wrappers.Query(entity), page);
        }
        public Page<T> Select(QueryWrapper<T> queryWrapper, Page<T> page)
        {
            return SqlHelper.GetResult<T>(queryWrapper.Page(page).ToSelect(), page);
        }
        public bool Insert(T entity)
        {
            return Insert(Wrappers.Query(entity));
        }
        public bool Insert(QueryWrapper<T> queryWrapper)
        {
            return SqlHelper.GetNonQueryResult(queryWrapper.ToInsert());
        }
        public bool Update(T entity)
        {
            return Update(Wrappers.Query(entity));
        }
        public bool Update(QueryWrapper<T> queryWrapper)
        {
            return SqlHelper.GetNonQueryResult(queryWrapper.ToUpdate());
        }
        public bool UpdateById(T entity)
        {
            return UpdateById(Wrappers.Query(entity));
        }
        public bool UpdateById(QueryWrapper<T> queryWrapper)
        {
            return SqlHelper.GetNonQueryResult(queryWrapper.ToUpdateById());
        }
        public bool Delete(T entity)
        {
            return Delete(entity);
        }
        public bool Delete(QueryWrapper<T> queryWrapper)
        {
            return SqlHelper.GetNonQueryResult(queryWrapper.ToDelete());
        }
        public int Count()
        {
            return SqlHelper.Count<T>();
        }
    }
}