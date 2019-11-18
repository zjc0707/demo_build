public class TransformGroupSaveData : AbstractToStringObject
{
    public Vector3SaveData Position { get; set; }
    public Vector3SaveData EulerAngles { get; set; }
    public Vector3SaveData Scale { get; set; }
    public TransformGroupSaveData()
    {

    }
    public static TransformGroupSaveData Zero()
    {
        return new TransformGroupSaveData()
        {
            Position = new Vector3SaveData(0, 0, 0),
            EulerAngles = new Vector3SaveData(0, 0, 0),
            Scale = new Vector3SaveData(1, 1, 1)
        };
    }
}