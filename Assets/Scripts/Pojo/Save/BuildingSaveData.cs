using System.Collections.Generic;
public class BuildingSaveData
{
    public TransformGroupSaveData TransformGroupSaveData { get; set; }
    public int ModelDataId { get; set; }
    public List<AnimDataSaveData> NormalAnimDataSaveDatas { get; set; }
    public bool IsAnimOn { get; set; }
    public List<AnimDataSaveData> AppearanceAnimDataSaveDatas { get; set; }
    public BuildingSaveData(Building building)
    {
        this.ModelDataId = building.modelDataId;
        this.TransformGroupSaveData = TransformGroupUtil.ToSaveData(building.transformGroup);
        this.IsAnimOn = building.isAnimOn;
        this.NormalAnimDataSaveDatas = AnimDataUtil.ToSaveData(building.normalAnimDatas);
        this.AppearanceAnimDataSaveDatas = AnimDataUtil.ToSaveData(building.appearanceAnimDatas);
    }
    public BuildingSaveData()
    {

    }
}