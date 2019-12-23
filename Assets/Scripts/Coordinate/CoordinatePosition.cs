using UnityEngine;
public class CoordinatePosition : Coordinate
{
    public new static CoordinatePosition current;
    private void Awake()
    {
        current = this;
    }
    private const float defaultLineXY = 0.001f;
    private const float defaultLineZ = 0.01f;
    public override void Change(Vector3 add)
    {
        if (hit != null)
        {
            this.transform.position = beforeData + hit.Project(add);
            targetTransform.position = this.transform.position;
            PanelControl.current.UpdatePosData();
        }
    }
    protected override Item FindItem(string name)
    {
        Transform t = this.transform.Find(name);
        Transform line = t.Find("Line");
        Transform head = t.Find("Head");
        MeshRenderer mrLine = line.GetComponent<MeshRenderer>();
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
        beforeData = targetTransform.position;
    }
}