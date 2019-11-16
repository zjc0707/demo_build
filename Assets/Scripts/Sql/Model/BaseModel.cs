using Jc.SqlTool.Core.Attribute;
using Jc.ToStringTool;
public class BaseModel : AbstractToStringObject
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
    public BaseModel()
    {

    }
}