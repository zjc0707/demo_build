using UnityEngine;
public class CoordinateScale : Coordinate
{
    public new static CoordinateScale current;
    private void Awake()
    {
        current = this;
    }
    private const float defaultLineXY = 1f;
    private const float defaultLineZ = 1f;
    public override void Change(Vector3 add)
    {
        if (hit != null)
        {
            // this.transform.localScale = Vector3.one + hit.Project(add);
            targetTransform.localScale = beforeData + hit.Project(add);
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
    protected override void SetScale(float distance)
    {
        items.ForEach(i => i.line.localScale = new Vector3(defaultLineXY / distance, defaultLineXY / distance, defaultLineZ));
        this.transform.localScale = Vector3.one * distance;
    }
    protected override void SetBeforeData()
    {
        beforeData = targetTransform.localScale;
    }
}