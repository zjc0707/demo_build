using UnityEngine;
using UnityEngine.UI;
public class PanelLoading : BasePanel<PanelLoading>
{
    public Text text;
    public Image progress;
    protected override void _Start()
    {
        Close();
    }
    public override void Close()
    {
        base.Close();
        progress.gameObject.SetActive(false);
    }
    public void Progress(float load, float amount, string text)
    {
        progress.gameObject.SetActive(true);
        progress.transform.localScale = new Vector3(load / amount, 1f, 1f);
        Open(text);
    }
    public void WebLoading()
    {
        Open("向后端发送数据中...");
    }
    public void SceneLoading()
    {
        Open("场景加载中...");
    }
    public void Success(string content = "提交成功")
    {
        Open(content, true);
    }
    public void Error(string content = "访问出错")
    {
        Open(content, true);
    }
    public void Open(string content, bool showCloseButton = false)
    {
        text.text = content;
        base.buttonClose.gameObject.SetActive(showCloseButton);
        base.Open();
    }
}