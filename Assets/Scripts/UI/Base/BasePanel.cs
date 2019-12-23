using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasePanel<T> : BaseUniqueObject<T> where T : MonoBehaviour
{
    public Button buttonClose;
    public Button buttonLink;
    protected virtual int StackType { get { return UIStackType.NULL; } }
    /// <summary>
    /// 子类中用该函数代替原来的Start()
    /// </summary>
    protected abstract void _Start();
    public virtual void Close()
    {
        if (StackType != UIStackType.NULL)
        {
            UIStackDic.Close(StackType);
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
        if (StackType != UIStackType.NULL)
        {
            UIStackDic.Clear(StackType);
        }
    }
    public virtual void Open()
    {
        if (StackType != UIStackType.NULL)
        {
            UIStackDic.Open(StackType, this.gameObject);
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
            // Debug.Log(buttonClose.transform.parent.name);
            // Debug.Log(buttonClose.transform.parent.Find("Button").GetComponent<RectTransform>().sizeDelta);
            // float height = buttonClose.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
            // buttonClose.GetComponent<RectTransform>().sizeDelta = Vector2.one * height;
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
