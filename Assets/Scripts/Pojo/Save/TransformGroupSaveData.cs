using UnityEngine;
public class TransformGroupSaveData : AbstractToStringObject
{
    public Vector3SaveData Position { get; set; }
    public Vector3SaveData EulerAngles { get; set; }
    public Vector3SaveData Scale { get; set; }
    public TransformGroupSaveData()
    {

    }
    public static explicit operator TransformGroup(TransformGroupSaveData t)
    {
        return new TransformGroup((Vector3)t.Position, (Vector3)t.EulerAngles, (Vector3)t.Scale);
    }
    public static explicit operator TransformGroupSaveData(TransformGroup t)
    {
        return new TransformGroupSaveData()
        {
            Position = (Vector3SaveData)t.Position,
            EulerAngles = (Vector3SaveData)t.EulerAngles,
            Scale = (Vector3SaveData)t.Scale
        };
    }
}