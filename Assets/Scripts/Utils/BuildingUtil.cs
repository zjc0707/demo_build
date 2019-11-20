using System.Collections.Generic;
using UnityEngine;

public static class BuildingUtil
{
    private static Dictionary<Transform, Building> buildingCache = new Dictionary<Transform, Building>();
    /// <summary>
    /// 根据centerAndSize添加包围盒
    /// </summary>
    /// <param name="building"></param>
    public static BoxCollider AddBoxCollider(Building building)
    {
        GameObject obj = building.gameObject;
        obj.transform.position = Vector3.zero;
        BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = obj.AddComponent<BoxCollider>();
            // boxCollider.center = Vector3.zero;
            boxCollider.center = building.centerAndSize.Center;
            boxCollider.size = building.centerAndSize.Size;
            Debug.Log(obj.name + "-" + boxCollider.center + "-" + boxCollider.size);
        }
        //避免碰撞
        // boxCollider.isTrigger = true;
        return boxCollider;
    }
    /// <summary>
    /// 鼠标点击到的为子物体，所以需要遍历所有父级查看是否为Building
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Building FindBuilding(Transform target)
    {
        if (buildingCache.ContainsKey(target))
        {
            return buildingCache[target];
        }
        while (target.parent != null)
        {
            Building building = GetComponentBuilding(target);
            if (building != null && target.parent == BuildingRoom.current.transform)
            {
                buildingCache.Add(target, building);
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
    /// <summary>
    /// 通过对齐左上角来使整体对齐
    /// </summary>
    /// <param name="building"></param>
    public static void AdjustPosition(Building building)
    {
        // Debug.Log("调整坐标:" + building.name);
        Transform t = building.transform;
        t.eulerAngles = new Vector3(FixEuler(t.eulerAngles.x),
            FixEuler(t.eulerAngles.y),
            FixEuler(t.eulerAngles.z));
        Vector3 afterFix = new Vector3(FixPos(building.LeftBackBottom.x),
            building.LeftBackBottom.y,
            FixPos(building.LeftBackBottom.z));
        //Debug.Log(building.LeftFrontBottom + "_after:" + afterFix + "_pos:" + t.localPosition);
        t.position += afterFix - building.LeftBackBottom;

        Vector3 vector3 = building.LeftBackBottom;
        if (vector3.x < 0)
        {
            t.position += new Vector3(0 - vector3.x, 0, 0);
        }
        if (vector3.z < 0)
        {
            t.position += new Vector3(0, 0, 0 - vector3.z);
        }
        float x = vector3.x + building.centerAndSize.Size.x;
        if (x > Floor.current.x)
        {
            t.position -= new Vector3(x - Floor.current.x, 0, 0);
        }
        float z = vector3.z + building.centerAndSize.Size.z;
        if (z > Floor.current.z)
        {
            t.position -= new Vector3(0, 0, z - Floor.current.z);
        }
        // Debug.Log(building.centerAndSize + "_" + building.LeftBackBottom + "_" + building.transform.position);
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