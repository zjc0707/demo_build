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
    public Building targetBuilding;
    private TimeGroup downTimeGroup = new TimeGroup(0.5f);
    private Vector3 startPos, endPos;
    private bool isDown = false;
    // public Rigidbody targetRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && target != null
             && !isDown)
        {
            //拾取
            if (!target.parent.Equals(this.transform))
            {
                //拾取后设为子物体并关掉刚体受力
                target.parent = this.transform;
                target.position += Vector3.up * 0.1f;
                // targetRigidbody.isKinematic = true;
            }
            //丢弃
            else
            {
                //释放后还给BuildingRoom，开起刚体受力由重力使物体降落
                target.parent = BuildingRoom.current.transform;
                // targetRigidbody.isKinematic = false;
                startPos = target.position;
                endPos = targetBuilding.DownToFloorPos();
                downTimeGroup.UseToZero();
                isDown = true;
            }
        }
        if (isDown)
        {
            Down(Time.deltaTime);
            if (!isDown)
            {
                target = null;
                targetBuilding = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        target = other.transform;
        targetBuilding = target.GetComponent<Building>();
        //只有building对象可以抓的起来
        if (targetBuilding == null)
        {
            target = null;
        }
        else
        {
            Debug.Log("enter:" + other.name);
        }
        // targetRigidbody = target.GetComponent<Rigidbody>();

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit:" + other.name);
        if (other.transform.Equals(target))
        {
            //target = null;
            // targetRigidbody = null;
        }
    }

    private void Down(float deltaTime)
    {
        isDown = downTimeGroup.Add(deltaTime);
        target.position = Vector3.Lerp(startPos, endPos, downTimeGroup.rate);
    }
}
