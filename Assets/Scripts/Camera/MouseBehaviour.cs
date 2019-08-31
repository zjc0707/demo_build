using System;
using UnityEngine;
public class MouseBehaviour : BaseUniqueObject<MouseBehaviour>
{
    new Camera camera;
    public Transform catchParent;
    private Building catchBuilding;
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
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            catchParent.localPosition = new Vector3(hit.point.x, 0, hit.point.z);
            if (catchBuilding != null)
            {
                this.AdjustCatchBuilding();
            }
            //鼠标单击
            if (Input.GetMouseButtonDown(0))
            {
                this.MouseLeftClick(hit);
            }
        }
    }
    public void Catch(Building building)
    {
        catchBuilding = building;

        Transform t = catchBuilding.transform;
        t.SetParent(catchParent);
        t.localPosition = Vector3.zero;
        //catchBuilding.AdjustPosition();
        catchBuilding.DownToFloor();
        localPosition = t.localPosition;
    }
    private void MouseLeftClick(RaycastHit hit)
    {
        //放置物品
        if (catchBuilding != null)
        {
            this.Build();
            return;
        }
        Building building = BuildingUtil.FindBuilding(hit.collider.transform);
        //点击到其他地方
        if (building == null)
        {
            Building.LastRecovery();
            return;
        }
        //双击时间检测，并检测两次点击是否为一个物体
        if (this.IsMouseLeftDoubleClick() && building.Equals(clickBuilding))
        {
            Debug.Log("doubleClick");
            this.Catch(building);
            return;
        }
        //对buildind单击，并记录点击物体
        else
        {
            clickBuilding = building;
            this.SetPanelControlData(building);
            return;
        }
    }
    private void Build()
    {
        Debug.Log("放置物品：" + catchBuilding.name);
        catchBuilding.Build();
        catchBuilding = null;
    }
    private void SetPanelControlData(Building building)
    {
        Debug.Log("获取物体参数：" + building.name);
        PanelControl.current.SetData(building);
        building.Choose();
    }
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
        Debug.Log(ts.TotalMilliseconds);
        lastClickDateTime = DateTime.Now;
        return ts.TotalMilliseconds < DOUBLE_CLICK_TIME_MILLISECOND;
    }

}