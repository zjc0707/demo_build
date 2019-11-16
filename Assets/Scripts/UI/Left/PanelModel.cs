using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PanelModel : BasePanel<PanelModel>
{
    protected override int StackType { get { return UIStackType.LEFT; } }
    public Transform baseItem;

    protected override void _Start()
    {
        Load();
        base.Open();
    }
    private void Load()
    {
        List<ModelData> items = ModelDataTest.List;
        foreach (ModelData t in items)
        {
            Transform clone = Instantiate(baseItem.gameObject).transform;
            clone.GetComponentInChildren<Text>().text = t.Name;
            clone.GetComponentInChildren<Button>().onClick.AddListener(delegate
            {
                Debug.Log(t.Url);
                Building building = BuildingHelper.Create(t);
                building.Choose();
                MouseBehaviour.current.Catch(building);
            });
            clone.Find("Image").GetComponent<Image>().sprite = ImageUtil.GetSprite(t);
            clone.SetParent(baseItem.parent);
        }

        baseItem.gameObject.SetActive(false);
    }
}
