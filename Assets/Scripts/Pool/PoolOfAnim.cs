using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
public class PoolOfAnim : BaseUniqueObject<PoolOfAnim>
{
    /// <summary>
    /// 并行
    /// </summary>
    private List<Item> itemList = new List<Item>();
    /// <summary>
    /// 串行
    /// </summary>
    private Queue<Item> itemQueue = new Queue<Item>();
    private void Update()
    {
        if (itemList.Count > 0)
        {
            for (int i = itemList.Count - 1; i >= 0; i--)
            {
                if (itemList[i].Update(Time.deltaTime))
                {
                    itemList.RemoveAt(i);
                }
            }
        }
        if (itemQueue.Count > 0)
        {
            if (itemQueue.Peek().Update(Time.deltaTime))
            {
                itemQueue.Dequeue();
            }
        }
    }
    public void AddList(float amountTime, Action<float> action)
    {
        itemList.Add(new Item() { amountTime = amountTime, action = action });
    }
    public void AddQueue(float amountTime, Action<float> action)
    {
        itemQueue.Enqueue(new Item() { amountTime = amountTime, action = action });
    }
    class Item
    {
        public Action<float> action;
        public float amountTime;
        public float useTime = 0;
        public bool Update(float t)
        {
            useTime += t;
            if (useTime > amountTime)
            {
                action(1);
                return true;
            }
            else
            {
                action(useTime / amountTime);
                return false;
            }
        }
    }
}