using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 基础的鼠标事件
/// 1.catching跟随鼠标移动
/// 2.右键转动相机
/// 3.滚轮缩放
/// </summary>
public abstract class BaseInputMouse
{
    public const float rotateSpeed = 180.0f;
    public const float scaleSpeed = 2f;
    protected Building catchBuilding;
    /// <summary>
    /// 第一次拾取时记录矫正后的坐标
    /// </summary>
    private Vector3 localPosition;
    public virtual void MyUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = MyCamera.current.Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                CatchParent.current.transform.localPosition = new Vector3(hit.point.x, 0, hit.point.z);
            }
        }
        if (catchBuilding != null)
        {
            RaycastHit hit;
            Ray ray = MyCamera.current.Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //长按状态下鼠标跟随，拾取对象边移动边调整位置，避免超出边界等
                CatchParent.current.transform.localPosition = new Vector3(hit.point.x, 0, hit.point.z);
                catchBuilding.transform.localPosition = localPosition;
                catchBuilding.AdjustPosition();
            }
            if (Input.GetMouseButton(0))
            {
                Debug.Log("放置物品：" + catchBuilding.name);
                catchBuilding.Build();
                catchBuilding = null;
                BuildingRoom.current.SetBuildingsColliderEnable(true);
                return;
            }
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //点到UGUI上不进行处理
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseLeftClickDown();
        }
        if (Input.GetMouseButton(0))
        {
            OnMouseLeftClick();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseLeftClickUp();
        }
        if (Input.GetMouseButton(1))
        {
            OnMouseRightClick();
            Coordinate.current.ChangeSizeByDistanceToCamera();
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            MyCamera.current.transform.Translate(Vector3.forward * scroll * scaleSpeed, Space.Self);
            Coordinate.current.ChangeSizeByDistanceToCamera();
        }
    }
    protected virtual void OnMouseLeftClick()
    {

    }
    protected virtual void OnMouseLeftClickDown()
    {

    }
    protected virtual void OnMouseLeftClickUp()
    {

    }
    protected virtual void OnMouseRightClick()
    {
        Transform camera = MyCamera.current.transform;
        camera.RotateAround(camera.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
        camera.RotateAround(camera.position, camera.right, -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime);
    }
    /// <summary>
    /// 拾取物体，点击面板选项
    /// </summary>
    /// <param name="building"></param>
    public void Catch(Building building)
    {
        this.ClearCatch();
        catchBuilding = building;

        Transform t = catchBuilding.transform;
        t.SetParent(CatchParent.current.transform);
        t.localPosition = Vector3.zero;
        catchBuilding.DownToFloor();
        localPosition = t.localPosition;
        catchBuilding.boxCollider.enabled = false;

        BuildingRoom.current.SetBuildingsColliderEnable(false);
    }
    /// <summary>
    /// 清空已经拾取的内容
    /// </summary>
    private void ClearCatch()
    {
        if (catchBuilding == null) return;
        catchBuilding.Build();
        BuildingRoom.current.Remove(catchBuilding);
        catchBuilding = null;
    }
}