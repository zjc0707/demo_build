using UnityEngine;
public class MouseMove : BaseUniqueObject<MouseMove>
{
    new Camera camera;
    public Transform catchParent;
    private Building catchBuilding;
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
                catchBuilding.AdjustPosition();
            }
        }
    }
    public void Catch(Building building)
    {
        catchBuilding = building;
        Transform t = catchBuilding.transform;
        t.SetParent(catchParent);
        t.localPosition = Vector3.zero;
    }
}