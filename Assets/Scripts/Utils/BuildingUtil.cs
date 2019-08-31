using System.Collections.Generic;
using UnityEngine;

public static class BuildingUtil
{
    /// <summary>
    /// 根据centerAndSize添加包围盒
    /// </summary>
    /// <param name="building"></param>
    public static void AddBoxCollider(Building building)
    {
        GameObject obj = building.gameObject;
        BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            return;
        }
        boxCollider = obj.AddComponent<BoxCollider>();
        boxCollider.center = Vector3.zero;
        boxCollider.size = building.centerAndSize.Size;
        //避免碰撞
        boxCollider.isTrigger = true;
    }
    /// <summary>
    /// 鼠标点击到的为子物体，所以需要遍历所有父级查看是否为Building
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Building FindBuilding(Transform target)
    {
        while (target.parent != null)
        {
            Building building = GetComponentBuilding(target);
            if (building != null && target.parent == BuildingRoom.current.transform)
            {
                return building;
            }
            target = target.parent;
        }
        return null;
    }
    /// <summary>
    /// 返回target上的Building及其子类的脚本
    /// </summary>
    public static Building GetComponentBuilding(Transform target)
    {
        Building result = target.GetComponent<Building>();
        if (result == null)
        {
            result = target.GetComponent<Crane>();
        }
        return result;
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