using System.Collections.Generic;
using UnityEngine;

public static class BuildingUtil
{
    private static Dictionary<Transform, Building> buildingCache = new Dictionary<Transform, Building>();
    /// <summary>
    /// 根据centerAndSize添加包围盒
    /// </summary>
    /// <param name="building"></param>
    // public static BoxCollider AddBoxCollider(Building building)
    // {
    //     GameObject obj = building.gameObject;
    //     obj.transform.position = Vector3.zero;
    //     BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
    //     if (boxCollider == null)
    //     {
    //         boxCollider = obj.AddComponent<BoxCollider>();
    //         // boxCollider.center = Vector3.zero;
    //         boxCollider.center = building.centerAndSize.Center;
    //         boxCollider.size = building.centerAndSize.Size;
    //         Debug.Log(obj.name + "-" + boxCollider.center + "-" + boxCollider.size);
    //     }
    //     //避免碰撞
    //     // boxCollider.isTrigger = true;
    //     return boxCollider;
    // }
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
        Transform parent = target.parent;
        while (parent != null)
        {
            if (parent == BuildingRoom.current.transform)
            {
                Building building = GetComponentBuilding(target);
                buildingCache.Add(target, building);
                return building;
            }
            parent = parent.parent;
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
    public static Building GetComponentBuilding(Transform target, ModelData data)
    {
        Building result = GetComponentBuilding(target);
        result.data = data;
        return result;
    }
    /// <summary>
    /// 通过对齐左上角来使整体对齐
    /// </summary>
    /// <param name="building"></param>
    public static void AdjustPosition(Building building)
    {
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
        float x = vector3.x + building.Size.x;
        if (x > Floor.current.x)
        {
            t.position -= new Vector3(x - Floor.current.x, 0, 0);
        }
        float z = vector3.z + building.Size.z;
        if (z > Floor.current.z)
        {
            t.position -= new Vector3(0, 0, z - Floor.current.z);
        }
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