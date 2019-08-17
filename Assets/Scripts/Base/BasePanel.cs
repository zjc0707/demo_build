using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasePanel<T> : BaseUniqueObject<T> where T : Object
{
    public Button buttonClose;
    protected abstract void _Start();
    protected virtual void OnButtonCloseClickSupply() { }
    // Use this for initialization
    void Start()
    {
        buttonClose.onClick.AddListener(delegate
        {
            this.Close();
        });

        _Start();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        OnButtonCloseClickSupply();
    }
}
