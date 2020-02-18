using System.Collections.Generic;
using UnityEngine;
public class BuildingSaveData
{
    public string Name { get; set; }
    public TransformGroupSaveData TransformGroupSaveData { get; set; }
    public int ModelDataId { get; set; }
    public List<AnimDataSaveData> NormalAnimDataSaveDatas { get; set; }
    public bool IsAnimOn { get; set; }
    public List<AnimDataSaveData> AppearanceAnimDataSaveDatas { get; set; }
    public Vector4SaveData MaterialColorSaveData { get; set; }
    public BuildingSaveData(Building building)
    {
        this.Name = building.name;
        this.ModelDataId = building.modelDataId;
        this.TransformGroupSaveData = (TransformGroupSaveData)building.transformGroup;
        this.IsAnimOn = building.isAnimOn;
        this.NormalAnimDataSaveDatas = AnimDataUtil.ToSaveData(building.normalAnimDatas);
        this.AppearanceAnimDataSaveDatas = AnimDataUtil.ToSaveData(building.appearanceAnimDatas);
        if (building.isChangeMaterial)
        {
            this.MaterialColorSaveData = (Vector4SaveData)building.Material.color;
        }
    }
    public BuildingSaveData()
    {

    }
}