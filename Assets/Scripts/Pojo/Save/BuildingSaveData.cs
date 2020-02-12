public class BuildingSaveData
{
    public TransformGroupSaveData TransformGroup { get; set; }
    public int ModelDataId { get; set; }
    public BuildingSaveData(Building building)
    {
        this.ModelDataId = building.modelDataId;
        this.TransformGroup = TransformGroupUtil.ToSaveData(building.transformGroup);
    }
    public BuildingSaveData()
    {

    }
}