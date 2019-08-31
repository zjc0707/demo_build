using UnityEngine;
public abstract class BaseObject<T> : MonoBehaviour where T : MonoBehaviour
{
    private CenterAndSize _centerAndSize;
    public CenterAndSize centerAndSize
    {
        get
        {
            if (_centerAndSize == null)
            {
                _centerAndSize = CenterAndSizeUtil.Get(this.transform);
            }
            return _centerAndSize;
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
            downToFloorY = centerAndSize.Size.y / 2 + floorY;
        }
        Vector3 pos = this.transform.position;
        this.transform.position = new Vector3(pos.x, downToFloorY, pos.z);
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