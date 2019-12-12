using System.Collections.Generic;
using System;
using UnityEngine;
public class Coordinate : BaseUniqueObject<Coordinate>
{
    private List<Item> items;
    /// <summary>
    /// 根据相机距离调整大小时的比例
    /// </summary>
    private const float multiple = 2f;
    /// <summary>
    /// 移动物体对象
    /// </summary>
    private Transform target;
    private Item hit;
    private void Start()
    {
        items = new List<Item>() { FindItem("X"), FindItem("Y"), FindItem("Z") };
        float posZ = items[2].line.GetComponent<MeshRenderer>().bounds.size.z;
        items.ForEach(i => i.SetHeadPosZ(posZ));
    }
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
        }
        return flag;
    }
    /// <summary>
    /// 恢复点击的变化
    /// </summary>
    public void Recovery()
    {
        items.ForEach(i => i.Recovery());
    }
    public void SetTarget(Transform t)
    {
        target = t;
        if (t == null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        this.gameObject.SetActive(true);
        this.transform.position = t.position;
        this.transform.eulerAngles = t.eulerAngles;
    }
    public void ChangeSizeByDistanceToCamera()
    {
        float distance = Math.Abs(Vector3.Distance(MyCamera.current.transform.position, this.transform.position)) / multiple;
        items.ForEach(i => i.SetScale(distance));
        this.transform.localScale = Vector3.one * distance;
    }
    public void Move(Vector3 pos, Vector3 add)
    {
        if (hit != null)
        {
            this.transform.position = pos + hit.Project(add);
            target.position = this.transform.position;
        }
    }
    private Item FindItem(string name)
    {
        Transform t = this.transform.Find(name);
        return new Item(t);
    }
    class Item
    {
        public Transform target { get; set; }
        public Transform line { get; set; }
        public Transform head { get; set; }
        private MeshRenderer mrLine, mrHead;
        private Material defaultMaterial;
        private float defaultLineXY = 0.0005f;
        private float defaultLineZ = 0.01f;
        public Item(Transform t)
        {
            target = t;
            Debug.Log(t.name + "    " + t.forward);
            line = t.Find("Line");
            head = t.Find("Head");
            mrLine = line.GetComponent<MeshRenderer>();
            mrHead = head.GetComponent<MeshRenderer>();
            defaultMaterial = mrLine.sharedMaterial;
        }
        public Vector3 Project(Vector3 v)
        {
            return Vector3.Project(v, target.forward);
        }
        public void SetHeadPosZ(float f)
        {
            head.localPosition = new Vector3(0, 0, f);
        }
        public void SetScale(float f)
        {
            line.localScale = new Vector3(defaultLineXY / f, defaultLineXY / f, defaultLineZ);
        }
        public void SetMaterial(Material m)
        {
            mrLine.sharedMaterial = m;
            mrHead.sharedMaterial = m;
        }
        public void Recovery()
        {
            mrLine.sharedMaterial = defaultMaterial;
            mrHead.sharedMaterial = defaultMaterial;
        }
    }
}