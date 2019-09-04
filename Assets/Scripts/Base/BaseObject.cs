using UnityEngine;
public abstract class BaseObject<T> : MonoBehaviour where T : MonoBehaviour
{
    private CenterAndSize _centerAndSize, _centerAndSizeChangeSizeXZ;
    public CenterAndSize centerAndSize
    {
        get
        {
            if (_centerAndSize == null)
            {
                Vector3 euler = this.transform.eulerAngles;
                this.transform.eulerAngles = Vector3.zero;
                _centerAndSize = CenterAndSizeUtil.Get(this.transform);
                _centerAndSizeChangeSizeXZ = _centerAndSize.ChangeSizeXZ();
                this.transform.eulerAngles = euler;
            }
            if (!isRotate)
            {
                return _centerAndSize;
            }
            else
            {
                return _centerAndSizeChangeSizeXZ;
            }
        }
    }
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
            return new TransformGroup(this.transform.position, this.transform.eulerAngles);
        }
    }
    /// <summary>
    /// floor的y值，用于落至地面计算
    /// </summary>
    private readonly float floorY = 0f;
    private float downToFloorY = float.MaxValue;
    /// <summary>
    /// 根据centerAndSize将物体落到地面上
    /// </summary>
    public void DownToFloor()
    {
        // float distance = centerAndSize.Center.y - centerAndSize.Size.y / 2 - floorY;
        // this.transform.position -= Vector3.up * distance;
        if (downToFloorY == float.MaxValue)
        {
            downToFloorY = centerAndSize.Size.y / 2 + floorY + FloorTile.current.thickness / 2;
        }
        Vector3 pos = this.transform.position;
        this.transform.position = new Vector3(pos.x, downToFloorY, pos.z);
    }
    public Vector3 DownToFloorPos()
    {
        Vector3 pos = this.transform.position;
        return new Vector3(pos.x, downToFloorY, pos.z);
    }
    /// <summary>
    /// 将物体落至地面并居于中心
    /// </summary>
    public void Reset()
    {
        this.transform.position = Floor.current.transform.position;
        DownToFloor();
    }
}