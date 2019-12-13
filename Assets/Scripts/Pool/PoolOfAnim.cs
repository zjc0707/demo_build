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
        float deltaTime = Time.deltaTime;
        //并行
        if (itemList.Count > 0)
        {
            for (int i = itemList.Count - 1; i >= 0; i--)
            {
                if (itemList[i].Update(deltaTime))
                {
                    itemList.RemoveAt(i);
                }
            }
        }
        //串行
        while (itemQueue.Count > 0 && itemQueue.Peek().surplusTime <= deltaTime)
        {
            deltaTime -= itemQueue.Peek().surplusTime;
            itemQueue.Dequeue().Finish();
        }
        if (itemQueue.Count > 0)
        {
            itemQueue.Peek().Update(Time.deltaTime);
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
        public float surplusTime { get { return amountTime - useTime; } }
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
        }
    }
}