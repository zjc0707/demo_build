using System.Reflection;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[UIType(UIStackType.NULL)]
public abstract class BasePanel<T> : BaseUniqueObject<T> where T : MonoBehaviour
{
    public Button buttonClose;
    public Button buttonLink;
    private int? _typeValue;
    public int typeValue
    {
        get
        {
            if (_typeValue == null)
            {
                _typeValue = this.GetType().GetCustomAttribute<UIType>().value;
            }
            return _typeValue.GetValueOrDefault();
        }
    }
    /// <summary>
    /// 子类中用该函数代替原来的Start()
    /// </summary>
    protected abstract void _Start();
    public virtual void Close()
    {
        if (!this.gameObject.activeInHierarchy)
        {
            return;//已经隐藏的界面调用不进行栈操作
        }
        if (typeValue != UIStackType.NULL)
        {
            UIStackDic.Close(typeValue);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 关闭并清除历史记录
    /// </summary>
    public void Clear()
    {
        if (typeValue != UIStackType.NULL)
        {
            UIStackDic.Clear(typeValue);
        }
    }
    public virtual void Open()
    {
        if (typeValue != UIStackType.NULL)
        {
            UIStackDic.Open(typeValue, this.gameObject);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    // Use this for initialization
    void Start()
    {
        if (buttonClose == null && this.transform.Find("Top") != null)
        {
            buttonClose = this.transform.Find("Top").Find("ButtonClose").GetComponent<Button>();
        }
        if (buttonClose != null)
        {
            buttonClose.onClick.AddListener(delegate
            {
                this.Close();
            });
        }
        if (buttonLink != null)
        {
            GameObject panel = this.gameObject;
            buttonLink.onClick.AddListener(delegate
            {
                if (!panel.activeInHierarchy)
                {
                    this.Open();
                }
                else
                {
                    this.Close();
                }
            });
        }
        this.gameObject.SetActive(false);
        _Start();
    }
}
