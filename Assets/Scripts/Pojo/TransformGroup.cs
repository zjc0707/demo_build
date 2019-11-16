using UnityEngine;
using Jc.ToStringTool;
public class TransformGroup : AbstractToStringObject
{
    public Vector3 Position { get; set; }
    public Vector3 EulerAngles { get; set; }
    public Vector3 Scale { get; set; }
    public void Inject(Transform t)
    {
        t.position = this.Position;
        t.eulerAngles = this.EulerAngles;
        t.localScale = this.Scale;
    }
    public TransformGroup(float px, float py, float pz, float ex, float ey, float ez)
    {
        this.Position = new Vector3(px, py, pz);
        this.EulerAngles = new Vector3(ex, ey, ez);
        this.Scale = Vector3.one;
    }
    public TransformGroup(Vector3 position, Vector3 eulerAngles)
    {
        this.Position = position;
        this.EulerAngles = eulerAngles;
        this.Scale = Vector3.one;
    }
    public TransformGroup(Transform transform)
    {
        this.Position = transform.position;
        this.EulerAngles = transform.eulerAngles;
        this.Scale = transform.localScale;
    }
    public TransformGroup()
    {

    }

}