using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class PanelControllerItemTest
{
    public static List<PanelControllerItemData> List
    {
        get
        {
            List<PanelControllerItemData> temp = new List<PanelControllerItemData>
            {
                new PanelControllerItemData("蓝色方块", "CubeBlue"),
                new PanelControllerItemData("绿色方块", "CubeGreen"),
                new PanelControllerItemData("红色方块", "CubeRed"),
                new PanelControllerItemData("黄色方块", "CubeYellow"),
                // new PanelControllerItem("组合方块", "CubeGroup"),
                new PanelControllerItemData("吊车","Crane", 1),
                new PanelControllerItemData("墙","Wall")
            };
            return temp;
        }
    }
}
