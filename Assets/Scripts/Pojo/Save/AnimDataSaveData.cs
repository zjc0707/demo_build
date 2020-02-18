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
    public static explicit operator AnimData(AnimDataSaveData animDataSaveData)
    {
        return new AnimData()
        {
            Name = animDataSaveData.Name,
            Begin = (TransformGroup)animDataSaveData.Begin,
            End = (TransformGroup)animDataSaveData.End,
            Duration = animDataSaveData.Duration,
            IsRelative = animDataSaveData.IsRelative
        };
    }
    public static explicit operator AnimDataSaveData(AnimData animData)
    {
        return new AnimDataSaveData()
        {
            Name = animData.Name,
            Begin = (TransformGroupSaveData)animData.Begin,
            End = (TransformGroupSaveData)animData.End,
            Duration = animData.Duration,
            IsRelative = animData.IsRelative
        };
    }
}