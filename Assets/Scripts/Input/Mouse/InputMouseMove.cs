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
    private Vector3 targetToScreenPos, mouseClickToWorldPos;
    /// <summary>
    /// 单击选择物体
    /// </summary>
    protected override void OnMouseLeftClickDown()
    {
        Ray ray = MyCamera.current.Camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit))
        {
            Coordinate.Target.SetTarget(null);
            PanelList.current.LastRecovery();
            PanelControl.current.Close();
            state = STATE_NONE;
            return;
        }
        Transform hitTarget = hit.collider.transform;
        Debug.Log(hitTarget.name);
        if (state <= STATE_BUILDING)
        {
            Building building = BuildingUtil.FindBuilding(hitTarget);
            bool isNormal = PanelState.current.state == PanelState.State.NORMAL;
            if (building != null && isNormal)
            {
                PanelList.current.Select(building);
                state = STATE_BUILDING;
            }
            else if (Coordinate.Target.Hit(hitTarget))
            {
                state = STATE_COORDINATE;
                targetToScreenPos = MyCamera.current.Camera.WorldToScreenPoint(Coordinate.Target.transform.position);
                mouseClickToWorldPos = MyCamera.current.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, targetToScreenPos.z));
            }
            else if (isNormal)
            {
                //点击到其他地方
                Coordinate.Target.SetTarget(null);
                PanelList.current.LastRecovery();
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
            Coordinate.Target.Change(nowMouseClickToWorldPos - mouseClickToWorldPos);
        }
    }
    protected override void OnMouseLeftClickUp()
    {
        if (state == STATE_COORDINATE)
        {
            state = STATE_BUILDING;
            Coordinate.Target.Recovery();
        }
    }
}