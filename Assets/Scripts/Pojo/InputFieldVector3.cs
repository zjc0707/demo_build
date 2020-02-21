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
    // private Vector3 data;
    public Vector3 Data
    {
        set
        {
            // Debug.Log("Set:oldData=" + data + "value=" + value);
            // data = value;
            X.text = value.x.ToString("F2");
            Y.text = value.y.ToString("F2");
            Z.text = value.z.ToString("F2");
        }
        get
        {
            // Debug.Log("Get:" + data);
            // return data;
            float x, y, z;
            float.TryParse(X.text, out x);
            float.TryParse(Y.text, out y);
            float.TryParse(Z.text, out z);
            return new Vector3(x, y, z);
        }
    }
    public bool interactable
    {
        set
        {
            X.interactable = value;
            Y.interactable = value;
            Z.interactable = value;
        }
    }
    public InputFieldVector3(InputField x, InputField y, InputField z)
    {
        X = x;
        Y = y;
        Z = z;
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
        X.onValueChanged.RemoveAllListeners();
        Y.onValueChanged.RemoveAllListeners();
        Y.onValueChanged.RemoveAllListeners();
    }
}
