using Jc.ToStringTool;
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
}