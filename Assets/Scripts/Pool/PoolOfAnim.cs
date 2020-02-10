using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
public class PoolOfAnim : BaseUniqueObject<PoolOfAnim>
{
    /// <summary>
    /// 是否为观赏模式，观赏模式下进行itemQueueList的动画
    /// </summary>
    private bool isViewModel;
    /// <summary>
    /// 单向动画，如相机的复位移动
    /// 播放完不会恢复
    /// </summary>
    private List<Item> itemList = new List<Item>();
    /// <summary>
    /// 串行单节点（多节点）动画，如单物体动画列表的播放、单个动画的播放
    /// 播放完会回到最初位置
    /// </summary>
    private MyQueue<Item> itemQueue = new MyQueue<Item>();
    /// <summary>
    /// 并行循环多节点动画，浏览整体时并行播放不同物体的动画
    /// </summary>
    private Dictionary<int, MyQueue<Item>> itemQueueDic = new Dictionary<int, MyQueue<Item>>();
    /// <summary>
    /// 添加并行循环动画列表
    /// </summary>
    /// <param name="target">目标物体</param>
    /// <param name="animDatas">动画列表</param>
    /// <returns>下标</returns>
    public void AddItemQueueInDic(int key, Transform target, List<AnimData> animDatas)
    {
        MyQueue<Item> items = new MyQueue<Item>();
        animDatas.ForEach(data => items.Add(new Item()
        {
            amountTime = data.Duration,
            action = f => data.Lerp(target, f)
        }));
        itemQueueDic.Add(key, items);
    }
    /// <summary>
    /// 移除集合中的动画并恢复初始状态,用于循环动画的取消
    /// </summary>
    /// <param name="key"></param>
    public bool RemoveItemQueueInDic(int key)
    {
        MyQueue<Item> items;
        if (itemQueueDic.TryGetValue(key, out items))
        {
            items[0].Restart();
            itemQueueDic.Remove(key);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 添加单节点动画,动画结束不会复原
    /// </summary>
    /// <param name="amountTime">时长</param>
    /// <param name="action">动画事件</param>
    /// <param name="result"></param>
    public void AddItem(float amountTime, Action<float> action, Action result = null)
    {
        // itemQueue.Clear();
        if (result != null)
        {
            AddQueue(amountTime, f =>
            {
                action(f);
                if (f >= 1)
                {
                    result();
                }
            });
            AddQueue(0.001f, f => result());
        }
        else
        {
            AddQueue(amountTime, action);
        }
    }
    // /// <summary>
    // /// 添加单节点动画，动画结束不会复原
    // /// </summary>
    // /// <param name="amountTime">时长</param>
    // /// <param name="action">动画事件</param>
    // /// <param name="result"></param>
    // public void AddItemNoRecovery(float amountTime, Action<float> action, Action result = null)
    // {
    //     itemList.Add(new Item()
    //     {
    //         amountTime = amountTime,
    //         action = f =>
    //         {
    //             action(f);
    //             if (f >= 1 && result != null)
    //             {
    //                 result();
    //             }
    //         }
    //     });
    // }
    /// <summary>
    /// 根据animDatas添加多节点动画并显示物体，若animDatas为空则直接显形
    /// </summary>
    /// <param name="target"></param>
    /// <param name="animDatas"></param>
    /// <param name="result"></param>
    public void AddItemInQueue(Transform target, List<AnimData> animDatas, Action result = null)
    {
        // itemQueue.Clear();
        if (animDatas.Count > 0)
        {
            int i = 0;
            animDatas.ForEach(data => AddQueue(data.Duration, f =>
            {
                if ((i++) == 0)
                {
                    target.gameObject.SetActive(true);
                }
                data.Lerp(target, f);
                Coordinate.Target.SetTarget(target);
            }));
        }
        else
        {
            AddQueue(0.001f, f => { target.gameObject.SetActive(true); });
        }
        if (result != null)
        {
            AddQueue(0.001f, f => result());
        }
    }
    public void ViewModelTurnOn()
    {
        isViewModel = true;
    }
    public void ViewModelTurnOff()
    {
        isViewModel = false;
        foreach (MyQueue<Item> p in itemQueueDic.Values)
        {
            p[0].Restart();
        }
    }
    /// <summary>
    /// 获取PanelList的items，并全部隐藏，通过动画一一显示
    /// </summary>
    public void PlayAppearanceAnim()
    {
        PanelList.current.items.ForEach(item =>
        {
            item.building.gameObject.SetActive(false);
            AddItemInQueue(item.building.transform, item.building.appearanceAnimDatas);
        });
    }
    /// <summary>
    /// 添加串行动画
    /// </summary>
    /// <param name="amountTime"></param>
    /// <param name="action"></param>
    private void AddQueue(float amountTime, Action<float> action)
    {
        itemQueue.Enqueue(new Item() { amountTime = amountTime, action = action });
    }
    private void Update()
    {
        float deltaTime = Time.deltaTime;
        if (!isViewModel)
        {
            // for (int i = itemList.Count - 1; i >= 0; i--)
            // {
            //     if (itemList[i].Update(deltaTime))
            //     {
            //         itemList.RemoveAt(i);
            //     }
            // }
            #region  串行，需判断amountTime小于deltaTime的情况,结束后不复原
            float deltaTimeQueue = deltaTime;
            while (itemQueue.SurplusCount > 0 && itemQueue.Peek().surplusTime <= deltaTimeQueue)
            {
                deltaTimeQueue -= itemQueue.Peek().surplusTime;
                itemQueue.Dequeue().Finish();
            }
            if (itemQueue.SurplusCount > 0)
            {
                itemQueue.Peek().Update(deltaTimeQueue);
            }
            else if (itemQueue.Count > 0)
            {
                // itemQueue[0].Restart();
                itemQueue.Clear();
            }
            #endregion
        }
        else
        {
            #region 并行多节点动画
            for (int i = itemQueueDic.Count - 1; i >= 0; i--)
            {
                float deltaTimeQueue2 = deltaTime;
                MyQueue<Item> items = itemQueueDic[i];
                while (items.Peek().surplusTime <= deltaTimeQueue2)
                {
                    deltaTimeQueue2 -= items.Peek().surplusTime;
                    items.Dequeue().Finish();
                }
                items.Peek().Update(deltaTimeQueue2);
            }
            #endregion
        }

    }
    class MyQueue<T> : List<T> where T : class
    {
        private int index;
        public int SurplusCount { get { return base.Count - index; } }
        public void Enqueue(T t)
        {
            base.Add(t);
        }
        public T Dequeue()
        {
            return base[(index++) % base.Count];
        }
        public T Peek()
        {
            return base[index % base.Count];
        }
        public new void Clear()
        {
            base.Clear();
            index = 0;
        }
    }
    class Item
    {
        public Action<float> action;
        public float amountTime;
        public float surplusTime { get { return amountTime - useTime; } }
        private float useTime = 0;
        public bool Update(float t)
        {
            useTime += t;
            if (useTime > amountTime)
            {
                Finish();
                return true;
            }
            else
            {
                action(useTime / amountTime);
                return false;
            }
        }
        public void Finish()
        {
            action(1);
            useTime = 0;
        }
        public void Restart()
        {
            action(0);
        }
    }
}