using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通过钩子上的触发器（BoxCollider-IsTrigger）,获取接触物体，进行拾取释放等操作
/// BoxCollider由于通过脚本计算有一定误差，直接手动添加
/// </summary>
public class Hook : MonoBehaviour
{
    public CenterAndSize centerAndSize;
    public Transform target;
    public Rigidbody targetRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        // centerAndSize = CenterAndSizeUtil.Get(this.transform);
        // Debug.Log(centerAndSize);
        // BoxCollider boxCollider = this.gameObject.AddComponent<BoxCollider>();
        // boxCollider.center = centerAndSize.Center;
        // boxCollider.size = centerAndSize.Size;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && target != null)
        {
            if (!target.parent.Equals(this.transform))
            {
                //拾取后设为子物体并关掉刚体受力
                target.parent = this.transform;
                targetRigidbody.isKinematic = true;
            }
            else
            {
                //释放后还给BuildingRoom，开起刚体受力由重力使物体降落
                target.parent = BuildingRoom.current.transform;
                targetRigidbody.isKinematic = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter:" + other.name);
        target = other.transform;
        targetRigidbody = target.GetComponent<Rigidbody>();
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit:" + other.name);
        if (other.transform.Equals(target))
        {
            target = null;
            targetRigidbody = null;
        }
    }
}
