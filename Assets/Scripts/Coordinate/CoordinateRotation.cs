using UnityEngine;
public class CoordinateRotation : Coordinate
{
    public new static CoordinateRotation current;
    private void Awake()
    {
        current = this;
    }
    Vector3 axis;
    public override void Change(Vector3 add)
    {
        if (hit != null)
        {
            this.transform.RotateAround(this.transform.position, axis, Input.GetAxis("Mouse X") * 10);
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
    protected override void SetBeforeData()
    {
        switch (hit.target.name)
        {
            case "X": axis = this.transform.right; break;
            case "Y": axis = this.transform.up; break;
            case "Z": axis = this.transform.forward; break;
            default: Debug.LogError("匹配失败：" + hit.target.name); break;
        }
    }
}