using Jc.SqlTool.Core.Mapper;
public abstract class AbstractMapper<T> : BaseMapper<T> where T : BaseModel
{
    public T SelectById(int id)
    {
        if (id < 1)
        {
            throw new System.Exception("Id不得小于1");
        }
        T t = System.Activator.CreateInstance<T>();
        t.Id = id;
        return base.SelectOne(t);
    }
}