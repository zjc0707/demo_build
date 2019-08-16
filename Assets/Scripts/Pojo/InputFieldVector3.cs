using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
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

    public override string ToString()
    {
        try
        {
            return string.Format("[x: {0}, y: {1}, z: {2} ]", X.text, Y.text, Z.text);
        }catch(NullReferenceException e)
        {
            return e.Message;
        }
    }
}
