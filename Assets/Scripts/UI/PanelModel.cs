using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelModel : BasePanel<PanelModel>
{
    public Transform item;

    protected override void _Start()
    {
        Load();
    }

    private void Load()
    {
        List<PanelControllerItem> items = PanelControllerItemTest.List;
        foreach (PanelControllerItem t in items)
        {
            Transform clone = Instantiate(item.gameObject).transform;
            clone.GetComponentInChildren<Text>().text = t.Name;
            clone.GetComponentInChildren<Button>().onClick.AddListener(delegate
            {
                Debug.Log(t.Url);
                GameObject targetObj = Instantiate(Resources.Load("Prefabs/" + t.Url)) as GameObject;
                targetObj.name = t.Name;
                Building building = BuildingUtil.GetComponentBuilding(targetObj.transform);
                if (building == null)
                {
                    building = targetObj.AddComponent<Building>();
                }
                building.Choose();
                // Person.current.Catch(cube.transform);
                MouseBehaviour.current.Catch(building);
            });
            clone.SetParent(item.parent);
        }
        item.gameObject.SetActive(false);
    }
}
