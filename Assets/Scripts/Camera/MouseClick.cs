using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    new Camera camera;
    Ray ray;
    RaycastHit hit;
    // Use this for initialization
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Building building = BuildingUtil.FindBuilding(hit.collider.transform);
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
}
