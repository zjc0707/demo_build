using System.Collections.Generic;
public static class MaterialDataTest
{
    private static List<MaterialData> list;
    public static List<MaterialData> List
    {
        get
        {
            if (list == null)
            {
                list = new List<MaterialData>{
                    new MaterialData(1,"00FF00FF"),
                    new MaterialData(2,"0000FFFF"),
                    new MaterialData(3,"FF0000FF"),
                    new MaterialData(4,"FFFF00FF")
                };
            }
            return list;
        }
    }
    public static MaterialData Find(int id)
    {
        foreach (MaterialData t in list)
        {
            if (t.Id == id)
            {
                return t;
            }
        }
        return null;
    }
}