using UnityEngine;
public class TransformGroup
{
    public Vector3 position;
    public Vector3 eulerAngles;
    public Vector3 scale;

    public TransformGroup(float px, float py, float pz, float ex, float ey, float ez)
    {
        this.position = new Vector3(px, py, pz);
        this.eulerAngles = new Vector3(ex, ey, ez);
        this.scale = Vector3.one;
    }
    public TransformGroup(Vector3 position, Vector3 eulerAngles)
    {
        this.position = position;
        this.eulerAngles = eulerAngles;
        this.scale = Vector3.one;
    }

}