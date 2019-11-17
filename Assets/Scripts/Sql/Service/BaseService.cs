using System.Collections.Generic;
using Jc.SqlTool.Core.Page;
//三个泛型参数最后一个S用于生成S脚本挂在unique gameObject上
//若直接用BaseService<T, M>，则子类定义的方法无法被使用
//也可以用IOC的方法实现该功能，目前这样用起来更方便
public abstract class BaseService<T, M, S> : BaseUniqueObject<S>
                                    where M : AbstractMapper<T>
                                    where T : BaseModel
                                    where S : BaseService<T, M, S>
{
    private M m;
    protected M Mapper
    {
        get
        {
            if (m == null)
            {
                m = System.Activator.CreateInstance<M>();
            }
            return m;
        }
    }
    public string Test()
    {
        return "test";
    }
    public T SelectById(int id)
    {
        return Mapper.SelectById(id);
    }
    public T SelectOne(T entity)
    {
        return Mapper.SelectOne(entity);
    }
    public List<T> Select()
    {
        return Mapper.Select();
    }
    public Page<T> Select(Page<T> page)
    {
        return Mapper.Select(page);
    }
    public List<T> Select(T entity)
    {
        return Mapper.Select(entity);
    }
    public Page<T> Select(T entity, Page<T> page)
    {
        return Mapper.Select(entity, page);
    }
    public bool Insert(T entity)
    {
        return Mapper.Insert(entity);
    }
    /// <summary>
    /// 避免与MonoBehaviour的update重名
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool UpdateAll(T entity)
    {
        return Mapper.Update(entity);
    }
    public bool UpdateById(T entity)
    {
        return Mapper.UpdateById(entity);
    }
    public bool Delete(T entity)
    {
        return Mapper.Delete(entity);
    }
    public int Count()
    {
        return Mapper.Count();
    }
}