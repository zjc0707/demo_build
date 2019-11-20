using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class PanelMaterial : BasePanel<PanelMaterial>
{
    protected override int StackType { get { return UIStackType.LEFT; } }
    public Transform baseItem;
    protected override void _Start()
    {
        Load();
    }
    private void Load()
    {
        List<MaterialData> items = MaterialDataTest.List;
        foreach (MaterialData t in items)
        {
            Transform clone = Instantiate(baseItem.gameObject).transform;
            clone.GetComponentInChildren<Text>().text = t.Name;
            clone.GetComponentInChildren<Button>().onClick.AddListener(delegate
            {
                Material material = Resources.Load(t.Url) as Material;
                PanelControl.current.SetTargetMaterial(material);
            });
            clone.Find("Image").GetComponent<Image>().sprite = ImageUtil.GetSprite(t);
            clone.SetParent(baseItem.parent);
        }

        baseItem.gameObject.SetActive(false);
    }
}