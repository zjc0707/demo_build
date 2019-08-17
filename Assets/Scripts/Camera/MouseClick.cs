using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    new Camera camera;
    Ray ray;
    RaycastHit hit;
    // Use this for initialization
    void Start () {
        camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                Building building = FindBuilding(hit.collider.transform);
                if (building != null)
                {
                    //Debug.Log("is Building");
                    PanelControl.current.SetData(building);
                    building.Choose();
                    State.current.SetStateToBuilding();
                }
            }
        }
	}

    /// <summary>
    /// 鼠标点击到的为子物体，所以需要遍历所有父级查看是否为Building
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private Building FindBuilding(Transform target)
    {
        while(target.parent != null)
        {
            Building building = target.GetComponent<Building>();
            if(building != null && target.parent == BuildingRoom.current.transform)
            {
                return building;
            }
            target = target.parent;
        }
        return null;
    }
}
