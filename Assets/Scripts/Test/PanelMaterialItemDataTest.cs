using System.Collections.Generic;
public static class PanelMaterialItemDataTest
{
    public static List<PanelMaterialItemData> List
    {
        get
        {
            return new List<PanelMaterialItemData>{
                new PanelMaterialItemData("00FF00FF"),
                new PanelMaterialItemData("0000FFFF"),
                new PanelMaterialItemData("FF0000FF"),
                new PanelMaterialItemData("FFFF00FF")
            };
        }
    }
}