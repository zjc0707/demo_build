using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PanelDialog : BasePanel<PanelDialog>
{
    public Button close, submit;
    public Text title, content;
    private UnityAction submitAction, quitAction;
    protected override void _Start()
    {
        this.submitAction = delegate
        {
            Debug.Log("empty");
        };
        this.quitAction = delegate
        {

        };
        close.onClick.AddListener(this.quitAction);
        submit.onClick.AddListener(this.submitAction);
    }
    public void Open(string contentText, UnityAction action, UnityAction after = null)
    {
        this.Open(null, contentText, action, after);
    }
    public void Open(string title, string contentText, UnityAction action, UnityAction after = null)
    {
        if (!string.IsNullOrEmpty(title))
        {
            this.title.text = title;
        }
        content.text = contentText;
        close.onClick.RemoveListener(this.quitAction);
        submit.onClick.RemoveListener(this.submitAction);
        this.submitAction = delegate
        {
            Debug.Log("123");
            if (action != null)
            {
                action();
            }
            base.Close();
            if (after != null)
            {
                after();
            }
        };
        this.quitAction = delegate
        {
            if (after != null)
            {
                after();
            }
            base.Close();
        };
        close.onClick.AddListener(this.quitAction);
        submit.onClick.AddListener(this.submitAction);
        Debug.Log("open");
        base.Open();
    }
}