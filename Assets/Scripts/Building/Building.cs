using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector3 Size { get; private set; }
    public Vector3 LeftFrontBottom { get
        {
            int a = ((int)Mathf.Abs(this.transform.localEulerAngles.y / 90)) % 2;
            if(a == 0)
            {
                return this.transform.localPosition - posToLeftFront;
            }
            else
            {
                return this.transform.localPosition - new Vector3(posToLeftFront.z, posToLeftFront.y, posToLeftFront.x);
            }
        } }
    private Dictionary<Renderer, Material> dicMaterial;
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
    /// 复原物体材质
    /// </summary>
    public void Recovery()
    {
        RecovercyMaterial();
    }
    /// <summary>
    /// 设置在awake中记录的renderer的材质
    /// </summary>
    /// <param name="material"></param>
    private void SetMaterial(Material material)
    {
        foreach(Renderer renderer in dicMaterial.Keys)
        {
            renderer.sharedMaterial = material;
        }
    }

    private void RecovercyMaterial()
    {
        foreach(var t in dicMaterial)
        {
            t.Key.sharedMaterial = t.Value;
        }
    }
    /// <summary>
    /// 加载材质到dic中
    /// </summary>
    private void LoadDicMaterial()
    {
        dicMaterial = new Dictionary<Renderer, Material>();
        foreach (Renderer renderer in this.GetComponentsInChildren<Renderer>())
        {
            dicMaterial.Add(renderer, renderer.sharedMaterial);
        }
    }
    /// <summary>
    /// 计算size，并算出坐标与左上角的差值。
    /// </summary>
    private void LoadSizeAndPosToLeftFront()
    {
        CenterAndSize centerAndSize = CenterAndSizeUtil.Get(this.transform);
        Size = centerAndSize.Size;
        Debug.Log(string.Format("center:{0},position:{1}", centerAndSize.Center, this.transform.localPosition));
        if (centerAndSize.Center != this.transform.localPosition)
        {
            Debug.LogWarning("中心与坐标不相等:" + this.transform.name);
            //throw new System.Exception("中心与坐标不相等:" + this.transform.name);
        }
        Vector3 dValue = this.transform.localPosition - centerAndSize.Center;
        posToLeftFront = Size / 2 + dValue;
    }

    private void Awake()
    {
        LoadDicMaterial();
        LoadSizeAndPosToLeftFront();
        
    }
}
