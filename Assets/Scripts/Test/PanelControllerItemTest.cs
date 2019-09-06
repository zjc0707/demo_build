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
                new PanelControllerItemData("立方体", "Cube"),
                new PanelControllerItemData("长方体", "Cuboid"),
                new PanelControllerItemData("圆柱体", "Cylinder"),
                new PanelControllerItemData("球", "Sphere"),
                new PanelControllerItemData("胶囊", "Capsule"),
                new PanelControllerItemData("吊车","Crane", 1)
            };
            return temp;
        }
    }
}
