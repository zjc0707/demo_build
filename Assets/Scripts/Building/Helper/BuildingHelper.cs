using UnityEngine;
public static class BuildingHelper
{
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
        building.isLock = data.IsLock;
        TransformGroupUtil.Parse(data.TransformGroup).Inject(obj.transform);
        return building;
    }
    private static GameObject CreateGameObjcet(ModelData data)
    {
        // GameObject obj = GameObject.Instantiate(Resources.Load(data.Url)) as GameObject;
        // obj.name = data.Name;
        // return obj;
        return PoolOfAsset.current.Create(data.Id);
    }
}