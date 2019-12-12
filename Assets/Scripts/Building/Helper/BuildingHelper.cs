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
    public static Building Create(ModelData data)
    {
        GameObject obj = CreateGameObjcet(data);
        Building building = BuildingUtil.GetComponentBuilding(obj.transform, data);
        return building;
    }
    public static Building Create(BuildingSaveData data)
    {
        ModelData modelData = ModelDataTest.Find(data.ModelDataId);
        GameObject obj = CreateGameObjcet(modelData);
        Building building = BuildingUtil.GetComponentBuilding(obj.transform, modelData);
        TransformGroupUtil.Parse(data.TransformGroup).Inject(obj.transform);
        return building;
    }
    private static GameObject CreateGameObjcet(ModelData data)
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