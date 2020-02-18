using UnityEngine;
public class Vector4SaveData
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }
    public Vector4SaveData(float x, float y, float z, float w)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.W = w;
    }
    public Vector4SaveData()
    {

    }
    public static explicit operator Color(Vector4SaveData v)
    {
        return new Color(v.X, v.Y, v.Z, v.W);
    }
    public static explicit operator Vector4SaveData(Color v)
    {
        return new Vector4SaveData(v.r, v.g, v.b, v.a);
    }
}