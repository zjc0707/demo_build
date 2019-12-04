using System.Data;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class MouseBehaviour : BaseUniqueObject<MouseBehaviour>
{
    new Camera camera;
    /// <summary>
    /// 约等于手，生成的或点击的物体都置于其中
    /// </summary>
    public Transform catchParent;
    private Building catchBuilding;
    /// <summary>
    /// 上一次点击对象
    /// </summary>
    private Building clickBuilding;
    private Vector3 localPosition;
    private readonly double DOUBLE_CLICK_TIME_MILLISECOND = 500;
    private DateTime lastClickDateTime;
    // Use this for initialization
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //点击到ugui上
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            BuildingHelper.LastRecovery();
        }
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            catchParent.localPosition = new Vector3(hit.point.x, 0, hit.point.z);
            if (catchBuilding != null)
            {
                this.AdjustCatchBuilding();
            }
            //鼠标左键
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("hit:" + hit.collider.transform.name);
                this.MouseLeftClick(hit);
            }
            //鼠标右键
            if (Input.GetMouseButtonDown(1))
            {
                if (catchBuilding == null)
                {
                    return;
                }
                catchBuilding.Rotate90();
            }
        }
    }
    /// <summary>
    /// 鼠标左键点击事件
    /// </summary>
    /// <param name="hit"></param>
    private void MouseLeftClick(RaycastHit hit)
    {
        //放置物品
        if (catchBuilding != null)
        {
            if (catchBuilding.gameObject.activeInHierarchy == false)
            {
                return;
            }
            this.Build();
            return;
        }
        Building building = BuildingUtil.FindBuilding(hit.collider.transform);
        //点击到其他地方
        if (building == null)
        {
            PanelControl.current.Close();
            return;
        }
        this.SetPanelControlData(building);
        // Debug.Log(building.Equals(clickBuilding) + "_" + (building == clickBuilding));
        //双击时间检测，并检测两次点击是否为一个物体
        if (this.IsMouseLeftDoubleClick() && building.Equals(clickBuilding))
        {
            Debug.Log("doubleClick");
            this.Catch(building);
            return;
        }
        //对buildind单击，并记录点击物体
        clickBuilding = building;
    }
    /// <summary>
    /// 更新面板显示数据，并标记该building为选中
    /// </summary>
    /// <param name="building"></param>
    private void SetPanelControlData(Building building)
    {
        PanelControl.current.SetData(building);
        building.Choose();
    }
    /// <summary>
    /// 调整building在catchParent中的位置
    /// </summary>
    private void AdjustCatchBuilding()
    {
        catchBuilding.transform.localPosition = localPosition;
        catchBuilding.AdjustPosition();
    }
    /// <summary>
    /// 记录点击时间，计算两次时间间隔是否满足条件
    /// </summary>
    private bool IsMouseLeftDoubleClick()
    {
        if (lastClickDateTime == null)
        {
            lastClickDateTime = DateTime.Now;
        }
        TimeSpan ts = DateTime.Now - lastClickDateTime;
        // Debug.Log(ts.TotalMilliseconds);
        lastClickDateTime = DateTime.Now;
        return ts.TotalMilliseconds < DOUBLE_CLICK_TIME_MILLISECOND;
    }
    /// <summary>
    /// 拾取物体，在点击面板选项和双击场景中到物体时调用
    /// </summary>
    /// <param name="building"></param>
    public void Catch(Building building)
    {
        this.ClearCatch();
        catchBuilding = building;

        Transform t = catchBuilding.transform;
        t.SetParent(catchParent);
        t.localPosition = Vector3.zero;
        //catchBuilding.AdjustPosition();
        catchBuilding.DownToFloor();
        localPosition = t.localPosition;
        catchBuilding.boxCollider.enabled = false;

        BuildingRoom.current.SetBuildingsColliderEnable(false);
    }
    /// <summary>
    /// 清空已经拾取的内容
    /// </summary>
    public void ClearCatch()
    {
        if (catchBuilding == null) return;
        DestroyImmediate(catchBuilding.gameObject);
        catchBuilding = null;
    }
    /// <summary>
    /// 放置物体，并清空已经拾取的内容
    /// </summary>
    private void Build()
    {
        Debug.Log("放置物品：" + catchBuilding.name);
        catchBuilding.Build();
        catchBuilding = null;

        BuildingRoom.current.SetBuildingsColliderEnable(true);
    }

}