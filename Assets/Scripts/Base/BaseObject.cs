using UnityEngine;
public class BaseObject<T> : MonoBehaviour where T : MonoBehaviour
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
    public readonly float floorY = 0f;
    public void DownToFloor()
    {
        float distance = centerAndSize.Center.y - centerAndSize.Size.y / 2 - floorY;
        this.transform.position -= Vector3.up * distance;
    }
    public void Reset()
    {
        this.transform.position = Floor.current.transform.position;
        DownToFloor();
    }
}