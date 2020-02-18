using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class Building : BaseObject
{
    /// <summary>
    /// 唯一标识符，不写入数据库仅用于缓存，在BuildingUtil中创建Building时赋值
    /// </summary>
    public int guid;
    public int modelDataId;
    /// <summary>
    /// 场景动画
    /// </summary>
    public List<AnimData> normalAnimDatas;
    public bool isAnimOn;
    /// <summary>
    /// 出场动画
    /// </summary>
    public List<AnimData> appearanceAnimDatas;
    /// <summary>
    /// 高亮射线的集合
    /// </summary>
    private List<Outline> outlineList;
    public BoxCollider boxCollider;
    protected float downToFloorY = float.MaxValue;
    public Vector3 Size
    {
        get
        {
            if (boxCollider == null)
            {
                Vector3 pos = this.transform.position;
                boxCollider = BuildingUtil.AddBoxCollider(this.gameObject);
                this.transform.position = pos;
            }
            Vector3 size = boxCollider.size;
            return size;
        }
    }
    /// <summary>
    /// 记录该物体的全部材质，用于替换及复原
    /// </summary>
    private Dictionary<Renderer, Material[]> dicMaterial;
    /// <summary>
    /// 放置该物体
    /// </summary>
    public void Build()
    {
        HideHighLight();
        PanelList.current.Add(this);
        // this.AdjustPosition();
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
    private Material material;
    public bool isChangeMaterial;
    public Material Material
    {
        set
        {
            isChangeMaterial = true;
            material = value;
            foreach (Renderer renderer in dicMaterial.Keys)
            {
                //下标替换无效，需整体数组替换
                Material[] materials = new Material[renderer.sharedMaterials.Length];
                for (int i = 0; i < materials.Length; ++i)
                {
                    materials.SetValue(value, i);
                }
                renderer.sharedMaterials = materials;
            }
        }
        get
        {
            if (material == null)
            {
                material = dicMaterial.ElementAt(0).Key.sharedMaterials[0];
            }
            return material;
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
        isChangeMaterial = false;
        material = null;
    }
    /// <summary>
    /// 根据centerAndSize将物体落到地面上
    /// </summary>
    public void DownToFloor()
    {
        if (downToFloorY == float.MaxValue)
        {
            throw new System.Exception("未计算downToFloorY");
        }
        Vector3 pos = this.transform.position;
        this.transform.position = new Vector3(pos.x, downToFloorY, pos.z);
    }
    /// <summary>
    /// 判断是否超出floor范围
    /// </summary>
    /// <returns>模型的最大边或int.MinValue</returns>
    public int IsTooBig()
    {
        float max = Math.Max(Size.x, Size.z);
        int maxBuilding = (int)max + 1;
        int minFloor = Math.Min(Floor.current.x, Floor.current.z);
        return (maxBuilding > minFloor) ? maxBuilding : int.MinValue;
    }
    public void ShowHighLight()
    {
        Debug.Log("Show:" + guid);
        MyCamera.current.OutlineEffect.enabled = true;
        foreach (Outline outline in outlineList)
        {
            // outline.enabled = true;
            outline.eraseRenderer = false;
        }
    }
    public void HideHighLight()
    {
        Debug.Log("Hide:" + guid);
        Debug.Log("count:" + outlineList.Count);
        foreach (Outline outline in outlineList)
        {
            // outline.enabled = false;
            outline.eraseRenderer = true;
        }
        MyCamera.current.OutlineEffect.enabled = false;
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
    private void LoadDownToFloorY()
    {
        float d = (boxCollider.center - Size / 2).y - (Floor.current.transform.position.y + FloorTile.thickness / 2);
        downToFloorY = this.transform.position.y - d;
    }
    private void Awake()
    {
        LoadDicMaterial();
        boxCollider = BuildingUtil.AddBoxCollider(this.gameObject);
        LoadDownToFloorY();
        outlineList = new List<Outline>();
        normalAnimDatas = new List<AnimData>();
        appearanceAnimDatas = new List<AnimData>();
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
