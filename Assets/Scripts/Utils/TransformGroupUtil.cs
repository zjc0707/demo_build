using UnityEngine;
public static class TransformGroupUtil
{
    public static TransformGroupSaveData ToSaveData(TransformGroup t)
    {
        return new TransformGroupSaveData()
        {
            Position = ToSaveData(t.Position),
            EulerAngles = ToSaveData(t.EulerAngles),
            Scale = ToSaveData(t.Scale)
        };
    }
    public static Vector3SaveData ToSaveData(Vector3 v)
    {
        return new Vector3SaveData()
        {
            X = v.x,
            Y = v.y,
            Z = v.z
        };
    }
    public static TransformGroup Parse(TransformGroupSaveData t)
    {
        return new TransformGroup()
        {
            Position = Parse(t.Position),
            EulerAngles = Parse(t.EulerAngles),
            Scale = Parse(t.Scale)
        };
    }
    public static Vector3 Parse(Vector3SaveData v)
    {
        return new Vector3(v.X, v.Y, v.Z);
    }
}