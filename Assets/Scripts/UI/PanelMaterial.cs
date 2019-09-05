using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class PanelMaterial : BasePanel<PanelMaterial>
{
    public Transform baseItem;
    protected override void _Start()
    {
        Load();
    }

    private void Load()
    {
        List<PanelMaterialItemData> items = PanelMaterialItemDataTest.List;
        foreach (PanelMaterialItemData t in items)
        {
            Transform clone = Instantiate(baseItem.gameObject).transform;
            clone.GetComponentInChildren<Text>().text = t.Name;
            clone.GetComponentInChildren<Button>().onClick.AddListener(delegate
            {

            });
            // clone.Find("Image").GetComponent<Image>().sprite = t.Sprite;
            clone.SetParent(baseItem.parent);
        }

        baseItem.gameObject.SetActive(false);
    }
}