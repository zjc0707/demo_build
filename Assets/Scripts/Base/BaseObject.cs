using UnityEngine;
public abstract class BaseObject : MonoBehaviour
{
    public TransformGroup transformGroup
    {
        get
        {
            return new TransformGroup(this.transform);
        }
    }
}