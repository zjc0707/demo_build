using System.Collections.Generic;
using UnityEngine;
public class UIStack : Stack<GameObject>
{
    public void Peek(bool isActive)
    {
        if (base.Count == 0) return;
        base.Peek().SetActive(isActive);
    }
    public new void Push(GameObject obj)
    {
        this.Peek(false);
        obj.SetActive(true);
        base.Push(obj);
    }
    public new void Pop()
    {
        if (base.Count == 0) return;
        base.Pop().SetActive(false);
        this.Peek(true);
    }
    public new void Clear()
    {
        if (base.Count == 0) return;
        this.Peek(false);
        base.Clear();
    }
}