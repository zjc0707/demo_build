public class PanelMaterialItemData : BaseItemData
{
    public override string Prefix { get { return "Material/"; } }
    public PanelMaterialItemData(string name, string url) : base(name, url)
    {

    }
    public PanelMaterialItemData(string url) : this(url, url)
    {

    }
}