using UnityEngine;
/// <summary>
/// 鼠标移动物体事件
/// 被选中的物体出现坐标系
/// 拖动坐标系移动物体
/// </summary>
public class InputMouseMove : BaseInputMouse
{
    private const int STATE_NONE = 0;
    private const int STATE_BUILDING = 1;
    private const int STATE_COORDINATE = 2;
    private int state = STATE_NONE;
    private RaycastHit hit;
    private Vector3 targetBeforeMovePos, targetToScreenPos, mouseClickToWorldPos, offset;
    /// <summary>
    /// 单击选择物体
    /// </summary>
    protected override void OnMouseLeftClickDown()
    {
        Ray ray = MyCamera.current.Camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit))
        {
            return;
        }
        Transform hitTarget = hit.collider.transform;
        Debug.Log(hitTarget.name);
        if (state <= STATE_BUILDING)
        {
            Building building = BuildingUtil.FindBuilding(hitTarget);
            //点击到其他地方
            if (building != null)
            {
                Coordinate.current.SetTarget(hitTarget);
                PanelControl.current.SetData(building);
                building.Choose();
                state = STATE_BUILDING;
            }
            else if (Coordinate.current.Hit(hitTarget))
            {
                state = STATE_COORDINATE;
                targetBeforeMovePos = Coordinate.current.transform.position;
                targetToScreenPos = MyCamera.current.Camera.WorldToScreenPoint(targetBeforeMovePos);
                mouseClickToWorldPos = MyCamera.current.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, targetToScreenPos.z));
                offset = targetBeforeMovePos - mouseClickToWorldPos;
            }
            else
            {
                Coordinate.current.SetTarget(null);
                PanelControl.current.Close();
                state = STATE_NONE;
            }
        }

    }
    /// <summary>
    /// 拖动物体
    /// </summary>
    protected override void OnMouseLeftClick()
    {
        if (state == STATE_COORDINATE)
        {
            Vector3 nowMouseClickToWorldPos = MyCamera.current.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, targetToScreenPos.z));
            Coordinate.current.Move(targetBeforeMovePos, nowMouseClickToWorldPos - mouseClickToWorldPos);
            PanelControl.current.UpdatePosData();
        }
    }
    protected override void OnMouseLeftClickUp()
    {
        if (state == STATE_COORDINATE)
        {
            state = STATE_BUILDING;
            Coordinate.current.Recovery();
        }
    }
}