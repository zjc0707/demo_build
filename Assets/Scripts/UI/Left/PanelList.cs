using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[UIType(UIStackType.LEFT)]
public class PanelList : BasePanel<PanelList>
{
    public class Item
    {
        public Transform ui { get; set; }
        public Image background { get; set; }
        public Building building { get; set; }
    }
    public List<Item> items { get; private set; }
    /// <summary>
    /// buildings对应的tg数据，启动动画时缓存，结束时赋值
    /// </summary>
    private Dictionary<Item, TransformGroup> itemDataDic;
    /// <summary>
    /// 点击/长按对象
    /// </summary>
    private Item selectItem;
    #region UI对象
    private Transform content;
    private Transform baseItem;
    private VerticalLayoutGroup verticalLayoutGroup;
    private const int itemHeight = 30;
    private Color selectColor = Color.yellow;
    #endregion
    #region 长按
    private Vector3 itemStartPosition;
    private int itemSiblingIndex;
    /// <summary>
    /// 按下计时
    /// /// </summary>
    private float downTime = 0f;
    /// <summary>
    ///出发长按所需时间
    /// </summary>
    private const float NEED_DOWN_TIME = 0.15f;
    #endregion
    #region 状态
    private enum STATE : int
    {
        NULL = 0,
        DOWN,
        LONG_DOWN
    }
    private STATE state = STATE.NULL;
    #endregion
    #region Building
    /// <summary>
    /// 所有building的父类
    /// </summary>
    public Transform buildingRoom;
    #endregion

    protected override void _Start()
    {
        if (content == null)
        {
            content = this.transform.Find("Scroll View").Find("Viewport").Find("Content");
        }
        if (baseItem == null)
        {
            baseItem = content.Find("Item");
        }
        verticalLayoutGroup = content.GetComponent<VerticalLayoutGroup>();
        baseItem.gameObject.SetActive(false);
        items = new List<Item>();
        itemDataDic = new Dictionary<Item, TransformGroup>();
        this.transform.Find("Buttons/ButtonViewModel").GetComponent<Button>().onClick.AddListener(UGUITree.current.ViewModelTurnOn);
        this.transform.Find("Buttons/ButtonPlayAppearanceAnim").GetComponent<Button>().onClick.AddListener(UGUITree.current.PlayAppearanceAnim);
    }
    private void Update()
    {
        if (state == STATE.DOWN)
        {
            downTime += Time.deltaTime;
            if (downTime >= NEED_DOWN_TIME)
            {
                state = STATE.LONG_DOWN;
                verticalLayoutGroup.enabled = false;
                itemStartPosition = selectItem.ui.position;
                itemSiblingIndex = selectItem.ui.GetSiblingIndex();
            }
        }
        if (state > STATE.NULL)
        {
            if (Input.GetMouseButtonUp(0))
            {
                state = STATE.NULL;
                // selectItem.ui = null;
                downTime = 0f;
                verticalLayoutGroup.enabled = true;
            }
        }
        if (state == STATE.LONG_DOWN)
        {
            selectItem.ui.position = new Vector3(selectItem.ui.position.x, Input.mousePosition.y, selectItem.ui.position.z);

            if (selectItem.ui.localPosition.y > 0)
            {
                selectItem.ui.localPosition = new Vector3(selectItem.ui.localPosition.x, 0, selectItem.ui.localPosition.z);
            }
            else if (selectItem.ui.localPosition.y < -1 * (items.Count - 1) * itemHeight)
            {
                selectItem.ui.localPosition = new Vector3(selectItem.ui.localPosition.x, -1 * (items.Count - 1) * itemHeight, selectItem.ui.localPosition.z);
            }

            float add = selectItem.ui.position.y - itemStartPosition.y;
            if (Math.Abs(add) >= itemHeight)//移动距离大于UI高度时
            {
                //根据移动的上下确定nextItem的位置，将items中二者位置调换，
                //UI中将selectItem.ui的初始坐标赋值nextItem
                int nextIndex = itemSiblingIndex - (add > 0 ? 1 : -1);
                Transform t = items[nextIndex - 1].ui;//getSiblingIndex()从1开始
                items.RemoveAt(itemSiblingIndex - 1);
                items.Insert(nextIndex - 1, selectItem);
                Vector3 pos = t.position;
                t.position = itemStartPosition;
                itemStartPosition = pos;

                itemSiblingIndex = nextIndex;
                selectItem.ui.SetSiblingIndex(nextIndex);
            }
        }
    }
    private void OnDown(GameObject go)
    {
        Select(items.Find(item => item.ui.gameObject == go));
        state = STATE.DOWN;
    }
    #region BuildRoom Method
    public void Add(Building building)
    {
        GameObject clone = Instantiate(baseItem.gameObject, content);
        clone.name = building.gameObject.name;
        clone.GetComponentInChildren<Text>().text = clone.name;
        EventTriggerListener.Get(clone).onDown += OnDown;
        clone.SetActive(true);
        building.transform.SetParent(buildingRoom);
        items.Add(new Item()
        {
            building = building,
            ui = clone.transform,
            background = clone.GetComponent<Image>()
        });
    }
    public void Remove(Building building)
    {
        LastRecovery();
        int index = items.FindIndex(i => i.building == building);
        Item item = items[index];
        PoolOfAsset.current.Destroy(building);
        GameObject.DestroyImmediate(item.ui.gameObject);
        Coordinate.Target.SetTarget(null);
        items.RemoveAt(index);
    }
    public void Reset()
    {
        items.ForEach(i =>
        {
            GameObject.DestroyImmediate(i.ui.gameObject);
            PoolOfAsset.current.Destroy(i.building);
        });
        items.Clear();
    }
    /// <summary>
    /// 统一修改building对象的boxcollider.enable,使放置时不会被挡住位置
    /// </summary>
    /// <param name="enable"></param>
    public void SetBuildingsColliderEnable(bool enable)
    {
        items.ForEach(i =>
        {
            i.building.boxCollider.enabled = enable;
        });
    }
    public void LastRecovery()
    {
        if (selectItem == null)
        {
            return;
        }
        selectItem.building.HideHighLight();
        selectItem.background.color = Color.white;
        selectItem = null;
    }
    public void Select(Building building)
    {
        Select(items.Find(i => i.building == building));
    }
    /// <summary>
    /// 缓存编辑状态下的building.tg
    /// </summary>
    public void SaveBuildingDatas()
    {
        itemDataDic.Clear();
        items.ForEach(p =>
        {
            itemDataDic.Add(p, p.building.transformGroup);
        });
    }
    /// <summary>
    /// 复原所有building的数据
    /// </summary>
    public void RecoveryBuildingDatas()
    {
        foreach (var p in itemDataDic)
        {
            p.Value.Inject(p.Key.building.transform);
        }
    }
    /// <summary>
    /// 选择目标，物体或UI
    /// </summary>
    /// <param name="item"></param>
    private void Select(Item item)
    {
        LastRecovery();
        if (item == null)
        {
            Debug.Log("null");
            return;
        }
        item.building.ShowHighLight();
        item.background.color = selectColor;
        PanelControl.current.SetData(item.building);
        Coordinate.Target.SetTarget(item.building.transform);
        selectItem = item;
    }
    #endregion
}