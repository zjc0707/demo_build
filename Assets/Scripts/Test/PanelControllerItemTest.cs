using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class PanelControllerItemTest
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
                new PanelControllerItem("黄色方块", "CubeYellow"),
                new PanelControllerItem("组合方块", "CubeGroup"),
                new PanelControllerItem("吊车","Crane", 1),
                new PanelControllerItem("墙","Wall")
            };
            return temp;
        }
    }
}
