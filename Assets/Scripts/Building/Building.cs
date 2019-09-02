using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : BaseObject<Building>
{
    public PanelControllerItem data;
    public new Rigidbody rigidbody;
    /// <summary>
    /// 基于世界坐标
    /// </summary>
    public Vector3 LeftBackBottom
    {
        get
        {
            int a = ((int)Mathf.Abs(this.transform.eulerAngles.y / 90)) % 2;
            if (a == 0)
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
    /// <summary>
    /// 放置该物体
    /// </summary>
    public void Build()
    {
        Recovery();
        BuildingRoom.current.Add(this);
        this.AdjustPosition();
    }
    /// <summary>
    /// 通过工具类调整物体坐标
    /// </summary>
    public void AdjustPosition()
    {
        BuildingUtil.AdjustPosition(this);
        base.DownToFloor();
    }
    public void Rotate90()
    {
        this.transform.Rotate(transform.up, 90f);
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
    /// <summary>
    /// 通过dic的value项进行复原材质
    /// </summary>
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
        posToLeftBackBottom = base.centerAndSize.Size / 2 + dValue;
    }

    private void Awake()
    {
        LoadDicMaterial();
        LoadSizeAndPosToLeftFront();
        BuildingUtil.AddBoxCollider(this);
        rigidbody = this.gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    public virtual void MyUpdate()
    {
        MoveAndRotateUtil.Update(this.transform);
    }
}
