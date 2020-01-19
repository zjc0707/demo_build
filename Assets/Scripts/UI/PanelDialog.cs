using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PanelDialog : BasePanel<PanelDialog>
{
    public Button buttonQuit, buttonSubmit;
    public Text title, content;
    private Action actionSubmit, actionQuit;
    protected override void _Start()
    {
        buttonQuit.onClick.AddListener(delegate
        {
            if (actionQuit != null)
            {
                actionQuit();
            }
            base.Close();
        });
        buttonSubmit.onClick.AddListener(delegate
        {
            if (actionSubmit != null)
            {
                actionSubmit();
            }
            base.Close();
        });
    }
    public void Open(string contentText)
    {
        this.Open(contentText, null);
    }
    public void Open(string contentText, Action submit, Action quit = null)
    {
        this.Open(null, contentText, submit, quit);
    }
    public void Open(string title, string contentText, Action submit, Action quit = null)
    {
        if (!string.IsNullOrEmpty(title))
        {
            this.title.text = title;
        }
        content.text = contentText;
        this.actionSubmit = submit;
        this.actionQuit = quit;
        this.buttonQuit.gameObject.SetActive(quit != null);
        base.Open();
    }
}