using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 操作者脚本，通过键盘操作
/// </summary>
public class Person : BaseUniqueObject<Person>
{
    private Transform target, model;

    private void Start()
    {
        target = transform.Find("Target");
        model = transform.Find("Model");
    }

    private void Update()
    {
        if (!State.current.IsPerson)
        {
            return;
        }
        //放置
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (target.childCount == 0)
            {
                return;
            }
            Building building = target.GetChild(0).GetComponent<Building>();
            if (building == null)
            {
                return;
            }
            building.Build();
        }
    }

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
        t.localPosition += Vector3.forward * offsetZ;
        float offsetY = targetCS.Size.y / 2 - targetCS.Center.y;
        t.localPosition += Vector3.up * offsetY;

        transform.localEulerAngles = euler;
    }

    private void ClearTarget()
    {
        foreach (Transform t in target)
        {
            DestroyImmediate(t.gameObject);
        }
    }

}
