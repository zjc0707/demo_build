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
}