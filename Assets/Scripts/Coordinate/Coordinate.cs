using System.Collections.Generic;
using System;
using UnityEngine;
public abstract class Coordinate : BaseUniqueObject<Coordinate>
{
    private static Coordinate _target;
    public static Coordinate Target
    {
        set
        {
            if (_target != null)
            {
                value.SetTarget(_target.targetTransform);
                _target.SetTarget(null);
            }
            _target = value;
        }
        get
        {
            return _target;
        }
    }
    protected List<Item> items;
    /// <summary>
    /// 目标物体修改前的pos/rot/scale数据
    /// </summary>
    protected Vector3 beforeData;
    /// <summary>
    /// 根据相机距离调整大小时的比例
    /// </summary>
    private const float multiple = 2f;
    /// <summary>
    /// 移动物体对象
    /// </summary>
    public Transform targetTransform;
    protected Item hit;
    private void Start()
    {
        items = new List<Item>() { FindItem("X"), FindItem("Y"), FindItem("Z") };
        this.gameObject.SetActive(false);
    }
    public abstract void Change(Vector3 add);
    protected abstract Item FindItem(string name);
    protected abstract void SetScale(float distance);
    /// <summary>
    /// 判断是否点击到，若是则出现相应变化
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public bool Hit(Transform t)
    {
        bool flag = t.parent == this.transform;
        if (flag)
        {
            foreach (Item i in items)
            {
                if (i.target == t)
                {
                    flag = true;
                    hit = i;
                    i.SetMaterial(ResourceStatic.CHOOSE);
                }
                else
                {
                    i.SetMaterial(ResourceStatic.OTHER);
                }
            }
            SetBeforeData();
        }
        return flag;
    }
    protected abstract void SetBeforeData();
    /// <summary>
    /// 恢复点击的变化
    /// </summary>
    public void Recovery()
    {
        items.ForEach(i => i.Recovery());
    }
    public virtual void SetTarget(Transform t)
    {
        targetTransform = t;
        if (t == null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        this.transform.position = t.position;
        this.transform.eulerAngles = t.eulerAngles;
        this.gameObject.SetActive(true);
        ChangeSizeByDistanceToCamera();
    }
    public void ChangeSizeByDistanceToCamera()
    {
        float distance = Math.Abs(Vector3.Distance(MyCamera.current.transform.position, this.transform.position)) / multiple;
        SetScale(distance);
    }
    protected class Item
    {
        public Transform target { get; set; }
        public Transform line { get; set; }
        public MeshRenderer mrLine, mrHead;
        public Material defaultMaterial;

        public Item()
        {

        }
        public Vector3 Project(Vector3 v)
        {
            return Vector3.Project(v, target.forward);
        }
        public void SetMaterial(Material m)
        {
            if (mrLine != null)
            {
                mrLine.sharedMaterial = m;
            }
            if (mrHead != null)
            {
                mrHead.sharedMaterial = m;
            }
        }
        public void Recovery()
        {
            if (mrLine != null)
            {
                mrLine.sharedMaterial = defaultMaterial;
            }
            if (mrHead != null)
            {
                mrHead.sharedMaterial = defaultMaterial;
            }
        }
    }
}