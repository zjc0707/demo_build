using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public static class UIStackDic
{
    public static Dictionary<int, UIStack> stackDic = new Dictionary<int, UIStack>();
    public static void Close(int key)
    {
        stackDic[key].Pop();
    }
    /// <summary>
    /// 关闭当前并清除整个栈
    /// </summary>
    /// <param name="key"></param>
    public static void Clear(int key)
    {
        stackDic[key].Clear();
    }
    public static void Open(int key, GameObject obj)
    {
        if (!stackDic.ContainsKey(key))
        {
            stackDic.Add(key, new UIStack());
        }
        stackDic[key].Push(obj);
    }
}