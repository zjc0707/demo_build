using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[UIType(UIStackType.LEFT)]
public class PanelList : BasePanel<PanelList>
{
    #region UI对象
    public Transform content;
    public Transform baseItem;
    public List<Transform> items;
    public VerticalLayoutGroup verticalLayoutGroup;
    public const int itemHeight = 30;
    #endregion
    #region 长按
    private Vector3 mouseToItem, itemStartPosition;
    private int itemSiblingIndex;
    /// <summary>
    /// 按下计时
    /// /// </summary>
    private float downTime = 0f;
    /// <summary>
    /// 长按计时
    /// </summary>
    private float longDownTime = 0f;
    /// <summary>
    /// 点击/长按对象
    /// </summary>
    private Transform selectItem;
    /// <summary>
    ///出发长按所需时间
    /// </summary>
    public const float NEED_DOWN_TIME = 0.15f;
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

        baseItem.gameObject.SetActive(true);
        for (int i = 0; i < 5; ++i)
        {
            GameObject clone = Instantiate(baseItem.gameObject, content);
            clone.name = i.ToString();
            clone.GetComponentInChildren<Text>().text = i.ToString();
            EventTriggerListener.Get(clone).onDown += OnDown;
            items.Add(clone.transform);
        }
        baseItem.gameObject.SetActive(false);
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
                itemStartPosition = selectItem.position;
                mouseToItem = Input.mousePosition - selectItem.position;
                itemSiblingIndex = selectItem.GetSiblingIndex();
                Debug.Log(selectItem.GetSiblingIndex());
            }
        }
        if (state > STATE.NULL)
        {
            if (Input.GetMouseButtonUp(0))
            {
                state = STATE.NULL;
                selectItem = null;
                downTime = 0f;
                longDownTime = 0f;
                verticalLayoutGroup.enabled = true;
            }
        }
        if (state == STATE.LONG_DOWN)
        {
            // if (longDownTime < NEED_DOWN_TIME)
            // {
            //     longDownTime += Time.deltaTime;
            //     selectItem.position = new Vector3(selectItem.position.x, (Input.mousePosition - mouseToItem).y, selectItem.position.z);
            // }
            // else
            // {
            //     selectItem.position = new Vector3(selectItem.position.x, Input.mousePosition.y, selectItem.position.z);
            // }
            selectItem.position = new Vector3(selectItem.position.x, Input.mousePosition.y, selectItem.position.z);

            if (selectItem.localPosition.y > 0)
            {
                selectItem.localPosition = new Vector3(selectItem.localPosition.x, 0, selectItem.localPosition.z);
            }
            else if (selectItem.localPosition.y < -1 * (items.Count - 1) * itemHeight)
            {
                selectItem.localPosition = new Vector3(selectItem.localPosition.x, -1 * (items.Count - 1) * itemHeight, selectItem.localPosition.z);
            }

            float add = selectItem.position.y - itemStartPosition.y;
            if (Math.Abs(add) >= itemHeight)//移动距离大于UI高度时
            {
                //根据移动的上下确定nextItem的位置，将items中二者位置调换，
                //UI中将selectItem的初始坐标赋值nextItem
                int nextIndex = itemSiblingIndex - (add > 0 ? 1 : -1);
                Transform t = items[nextIndex - 1];//getSiblingIndex()从1开始
                items.RemoveAt(itemSiblingIndex - 1);
                items.Insert(nextIndex - 1, selectItem);
                Vector3 pos = t.position;
                t.position = itemStartPosition;
                itemStartPosition = pos;

                itemSiblingIndex = nextIndex;
                selectItem.SetSiblingIndex(nextIndex);
            }
        }
    }
    private void OnDown(GameObject go)
    {
        Debug.Log("down:" + go.name);
        selectItem = go.transform;
        state = STATE.DOWN;
    }
}