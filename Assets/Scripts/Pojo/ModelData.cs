/// <summary>
/// 资源模型的数据类
/// </summary>
public class ModelData : BaseItemData
{
    public override string UrlFolder { get { return "Prefabs/"; } }
    /// <summary>
    /// 是否生效building的MyUpdate  0-不可操作；1-可操作
    /// </summary>
    public int Operate { get; set; }
    public ModelData(int id, string name, string url, int operate) : this(id, name, url)
    {
        this.Operate = operate;
    }
    public ModelData(int id, string name, string url) : base(id, name, url)
    {
        this.Operate = 0;
    }
    public ModelData()
    {

    }
}
