using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class Building : BaseObject
{
    public ModelData data;
    public BoxCollider boxCollider;
    /// <summary>
    /// 物体对象是否锁定，用于移动或修改数据面板时是否自动定位
    /// </summary>
    public bool isLock = true;
    /// <summary>
    /// 高亮射线的集合
    /// </summary>
    private List<Outline> outlineList;
    /// <summary>
    /// 基于世界坐标
    /// </summary>
    public Vector3 LeftBackBottom
    {
        get
        {
            if (!base.isRotate)
            {
                return this.transform.position - posToLeftBackBottom;
            }
            else
            {
                return this.transform.position - new Vector3(posToLeftBackBottom.z, posToLeftBackBottom.y, posToLeftBackBottom.x);
            }
        }
    }
    /// <summary>
    /// 记录该物体的全部材质，用于替换及复原
    /// </summary>
    private Dictionary<Renderer, Material[]> dicMaterial;
    /// <summary>
    /// 初始化时计算出物体中心到左上角到距离差，后续调整位置时直接使用，减少计算量
    /// </summary>
    private Vector3 posToLeftBackBottom;



    /// <summary>
    /// 选中物体，恢复上一个被选中的物体，替换选中物体的材质
    /// </summary>
    public void Choose()
    {
        BuildingHelper.LastRecovery();
        ShowHighLight();

        BuildingHelper.LastTarget = this;
    }
    /// <summary>
    /// 复原物体材质
    /// </summary>
    public void Recovery()
    {
        HideHighLight();
    }
    /// <summary>
    /// 放置该物体
    /// </summary>
    public void Build()
    {
        Recovery();
        BuildingRoom.current.Add(this);
        // this.AdjustPosition();
    }
    /// <summary>
    /// 通过工具类调整物体坐标,未锁定则自由移动
    /// </summary>
    public void AdjustPosition()
    {
        if (!isLock) return;
        BuildingUtil.AdjustPosition(this);
        // base.DownToFloor();
    }
    public void Rotate90()
    {
        this.transform.Rotate(transform.up, 90f);
    }
    /// <summary>
    /// 设置在awake中记录的renderer的材质
    /// </summary>
    /// <param name="material"></param>
    public void SetMaterial(Material material)
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
    /// <summary>
    /// 通过dic的value项进行复原材质
    /// </summary>
    public void RecovercyMaterial()
    {
        foreach (var t in dicMaterial)
        {
            t.Key.sharedMaterials = t.Value;
        }
    }
    private void ShowHighLight()
    {
        foreach (Outline outline in outlineList)
        {
            outline.enabled = true;
        }
    }
    private void HideHighLight()
    {
        foreach (Outline outline in outlineList)
        {
            outline.enabled = false;
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
            // Debug.LogWarning("中心与坐标不相等:" + this.transform.name + "-" + transform.position + "-" + centerAndSize.Center);

            //throw new System.Exception("中心与坐标不相等:" + this.transform.name);
        }
        Vector3 dValue = this.transform.position - base.centerAndSize.Center;
        posToLeftBackBottom = base.centerAndSize.Size / 2 + dValue;

        Debug.Log(LeftBackBottom.y);
        float d = LeftBackBottom.y - (base.floorY + FloorTile.current.thickness / 2);
        base.downToFloorY = this.transform.position.y - d;
    }
    /// <summary>
    /// 判断是否超出floor范围
    /// </summary>
    /// <returns>模型的最大边或int.MinValue</returns>
    public int IsTooBig()
    {
        Debug.Log(centerAndSize.Size);
        float max = Math.Max(centerAndSize.Size.x, centerAndSize.Size.z);
        int maxBuilding = (int)max + 1;
        int minFloor = Math.Min(Floor.current.x, Floor.current.z);
        Debug.Log(maxBuilding + "-" + minFloor);
        return (maxBuilding > minFloor) ? maxBuilding : int.MinValue;
    }

    private void Awake()
    {
        LoadDicMaterial();
        boxCollider = BuildingUtil.AddBoxCollider(this);
        LoadSizeAndPosToLeftFront();
        outlineList = new List<Outline>();
        foreach (MeshRenderer mesh in this.transform.GetComponentsInChildren<MeshRenderer>())
        {
            outlineList.Add(mesh.gameObject.AddComponent<Outline>());
        }
        HideHighLight();
    }

    /// <summary>
    /// 默认的操控移动方法
    /// </summary>
    public virtual void MyUpdate()
    {
        MoveAndRotateUtil.Update(this.transform);
    }
}
