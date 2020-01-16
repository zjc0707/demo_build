using System;
using System.Collections.Generic;
using UnityEngine;

public static class BuildingUtil
{
    /// <summary>
    /// 记录场景中每种物体的个数
    /// </summary>
    /// <typeparam name="int">modelId</typeparam>
    /// <typeparam name="int">number of model</typeparam>
    /// <returns></returns>
    private static Dictionary<int, int> dicCount = new Dictionary<int, int>();
    private static Dictionary<Transform, Building> buildingCache = new Dictionary<Transform, Building>();
    #region util
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
        Transform buildingRoom = PanelList.current.buildingRoom;
        while (t != null)
        {
            if (t.parent == buildingRoom)
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
    #endregion
    #region create
    public static Building Create(Model data)
    {
        GameObject obj = CreateGameObjcet(data);
        Building building = BuildingUtil.GetComponentBuilding(obj.transform, data);
        return building;
    }
    public static Building Create(BuildingSaveData data)
    {
        Model model = LocalAssetUtil.GetModel(data.ModelDataId);
        if (model == null)
        {
            return null;
        }
        GameObject obj = CreateGameObjcet(model);
        Building building = BuildingUtil.GetComponentBuilding(obj.transform, model);
        TransformGroupUtil.Parse(data.TransformGroup).Inject(obj.transform);
        return building;
    }
    private static GameObject CreateGameObjcet(Model data)
    {
        GameObject rs = PoolOfAsset.current.Create(data.Id);
        int count = 0;
        string name = data.Name;
        if (name.Contains("."))
        {
            name = name.Substring(0, name.IndexOf('.'));
        }
        if (dicCount.TryGetValue(data.Id, out count))
        {
            rs.name = string.Format("{0}({1})", name, count);
            dicCount[data.Id] = ++count;
        }
        else
        {
            dicCount.Add(data.Id, ++count);
            rs.name = name;
        }
        return rs;
    }
    #endregion
}