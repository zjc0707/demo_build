using System;
using System.Collections.Generic;
using UnityEngine;

public static class BuildingUtil
{
    private static Dictionary<Transform, Building> buildingCache = new Dictionary<Transform, Building>();
    /// <summary>
    /// 根据centerAndSize添加包围盒
    /// </summary>
    public static BoxCollider AddBoxCollider(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = obj.AddComponent<BoxCollider>();
            CenterAndSize cs = CenterAndSizeUtil.Get(obj.transform);
            boxCollider.center = cs.Center;
            boxCollider.size = cs.Size;
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
        Transform t = target;
        while (t != null)
        {
            if (t.parent == BuildingRoom.current.transform)
            {
                Building building = GetComponentBuilding(t);
                buildingCache.Add(target, building);
                return building;
            }
            t = t.parent;
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
            result = target.gameObject.AddComponent<Building>();
        }
        //目前忽视吊车相关脚本
        // if (result == null)
        // {
        //     result = target.GetComponent<Crane>();
        // }
        return result;
    }
    public static Building GetComponentBuilding(Transform target, Model data)
    {
        Building result = GetComponentBuilding(target);
        result.data = data;
        // Debug.Log(data == null);
        return result;
    }
}