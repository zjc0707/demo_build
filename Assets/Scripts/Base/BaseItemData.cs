using System.Collections.Generic;
using UnityEngine;
public abstract class BaseItemData
{
    public int Id { get; set; }
    public string Name { get; set; }
    private string url;
    public string Url { get { return UrlFolder + url; } set { this.url = value; } }
    public string ABName { get { return url.ToLower() + ".zjc"; } }
    public abstract string UrlFolder { get; }
    public BaseItemData(int id, string name, string url)
    {
        this.Id = id;
        this.Name = name;
        this.Url = url;
    }
    public BaseItemData()
    {

    }
}