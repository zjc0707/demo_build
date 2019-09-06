using UnityEngine;

/// <summary>
/// 抽象类，场景中只存在一个的对象
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseUniqueObject<T> : BaseObject where T : MonoBehaviour
{
    private static T _instance;
    public static T current
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
            }
            if (_instance == null)
            {
                throw new System.NullReferenceException("not found");
            }
            return _instance;
        }
    }

    protected BaseUniqueObject()
    {
        _instance = this as T;
    }

}
