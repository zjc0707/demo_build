using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public abstract class BaseItemData
{
    public string Name { get; private set; }
    public string Url { get; private set; }
    public abstract string Prefix { get; }
    public BaseItemData(string name, string url)
    {
        this.Name = name;
        this.Url = this.Prefix + url;
    }
}