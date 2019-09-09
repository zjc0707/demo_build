using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public abstract class BaseItemData
{
    public string Name { get; private set; }
    public string Url
    {
        get; private set;
    }
    public abstract string UrlFolder { get; }
    public readonly string ImgFolder = "Image/";
    public string ImgUrl { get { return ImgFolder + Url; } }
    private Sprite _sprite;
    public Sprite sprite
    {
        get
        {
            if (_sprite == null)
            {
                // _sprite = Resources.Load(ImgUrl) as Sprite;
                _sprite = Resources.Load<Sprite>(ImgUrl);
            }
            return _sprite;
        }
    }
    public BaseItemData(string name, string url)
    {
        this.Name = name;
        this.Url = this.UrlFolder + url;
    }
}