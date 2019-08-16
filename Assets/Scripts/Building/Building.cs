using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private Dictionary<Renderer, Material> dicMaterial;
    /// <summary>
    /// 记录上一个被选中的物体
    /// </summary>
    private static Building lastTarget;
    public static void LastRecovery()
    {
        if (lastTarget == null)
        {
            return;
        }
        lastTarget.Recovery();
    }

    private void Awake()
    {
        dicMaterial = new Dictionary<Renderer, Material>();
        foreach (Renderer renderer in this.GetComponentsInChildren<Renderer>())
        {
            dicMaterial.Add(renderer, renderer.sharedMaterial);
        }
    }

    public void Choose()
    {
        LastRecovery();
        SetMaterial(MaterialStatic.TRANSLUCENT_BLUE);

        lastTarget = this;
    }

    public void Recovery()
    {
        RecovercyMaterial();
    }

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
}
