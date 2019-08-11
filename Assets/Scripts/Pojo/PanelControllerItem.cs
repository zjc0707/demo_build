using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PanelControllerItem
{
    public static List<PanelControllerItem> List
    {
        get
        {
            List<PanelControllerItem> temp = new List<PanelControllerItem>
            {
                new PanelControllerItem("蓝色方块", "CubeBlue"),
                new PanelControllerItem("绿色方块", "CubeGreen"),
                new PanelControllerItem("红色方块", "CubeRed"),
                new PanelControllerItem("黄色方块", "CubeYellow")
            };
            return temp;
        }
    }
    public string Name { get; set; }
    public string Url { get; set; }
    PanelControllerItem(string name, string url)
    {
        Name = name;
        Url = url;
    }
}
