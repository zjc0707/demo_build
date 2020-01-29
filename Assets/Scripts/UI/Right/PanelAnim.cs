using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
[UIType(UIStackType.RIGHT)]
public class PanelAnim : BasePanel<PanelAnim>
{
    private Building targetBuilding;
    private List<AnimData> animDatas;
    public enum AnimType
    {
        NORMAL = 0,
        APPEARANCE = 1
    }
    private const string NAME_NORMAL_ANIMS = "场景动画列表";
    private const string NAME_APPEARANCE_ANIMS = "出场动画列表";
    #region UI
    private Text panelName;
    private InputField buildingName;
    private Button buttonPlay;
    private Transform item;
    private List<Transform> items;
    private Dictionary<int, List<Transform>> itemsCache;
    #endregion
    protected override void _Start()
    {
        panelName = transform.Find("Top/Button/Text").GetComponent<Text>();
        buildingName = transform.Find("Content/Name/InputField").GetComponent<InputField>();
        item = transform.Find("Content/Scroll View/Viewport/Content/Item");
        buttonPlay = transform.Find("Content/ButtonPlay").GetComponent<Button>();
        item.gameObject.SetActive(false);
        items = new List<Transform>();
        itemsCache = new Dictionary<int, List<Transform>>();
        //listener
        transform.Find("Content/ButtonAdd").GetComponent<Button>().onClick.AddListener(delegate
        {
            PanelAnimEditor.current.Add(animDatas, targetBuilding, animData =>
            {
                AddItem(animData);
                animDatas.Add(animData);
            });
        });
        buttonPlay.onClick.AddListener(delegate
        {
            buttonPlay.interactable = false;
            PoolOfAnim.current.AddQueue(targetBuilding.transform, animDatas, () =>
            {
                buttonPlay.interactable = true;
            });
        });
    }
    public void Open(Building building, AnimType type)
    {
        BeforeFreshItems();
        targetBuilding = building;
        if (type == AnimType.APPEARANCE)
        {
            animDatas = targetBuilding.appearanceAnimDatas;
            panelName.text = NAME_APPEARANCE_ANIMS;
        }
        else if (type == AnimType.NORMAL)
        {
            animDatas = targetBuilding.normalAnimDatas;
            panelName.text = NAME_NORMAL_ANIMS;
        }
        FreshItems();
        base.Open();
    }
    private void BeforeFreshItems()
    {
        if (targetBuilding == null)
        {
            return;
        }
        if (items.Count == 0)
        {
            return;//无内容不进行缓存
        }
        if (!itemsCache.ContainsKey(targetBuilding.guid))
        {
            itemsCache.Add(targetBuilding.guid, items);
        }
        items.ForEach(p => p.gameObject.SetActive(false));
        items.Clear();
    }
    private void FreshItems()
    {
        buildingName.text = targetBuilding.gameObject.name;
        if (animDatas.Count == 0)
        {
            return;
        }
        if (itemsCache.TryGetValue(targetBuilding.guid, out items))
        {
            //缓存中存在则不重复加载
            items.ForEach(p => p.gameObject.SetActive(true));
            return;
        }
        animDatas.ForEach(AddItem);
    }
    /// <summary>
    /// 添加UI item
    /// </summary>
    /// <param name="data"></param>
    private void AddItem(AnimData data)
    {
        Transform clone = Instantiate(item, item.parent);
        clone.Find("ButtonName/Text").GetComponent<Text>().text = data.Name;
        clone.Find("ButtonName").GetComponent<Button>().onClick.AddListener(delegate
        {
            PanelAnimEditor.current.Open(data, targetBuilding, rs =>
            {
                data.Begin = rs.Begin;
                data.Duration = rs.Duration;
                data.End = rs.End;
                data.Name = rs.Name;
            });
        });
        //delete
        clone.Find("ButtonDelete").GetComponent<Button>().onClick.AddListener(delegate
        {
            Debug.Log("delete:" + data.Name);
        });
        clone.gameObject.SetActive(true);
    }
}