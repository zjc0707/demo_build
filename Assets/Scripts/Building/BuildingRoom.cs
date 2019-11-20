using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存放所有放置建筑的父类
/// </summary>
public class BuildingRoom : BaseUniqueObject<BuildingRoom>
{
    public List<Building> buildingList = new List<Building>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Add(Building building)
    {
        building.transform.SetParent(this.transform);
        buildingList.Add(building);
    }
    public void Remove(Building building)
    {
        buildingList.Remove(building);
        GameObject.DestroyImmediate(building.gameObject);
    }
    public void Reset()
    {
        foreach (Building building in buildingList)
        {
            if (building != null && building.gameObject != null)
                GameObject.DestroyImmediate(building.gameObject);
        }
        buildingList.Clear();
    }
    /// <summary>
    /// 统一修改building对象的boxcollider.enable,使放置时不会被挡住位置
    /// </summary>
    /// <param name="enable"></param>
    public void SetBuildingsColliderEnable(bool enable)
    {
        foreach (Building building in buildingList)
        {
            building.boxCollider.enabled = enable;
        }
    }
}
