﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
public class MyCamera : BaseUniqueObject<MyCamera>
{

    public readonly float y = 10;
    public readonly TransformGroup initTransformGroup = new TransformGroup(new Vector3(0, 5, 0), new Vector3(45, 45, 0), Vector3.one);
    private Camera _camera;
    public Camera Camera
    {
        get
        {
            if (_camera == null)
            {
                _camera = this.gameObject.GetComponent<Camera>();
            }
            return _camera;
        }
    }
    // private OutlineEffect outlineEffect;
    // public OutlineEffect OutlineEffect
    // {
    //     get
    //     {
    //         if (outlineEffect == null)
    //         {
    //             outlineEffect = this.transform.GetComponent<OutlineEffect>();
    //         }
    //         return outlineEffect;
    //     }
    // }
    // Use this for initialization
    void Awake()
    {
        this.Reset();
    }
    public void Reset()
    {
        MoveAnim(initTransformGroup);
        // OutlineEffect.enabled = false;
    }
    public void MoveAnim(Vector3 position)
    {
        TransformGroup t = base.transformGroup;
        t.Position = position;
        MoveAnim(t);
    }
    public void MoveAnim(TransformGroup end)
    {
        TransformGroup start = base.transformGroup;
        PoolOfAnim.current.AddItem(0.5f, f =>
        {
            this.transform.position = Vector3.Lerp(start.Position, end.Position, f);
            this.transform.eulerAngles = Vector3.Lerp(start.EulerAngles, end.EulerAngles, f);
        });
    }
}
