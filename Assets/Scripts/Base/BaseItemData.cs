using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public abstract class BaseItemData
{
    public string Name { get; private set; }
    public string Url { get; private set; }
    public abstract string Prefix { get; }
    private Texture2D texture2D;
    public Texture2D Texture2D
    {
        get
        {
            if (texture2D == null)
            {
                texture2D = AssetPreview.GetAssetPreview(Resources.Load(this.Url)) as Texture2D;
            }
            return texture2D;
        }
    }
    private Sprite sprite;
    public Sprite Sprite
    {
        get
        {
            if (sprite == null)
            {
                Texture2D tex = this.Texture2D;
                sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            }
            return sprite;
        }
    }
    public BaseItemData(string name, string url)
    {
        this.Name = name;
        this.Url = this.Prefix + url;
    }
}