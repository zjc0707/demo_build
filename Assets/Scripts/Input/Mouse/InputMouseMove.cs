using UnityEngine;
/// <summary>
/// 鼠标移动物体事件
/// 被选中的物体出现坐标系
/// 拖动坐标系移动物体
/// </summary>
public class InputMouseMove : BaseInputMouse
{
    protected override void OnMouseLeftClick(Transform hit)
    {
        // Debug.Log(hit.name);
        Building building = BuildingUtil.FindBuilding(hit);
        //点击到其他地方
        if (building == null)
        {
            PanelControl.current.Close();
            return;
        }
        PanelControl.current.SetData(building);
        building.Choose();
    }
}