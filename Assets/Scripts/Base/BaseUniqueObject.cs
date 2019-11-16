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
                GameObject obj = new GameObject(typeof(T).Name);
                obj.transform.position = Vector3.zero;
                obj.transform.SetParent(Unique.transform);
                _instance = obj.AddComponent<T>();
            }
            if (_instance == null)
            {
                throw new System.NullReferenceException("not found");
            }
            return _instance;
        }
    }
    private static GameObject unique;
    private static GameObject Unique
    {
        get
        {
            if (unique == null)
            {
                unique = new GameObject("Unique");
                unique.transform.position = Vector3.zero;
            }
            return unique;
        }
    }

    protected BaseUniqueObject()
    {
        _instance = this as T;
    }

}
