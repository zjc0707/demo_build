using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PanelDialog : BasePanel<PanelDialog>
{
    public Button close, submit;
    public Text title, content;
    private UnityAction action;
    protected override void _Start()
    {
        close.GetComponent<Button>().onClick.AddListener(base.Close);
    }
    public void Open(string contentText, UnityAction submitAction)
    {
        this.Open(null, contentText, submitAction);
    }
    public void Open(string title, string contentText, UnityAction submitAction)
    {
        if (title != null && title != "")
        {
            this.title.text = title;
        }
        content.text = contentText;
        this.action = delegate
        {
            submitAction();
            base.Close();
        };
        submit.onClick.AddListener(this.action);

        this.gameObject.SetActive(true);
    }
}