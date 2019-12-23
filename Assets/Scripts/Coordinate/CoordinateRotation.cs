using UnityEngine;
public class CoordinateRotation : Coordinate
{
    public new static CoordinateRotation current;
    private void Awake()
    {
        current = this;
    }
    public override void Change(Vector3 add)
    {
        if (hit != null)
        {
            this.transform.eulerAngles = Compute(beforeData + hit.Project(add) * 10);
            targetTransform.eulerAngles = this.transform.eulerAngles;
            PanelControl.current.UpdateRotData();
        }
    }
    private Vector3 Compute(Vector3 v)
    {
        return new Vector3(Compute(v.x), Compute(v.y), Compute(v.z));
    }
    private float Compute(float f)
    {
        return (f + 360) % 360;
    }
    protected override Item FindItem(string name)
    {
        Transform t = this.transform.Find(name);
        Transform line = t;
        MeshRenderer mrLine = line.GetComponent<MeshRenderer>();
        return new Item()
        {
            target = t,
            line = line,
            mrLine = mrLine,
            defaultMaterial = mrLine.sharedMaterial,
        };
    }
    protected override void SetScale(float distance)
    {
        this.transform.localScale = Vector3.one * distance;
    }
    protected override void SetBeforeData()
    {
        beforeData = targetTransform.eulerAngles;
    }
}