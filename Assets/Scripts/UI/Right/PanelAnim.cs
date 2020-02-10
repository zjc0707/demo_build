using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
[UIType(UIStackType.RIGHT)]
public class PanelAnim : BasePanel<PanelAnim>
{
    private Building targetBuilding;
    /// <summary>
    /// open时记录对象初始值，用于close时复原
    /// </summary>
    private TransformGroup openTransformGroup;
    private List<AnimData> animDatas;
    public enum AnimType
    {
        NORMAL = 0,
        APPEARANCE = 1
    }
    private AnimType animType;
    private const string NAME_NORMAL_ANIMS = "场景动画列表";
    private const string NAME_APPEARANCE_ANIMS = "出场动画列表";
    #region UI
    private Text panelName;
    private InputField buildingName;
    private Button buttonPlay;
    private Transform item;
    private List<Transform> items;
    /// <summary>
    /// itemsCache为后两者的引用，在open时根据不同的type赋值
    /// </summary>
    private Dictionary<int, List<Transform>> itemsCache, itemsAppearanceAnimCache, itemsNormalAnimCache;
    #endregion
    protected override void _Start()
    {
        panelName = transform.Find("Top/Button/Text").GetComponent<Text>();
        buildingName = transform.Find("Content/Name/InputField").GetComponent<InputField>();
        item = transform.Find("Content/Scroll View/Viewport/Content/Item");
        buttonPlay = transform.Find("Content/ButtonPlay").GetComponent<Button>();
        item.gameObject.SetActive(false);
        items = new List<Transform>();
        itemsAppearanceAnimCache = new Dictionary<int, List<Transform>>();
        itemsNormalAnimCache = new Dictionary<int, List<Transform>>();
        //listener
        transform.Find("Content/ButtonAdd").GetComponent<Button>().onClick.AddListener(delegate
        {
            PanelAnimEditor.current.Add(animDatas, targetBuilding, animData =>
            {
                AddItem(animData);
                animDatas.Add(animData);
            });
        });
        transform.Find("Content/ButtonBack").GetComponent<Button>().onClick.AddListener(Close);
        buttonPlay.onClick.AddListener(delegate
        {
            buttonPlay.interactable = false;
            PoolOfAnim.current.AddItemInQueue(targetBuilding.transform, animDatas, () =>
            {
                buttonPlay.interactable = true;
            });
        });
    }
    /// <summary>
    /// 关闭并复位,编辑出场动画时判断end的状态和物体编辑时状态是否相同
    /// </summary>
    public override void Close()
    {
        if (animType == AnimType.NORMAL)
        {
            openTransformGroup.Inject(targetBuilding.transform);
            base.Close();
        }
        else
        {
            if (animDatas.Count > 0 && !animDatas[animDatas.Count - 1].End.Equals(openTransformGroup))
            {
                PanelDialog.current.Open("动画末尾项与编辑位置不相同，是否修改编辑位置", () =>
                {
                    animDatas[animDatas.Count - 1].End.Inject(targetBuilding.transform);
                    base.Close();
                });
            }
            else
            {
                openTransformGroup.Inject(targetBuilding.transform);
                base.Close();
            }
        }
    }
    /// <summary>
    /// 打开页面，根据type选择窗口名和缓存集合
    /// <br/>building的transformGroup改为animDatas的最后一项
    /// </summary>
    public void Open(Building building, AnimType type)
    {
        BeforeFreshItems();
        targetBuilding = building;
        openTransformGroup = targetBuilding.transformGroup;
        animType = type;
        if (type == AnimType.APPEARANCE)
        {
            animDatas = targetBuilding.appearanceAnimDatas;
            itemsCache = itemsAppearanceAnimCache;
            panelName.text = NAME_APPEARANCE_ANIMS;
        }
        else if (type == AnimType.NORMAL)
        {
            animDatas = targetBuilding.normalAnimDatas;
            itemsCache = itemsNormalAnimCache;
            panelName.text = NAME_NORMAL_ANIMS;
        }
        if (animDatas.Count > 0)
        {
            animDatas[animDatas.Count - 1].End.Inject(targetBuilding.transform);
            Coordinate.Target.SetTarget(targetBuilding.transform);
        }
        FreshItems();
        base.Open();
    }
    public void Reverse()
    {
        animDatas.Reverse();
        animDatas.ForEach(p =>
        {

        });
    }
    private void BeforeFreshItems()
    {
        if (targetBuilding == null)
        {
            return;
        }
        if (items.Count == 0)
        {
            Debug.Log("无内容不进行缓存");
            return;//无内容不进行缓存
        }
        if (!itemsCache.ContainsKey(targetBuilding.guid))
        {
            Debug.Log("缓存");
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
        Transform clone = Instantiate(item.gameObject, item.parent).transform;
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
        items.Add(clone);
    }
}