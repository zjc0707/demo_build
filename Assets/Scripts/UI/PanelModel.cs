using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PanelModel : BasePanel<PanelModel>
{
    public Transform baseItem;

    protected override void _Start()
    {
        Load();
    }

    private void Load()
    {
        List<PanelControllerItemData> items = PanelControllerItemTest.List;
        foreach (PanelControllerItemData t in items)
        {
            Transform clone = Instantiate(baseItem.gameObject).transform;
            clone.GetComponentInChildren<Text>().text = t.Name;
            clone.GetComponentInChildren<Button>().onClick.AddListener(delegate
            {
                Debug.Log(t.Url);
                GameObject targetObj = Instantiate(Resources.Load(t.Url)) as GameObject;
                targetObj.name = t.Name;
                Building building = BuildingUtil.GetComponentBuilding(targetObj.transform);
                if (building == null)
                {
                    building = targetObj.AddComponent<Building>();
                }
                building.data = t;
                building.Choose();
                // Person.current.Catch(cube.transform);
                MouseBehaviour.current.Catch(building);
            });
            StartCoroutine(CoroutineUtil.LoadSprite(clone.Find("Image").GetComponent<Image>(), t));
            clone.SetParent(baseItem.parent);
        }

        baseItem.gameObject.SetActive(false);
    }
}
