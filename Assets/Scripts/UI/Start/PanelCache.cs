using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
[UIType(UIStackType.START)]
public class PanelCache : PanelPage<PanelCache>
{
    protected override void LoadNewPage()
    {
        List<Item> items = GetItems();
        List<GameObject> list = new List<GameObject>();
        item.SetActive(true);
        foreach (Item p in items)
        {
            Transform clone = Instantiate(item, content).transform;
            clone.Find("Name").GetComponentInChildren<Text>().text = p.name;
            clone.Find("Size").GetComponentInChildren<Text>().text = p.size;
            Transform operate = clone.Find("Operate");
            operate.Find("ButtonLoad").gameObject.SetActive(false);
            operate.Find("ButtonDelete").GetComponent<Button>().onClick.AddListener(delegate
            {
                AssetBundleUtil.Clear();
                DirectoryInfo directoryInfo = new DirectoryInfo(MyWebRequest.current.LOCAL_ASSET_PATH);
                foreach (FileInfo fi in directoryInfo.GetFiles())
                {
                    fi.Delete();
                }
                directoryInfo.Delete();
            });
            list.Add(clone.gameObject);
        }
        item.SetActive(false);
        pageCache.Add(nowIndex, list);
        pages = 1;
        FreshNavigation();
    }
    private List<Item> GetItems()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(MyWebRequest.current.LOCAL_ASSET_PATH);
        float size = 0f;
        foreach (FileInfo fi in directoryInfo.GetFiles())
        {
            size += fi.Length;
        }
        return new List<Item>() { new Item() { name = directoryInfo.Name, size = FormatterUtil.GetSizeString(size) } };
    }
    public class Item
    {
        public string name { get; set; }
        public string size { get; set; }
    }
}