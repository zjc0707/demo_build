using UnityEngine;
public class CoordinatePosition : Coordinate
{
    public new static CoordinatePosition current;
    private void Awake()
    {
        current = this;
    }
    private Vector3 beforeTargetData;
    private Vector3 forward;
    private Vector3 beforeData;
    public override void Change(Vector3 add)
    {
        if (hit != null)
        {
            Vector3 project = Vector3.Project(add, forward);
            // this.transform.position = beforeData + project;
            // targetTransform.position = beforeTargetData + project / multiple;
            float f = (targetTransform.position - MyCamera.current.transform.position).magnitude / distanceToCamera;
            targetTransform.position = beforeTargetData + project * f;
            base.ChangeSizeByDistanceToCamera();
            PanelControl.current.UpdatePosData();
            PanelAnimEditor.current.UpdatePosition();
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
    protected override void SetBeforeData()
    {
        beforeData = this.transform.position;
        beforeTargetData = targetTransform.position;
        forward = hit.target.forward;
    }
}