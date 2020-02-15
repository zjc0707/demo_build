public class AnimDataSaveData
{
    public string Name { get; set; }
    public TransformGroupSaveData Begin { get; set; }
    public TransformGroupSaveData End { get; set; }
    public float Duration { get; set; }
    public bool IsRelative { get; set; }
    public AnimDataSaveData()
    {

    }
}