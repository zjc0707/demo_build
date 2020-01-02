using UnityEngine;
public class CoordinateScale : Coordinate
{
    public new static CoordinateScale current;
    private void Awake()
    {
        current = this;
    }
    private Vector3 axis;
    private Vector3 forward;
    private Vector3 beforeData;
    public override void Change(Vector3 add)
    {
        if (hit != null)
        {
            float f = Vector3.Dot(add, forward) > 0 ? 1 : -1;
            targetTransform.localScale = beforeData + 2 * f * axis * Vector3.Project(add, forward).magnitude;
            PanelControl.current.UpdateScaleData();
        }
    }
    protected override Item FindItem(string name)
    {
        Transform t = this.transform.Find(name);
        Transform line = t.Find("Line");
        Transform head = t.Find("Head");
        MeshRenderer mrLine = line.Find("Line").GetComponent<MeshRenderer>();
        return new Item()
        {
            target = t,
            line = line,
            mrLine = mrLine,
            mrHead = head.GetComponent<MeshRenderer>(),
            defaultMaterial = mrLine.sharedMaterial,
        };
    }
    protected override void SetBeforeData()
    {
        switch (hit.target.name)
        {
            case "X": axis = Vector3.right; break;
            case "Y": axis = Vector3.up; break;
            case "Z": axis = Vector3.forward; break;
            default: Debug.LogError("匹配失败：" + hit.target.name); break;
        }
        beforeData = targetTransform.localScale;
        forward = hit.target.forward;
    }
}