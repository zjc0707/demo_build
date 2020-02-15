public class TransformGroupSaveData : AbstractToStringObject
{
    public Vector3SaveData Position { get; set; }
    public Vector3SaveData EulerAngles { get; set; }
    public Vector3SaveData Scale { get; set; }
    public TransformGroupSaveData()
    {

    }
}