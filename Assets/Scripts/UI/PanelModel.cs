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
                GameObject cube = Instantiate(Resources.Load("Prefabs/" + t.Url)) as GameObject;
                cube.name = t.Name;
                Building building = cube.AddComponent<Building>();
                building.Choose();
                // Person.current.Catch(cube.transform);
                MouseMove.current.Catch(building);
            });
            clone.SetParent(item.parent);
        }
        item.gameObject.SetActive(false);
    }
}
