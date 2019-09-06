﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 唯一对象，存放所有放置建筑的父类
/// </summary>
public class BuildingRoom : BaseUniqueObject<BuildingRoom>
{
    public List<Building> buildingList;
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

    public void SetBuildingsColliderEnable(bool enable)
    {
        foreach (Building building in buildingList)
        {
            building.boxCollider.enabled = enable;
        }
    }
}
