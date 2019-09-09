public class PanelControllerItemData : BaseItemData
{
    public override string UrlFolder { get { return "Prefabs/"; } }
    /// <summary>
    /// 是否生效building的MyUpdate  0-不可操作；1-可操作
    /// </summary>
    public int Operate { get; private set; }
    public PanelControllerItemData(string name, string url, int operate) : this(name, url)
    {
        this.Operate = operate;
    }
    public PanelControllerItemData(string name, string url) : base(name, url)
    {
        this.Operate = 0;
    }
}
