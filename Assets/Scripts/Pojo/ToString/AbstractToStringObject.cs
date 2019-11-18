public abstract class AbstractToStringObject
{
    public override string ToString()
    {
        return Json.Serialize(this);
    }
}