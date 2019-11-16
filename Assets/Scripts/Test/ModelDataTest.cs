using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class ModelDataTest
{
    private static List<ModelData> list;
    public static List<ModelData> List
    {
        get
        {
            if (list == null)
            {
                list = new List<ModelData>
                {
                    new ModelData(1,"立方体", "Cube"),
                    new ModelData(2,"长方体", "Cuboid"),
                    new ModelData(3,"圆柱体", "Cylinder"),
                    new ModelData(4,"球", "Sphere"),
                    new ModelData(5,"胶囊", "Capsule"),
                    new ModelData(6,"吊车","Crane", 1)
                };
            }

            return list;
        }
    }
    public static ModelData Find(int id)
    {
        foreach (ModelData t in list)
        {
            if (t.Id == id)
            {
                return t;
            }
        }
        return null;
    }
}
