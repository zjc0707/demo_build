using UnityEngine;
public class Vector3SaveData : AbstractToStringObject
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public Vector3SaveData(float x, float y, float z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
    public Vector3SaveData()
    {

    }
    public static explicit operator Vector3SaveData(Vector3 v)
    {
        return new Vector3SaveData(v.x, v.y, v.z);
    }
    public static explicit operator Vector3(Vector3SaveData v)
    {
        return new Vector3(v.X, v.Y, v.Z);
    }
}