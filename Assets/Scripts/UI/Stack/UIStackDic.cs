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
    public static void Open(int key, GameObject obj)
    {
        if (!stackDic.ContainsKey(key))
        {
            stackDic.Add(key, new UIStack());
        }
        stackDic[key].Push(obj);
    }
}