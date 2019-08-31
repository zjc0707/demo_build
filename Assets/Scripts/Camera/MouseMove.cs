using UnityEngine;
public class MouseMove : BaseUniqueObject<MouseMove>
{
    new Camera camera;
    public Transform catchParent;
    private Building catchBuilding;
    private Vector3 localPosition;
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
                catchBuilding.transform.localPosition = localPosition;
                catchBuilding.AdjustPosition();
            }
            //鼠标单击
            if (Input.GetMouseButtonDown(0))
            {
                if (catchBuilding != null)
                {
                    Debug.Log("放置物品：" + catchBuilding.name);
                    catchBuilding.Build();
                    catchBuilding = null;

                    return;
                }
                Building building = BuildingUtil.FindBuilding(hit.collider.transform);
                if (building != null)
                {
                    Debug.Log("获取物体参数：" + building.name);
                    PanelControl.current.SetData(building);
                    building.Choose();

                    return;
                }

                Building.LastRecovery();
            }
        }
    }
    public void Catch(Building building)
    {
        catchBuilding = building;

        Transform t = catchBuilding.transform;
        t.SetParent(catchParent);
        t.localPosition = Vector3.zero;
        catchBuilding.DownToFloor();
        localPosition = t.localPosition;
    }
}