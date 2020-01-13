using System.Collections.Generic;
using UnityEngine;
public static class BuildingHelper
{
    private static Dictionary<int, int> dicCount = new Dictionary<int, int>();
    /// <summary>
    /// 记录上一个被选中的物体
    /// </summary>
    public static Building LastTarget { get; set; }
    /// <summary>
    /// 复原上一个被选中的物体，不存在则直接return
    /// </summary>
    public static void LastRecovery()
    {
        if (LastTarget == null)
        {
            return;
        }
        LastTarget.Recovery();
        LastTarget = null;
    }
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
        if (dicCount.TryGetValue(data.Id, out count))
        {
            rs.name = string.Format("{0}({1})", data.Name, count);
            dicCount[data.Id] = ++count;
        }
        else
        {
            dicCount.Add(data.Id, ++count);
            rs.name = data.Name;
        }
        return rs;
    }
}