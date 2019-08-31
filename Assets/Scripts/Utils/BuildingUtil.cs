using System.Collections.Generic;
using UnityEngine;

public static class BuildingUtil
{
    /// <summary>
    /// 鼠标点击到的为子物体，所以需要遍历所有父级查看是否为Building
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Building FindBuilding(Transform target)
    {
        while (target.parent != null)
        {
            Building building = target.GetComponent<Building>();
            if (building != null && target.parent == BuildingRoom.current.transform)
            {
                return building;
            }
            target = target.parent;
        }
        return null;
    }
    public static void AdjustPosition(Building building)
    {
        // Debug.Log("调整坐标:" + building.name);
        Transform t = building.transform;
        t.eulerAngles = new Vector3(FixEuler(t.eulerAngles.x),
            FixEuler(t.eulerAngles.y),
            FixEuler(t.eulerAngles.z));
        Vector3 afterFix = new Vector3(FixPos(building.LeftFrontBottom.x),
            FixPos(building.LeftFrontBottom.y),
            FixPos(building.LeftFrontBottom.z));
        //Debug.Log(building.LeftFrontBottom + "_after:" + afterFix + "_pos:" + t.localPosition);
        t.position += afterFix - building.LeftFrontBottom;
    }

    private static int FixEuler(float f)
    {
        return ((int)(f + 45) / 90) * 90;
    }
    private static int FixPos(float f)
    {
        if (f > 0)
            return (int)(f + 0.5);
        else
            return (int)(f - 0.5);
    }
}