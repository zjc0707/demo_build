﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : BaseObject<Building>
{
    /// <summary>
    /// 基于世界坐标
    /// </summary>
    public Vector3 LeftFrontBottom
    {
        get
        {
            int a = ((int)Mathf.Abs(this.transform.eulerAngles.y / 90)) % 2;
            if (a == 0)
            {
                return this.transform.position - posToLeftFront;
            }
            else
            {
                return this.transform.position - new Vector3(posToLeftFront.z, posToLeftFront.y, posToLeftFront.x);
            }
        }
    }
    private Dictionary<Renderer, Material[]> dicMaterial;
    private Vector3 posToLeftFront;

    /// <summary>
    /// 记录上一个被选中的物体
    /// </summary>
    public static Building LastTarget { get; private set; }
    /// <summary>
    /// 复原上一个被选中的物体，不存在则直接return
    /// </summary>
    public static void LastRecovery()
    {
        if (LastTarget == null)
        {
            return;
        }
        LastTarget.Recovery();
        LastTarget = null;
    }

    /// <summary>
    /// 选中物体，恢复上一个被选中的物体，替换选中物体的材质
    /// </summary>
    public void Choose()
    {
        LastRecovery();
        SetMaterial(MaterialStatic.BLUE);

        LastTarget = this;
    }

    public void Build()
    {
        Recovery();
        BuildingRoom.current.Add(this);
        this.AdjustPosition();
    }
    public void AdjustPosition()
    {
        BuildingUtil.AdjustPosition(this);
        base.DownToFloor();
    }
    /// <summary>
    /// 复原物体材质
    /// </summary>
    private void Recovery()
    {
        RecovercyMaterial();
    }

    /// <summary>
    /// 设置在awake中记录的renderer的材质
    /// </summary>
    /// <param name="material"></param>
    private void SetMaterial(Material material)
    {
        foreach (Renderer renderer in dicMaterial.Keys)
        {
            //下标替换无效，需整体数组替换
            Material[] materials = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < materials.Length; ++i)
            {
                materials.SetValue(material, i);
            }
            renderer.sharedMaterials = materials;
        }
    }

    private void RecovercyMaterial()
    {
        foreach (var t in dicMaterial)
        {
            t.Key.sharedMaterials = t.Value;
        }
    }
    /// <summary>
    /// 加载材质到dic中
    /// </summary>
    private void LoadDicMaterial()
    {
        dicMaterial = new Dictionary<Renderer, Material[]>();
        foreach (Renderer renderer in this.GetComponentsInChildren<Renderer>())
        {
            dicMaterial.Add(renderer, renderer.sharedMaterials);
        }
    }
    /// <summary>
    /// 计算size，并算出坐标与左上角的差值。
    /// </summary>
    private void LoadSizeAndPosToLeftFront()
    {
        // Debug.Log(string.Format("center:{0},position:{1}", centerAndSize.Center, this.transform.localPosition));
        if (base.centerAndSize.Center != this.transform.position)
        {
            Debug.LogWarning("中心与坐标不相等:" + this.transform.name);
            //throw new System.Exception("中心与坐标不相等:" + this.transform.name);
        }
        Vector3 dValue = this.transform.position - base.centerAndSize.Center;
        posToLeftFront = base.centerAndSize.Size / 2 + dValue;
    }

    private void Awake()
    {
        LoadDicMaterial();
        LoadSizeAndPosToLeftFront();
        BuildingUtil.AddBoxCollider(this);
    }

    public virtual void MyUpdate()
    {

    }
}