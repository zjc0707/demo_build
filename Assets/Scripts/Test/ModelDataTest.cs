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
                    // new ModelData(7,"车","Car"),
                    // new ModelData(8,"电动门","Door"),
                    // new ModelData(9,"栅栏","Guardrail"),
                    // new ModelData(10,"楼","House"),
                    // new ModelData(11,"保安室","Police Station"),
                    // new ModelData(12,"树","Tree"),
                    // new ModelData(13,"草堆","Grass"),
                    new ModelData(1,"立方体", "Cube"),
                    new ModelData(2,"长方体", "Cuboid"),
                    new ModelData(3,"圆柱体", "Cylinder"),
                    new ModelData(4,"球", "Sphere"),
                    new ModelData(5,"胶囊", "Capsule"),
                    // new ModelData(6,"吊车","Crane", 1)

            };
            }

            return list;
        }
    }
    public static ModelData Find(int id)
    {
        foreach (ModelData t in List)
        {
            if (t.Id == id)
            {
                return t;
            }
        }
        return null;
    }
}
