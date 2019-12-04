using UnityEngine;
public abstract class BaseObject : MonoBehaviour
{
    /// <summary>
    /// 判断物体是否旋转影响长宽，false：0或180度，true：90或270
    /// </summary>
    public bool isRotate
    {
        get
        {
            return ((int)Mathf.Abs(this.transform.eulerAngles.y / 90)) % 2 == 1;
        }
    }
    public TransformGroup transformGroup
    {
        get
        {
            return new TransformGroup(this.transform);
        }
    }
    /// <summary>
    /// floor的y值，用于落至地面计算
    /// </summary>
    protected readonly float floorY = 0f;
    protected float downToFloorY = float.MaxValue;
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
    public Vector3 DownToFloorPos()
    {
        Vector3 pos = this.transform.position;
        return new Vector3(pos.x, downToFloorY, pos.z);
    }
}