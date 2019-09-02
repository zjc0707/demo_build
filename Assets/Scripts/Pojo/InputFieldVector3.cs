using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputFieldVector3
{
    public InputField X { get; }
    public InputField Y { get; }
    public InputField Z { get; }

    public InputFieldVector3(InputField x, InputField y, InputField z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public void Set(float x, float y, float z)
    {
        X.text = x.ToString();
        Y.text = y.ToString();
        Z.text = z.ToString();
    }

    public void Set(Vector3 v)
    {
        X.text = v.x.ToString();
        Y.text = v.y.ToString();
        Z.text = v.z.ToString();
    }

    public Vector3 ToVector3()
    {
        float x = 0f, y = 0f, z = 0f;
        float.TryParse(X.text, out x);
        float.TryParse(Y.text, out y);
        float.TryParse(Z.text, out z);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// 添加监听事件
    /// </summary>
    /// <param name="call"></param>
    public void AddValueChangedListener(UnityAction<string> call)
    {
        X.onValueChanged.AddListener(call);
        Y.onValueChanged.AddListener(call);
        Z.onValueChanged.AddListener(call);
    }
    public void AddEndEditListener(UnityAction<string> call)
    {
        X.onEndEdit.AddListener(call);
        Y.onEndEdit.AddListener(call);
        Z.onEndEdit.AddListener(call);
    }

    public void RemoveValueChangedListener()
    {
        //不清楚监听事件会叠加
        X.onValueChanged.RemoveAllListeners();
        Y.onValueChanged.RemoveAllListeners();
        Y.onValueChanged.RemoveAllListeners();
    }

    public override string ToString()
    {
        try
        {
            return string.Format("[x: {0}, y: {1}, z: {2} ]", X.text, Y.text, Z.text);
        }
        catch (NullReferenceException e)
        {
            return e.Message;
        }
    }
}
