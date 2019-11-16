public class BuildingSaveData
{
    public TransformGroupSaveData TransformGroup { get; set; }
    public int ModelDataId { get; set; }
    public bool IsLock { get; set; }
    public BuildingSaveData(Building building)
    {
        this.ModelDataId = building.data.Id;
        this.IsLock = building.isLock;
        this.TransformGroup = TransformGroupUtil.ToSaveData(building.transformGroup);
    }
    public BuildingSaveData()
    {

    }
}