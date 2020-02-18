using UnityEngine;
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
    public TransformGroup(Vector3 position, Vector3 eulerAngles, Vector3 scale)
    {
        this.Position = position;
        this.EulerAngles = eulerAngles;
        this.Scale = scale;
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
    public TransformGroup Clone()
    {
        return new TransformGroup()
        {
            Position = this.Position,
            EulerAngles = this.EulerAngles,
            Scale = this.Scale
        };
    }
    public bool Equals(TransformGroup obj)
    {
        if (obj == null)
        {
            return false;
        }
        return this.Position.Equals(obj.Position)
            && this.EulerAngles.Equals(obj.EulerAngles)
            && this.Scale.Equals(obj.Scale);
    }
    public static TransformGroup operator +(TransformGroup a, TransformGroup b)
    {
        return new TransformGroup()
        {
            Position = a.Position + b.Position,
            EulerAngles = a.EulerAngles + b.EulerAngles,
            Scale = a.Scale + b.Scale
        };
    }
    public static TransformGroup operator -(TransformGroup a, TransformGroup b)
    {
        return new TransformGroup()
        {
            Position = a.Position - b.Position,
            EulerAngles = a.EulerAngles - b.EulerAngles,
            Scale = a.Scale - b.Scale
        };
    }
}