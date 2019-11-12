using Jc.SqlTool.Core.Attribute;
using Jc.SqlTool.Core.Base;
public class BaseModel : AbstractObject
{
    private int id = StringUtil.INT_NULL;

    [Id]
    public int Id
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }
}