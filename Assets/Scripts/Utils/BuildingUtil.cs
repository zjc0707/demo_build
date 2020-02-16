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
    /// <summary>
    /// 赋值给每个Building对象唯一标识符，自增
    /// </summary>
    private static int guid = 0;
    private const string CLONE_SUFFIX = "(clone)";
    /// <summary>
    /// 点击物体和对应building的映射缓存
    /// </summary>
    /// <typeparam name="Transform"></typeparam>
    /// <typeparam name="Building"></typeparam>
    /// <returns></returns>
    private static Dictionary<Transform, Building> buildingCache = new Dictionary<Transform, Building>();
    #region util
    public static void Fresh()
    {
        guid = 0;
        dicCount.Clear();
    }
    /// <summary>
    /// 根据centerAndSize添加包围盒
    /// </summary>
    public static BoxCollider AddBoxCollider(GameObject obj)
    {
        Vector3 pos = obj.transform.position;
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
        obj.transform.position = pos;
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
    /// 返回target上的Building脚本，若不存在则添加
    /// </summary>
    public static Building GetComponentBuilding(Transform target)
    {
        Building result = target.GetComponent<Building>();
        if (result == null)
        {
            result = target.gameObject.AddComponent<Building>();
        }
        return result;
    }
    /// <summary>
    /// 根据data给target新增building脚本，并赋值guid
    /// </summary>
    /// <param name="target"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Building AddComponentBuilding(Transform target, Model data)
    {
        Building result = GetComponentBuilding(target);
        result.modelDataId = data.Id;
        result.guid = guid++;
        return result;
    }
    /// <summary>
    /// 克隆
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
    public static Building Clone(Building building)
    {
        Building cloneBuilding = Create(LocalAssetUtil.GetModel(building.modelDataId), building.gameObject.name + CLONE_SUFFIX);
        cloneBuilding.Build();
        building.transformGroup.Inject(cloneBuilding.transform);
        building.appearanceAnimDatas.ForEach(p => cloneBuilding.appearanceAnimDatas.Add(p.Clone()));
        building.normalAnimDatas.ForEach(p => cloneBuilding.normalAnimDatas.Add(p.Clone()));
        cloneBuilding.isAnimOn = building.isAnimOn;
        return cloneBuilding;
    }
    #endregion
    #region create
    public static Building Create(Model data, string goName = null)
    {
        GameObject obj = CreateGameObjcet(data, goName);
        return AddComponentBuilding(obj.transform, data);
    }
    /// <summary>
    /// 从数据库储存数据加载building
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Building Create(BuildingSaveData data)
    {
        Model model = LocalAssetUtil.GetModel(data.ModelDataId);
        if (model == null)
        {
            return null;
        }
        GameObject obj = CreateGameObjcet(model, data.Name);
        TransformGroupUtil.Parse(data.TransformGroupSaveData).Inject(obj.transform);
        Building building = AddComponentBuilding(obj.transform, model);
        #region animation
        building.isAnimOn = data.IsAnimOn;
        building.appearanceAnimDatas = AnimDataUtil.Parse(data.AppearanceAnimDataSaveDatas);
        building.normalAnimDatas = AnimDataUtil.Parse(data.NormalAnimDataSaveDatas);
        #endregion
        return building;
    }
    private static GameObject CreateGameObjcet(Model data, string goName = null)
    {
        GameObject rs = PoolOfAsset.current.Create(data.Id);
        if (string.IsNullOrEmpty(goName))
        {
            string name = data.Name;
            if (name.Contains("."))
            {
                name = name.Substring(0, name.IndexOf('.'));
            }
            int count = 0;
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
        }
        else
        {
            rs.name = goName;
        }
        return rs;
    }
    #endregion
}