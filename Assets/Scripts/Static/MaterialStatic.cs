using System;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceStatic
{
    //字体
    private static Font _font;
    public static Font FONT
    {
        get
        {
            if (_font == null)
            {
                _font = Resources.Load("上首京东体") as Font;
            }
            return _font;
        }
    }
    //颜色材质
    public static Material BLUE { get { return GetTranslucentMaterial("Blue"); } }
    public static Material RED { get { return GetTranslucentMaterial("Red"); } }
    public static Material GREEN { get { return GetTranslucentMaterial("Green"); } }

    private static Dictionary<string, Material> dicMaterial = new Dictionary<string, Material>();
    public static Material GetTranslucentMaterial(string color)
    {
        if (!dicMaterial.ContainsKey(color))
        {
            Material material = Resources.Load("Material/Translucent/" + color) as Material;
            if (material == null)
            {
                throw new NullReferenceException("材质[" + color + "]不存在");
            }
            dicMaterial.Add(color, material);
            return material;
        }

        return dicMaterial[color];
    }
}
