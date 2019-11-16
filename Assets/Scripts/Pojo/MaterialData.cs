public class MaterialData : BaseItemData
{
    public override string UrlFolder { get { return "Material/"; } }
    public MaterialData(int id, string name, string url) : base(id, name, url)
    {

    }
    public MaterialData(int id, string url) : this(id, url, url)
    {

    }
    public MaterialData()
    {

    }
}