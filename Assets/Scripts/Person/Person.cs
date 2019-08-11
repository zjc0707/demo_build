using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Person : MonoBehaviour
{
    private static Person _instance;
    public static Person current
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Person>();
            }
            if(_instance == null)
            {
                _instance = GameObject.Find("Person").AddComponent<Person>();
            }
            return _instance;
        }
    }

    private Transform target, model;
    
    public void Catch(Transform t)
    {
        Debug.Log("Catch");
        Vector3 euler = transform.localEulerAngles;
        transform.localEulerAngles = Vector3.zero;

        ClearTarget();
        t.SetParent(target);
        t.localPosition = Vector3.zero;
        t.localEulerAngles = Vector3.zero;

        target.localPosition = Vector3.zero;
        CenterAndSize targetCS = CenterAndSizeUtil.Get(target);
        CenterAndSize modelCS = CenterAndSizeUtil.Get(model);
        float offsetZ = (targetCS.Size.z + modelCS.Size.z) / 2;
        t.localPosition +=  Vector3.forward * offsetZ;
        float offsetY = targetCS.Size.y / 2 - targetCS.Center.y;
        t.localPosition += Vector3.up * offsetY;

        transform.localEulerAngles = euler;
    }

    private void ClearTarget()
    {
        foreach(Transform t in target)
        {
            DestroyImmediate(t.gameObject);
        }
    }

    private void Start()
    {
        target = transform.Find("Target");
        model = transform.Find("Model");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if(target.childCount > 0)
            {
                Buildings.current.Build(target.GetChild(0));
            }
        }
    }

}
