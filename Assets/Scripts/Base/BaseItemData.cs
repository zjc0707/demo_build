using System.Collections.Generic;
using UnityEngine;
using Jc.SqlTool.Core.Attribute;
public abstract class BaseItemData : BaseModel
{
    public string Name { get; set; }
    private string url;
    public string Url { get { return UrlFolder + url; } set { this.url = value; } }
    [Assistant]
    public abstract string UrlFolder { get; }
    public BaseItemData(int id, string name, string url)
    {
        base.Id = id;
        this.Name = name;
        this.Url = url;
    }
    public BaseItemData()
    {

    }
}