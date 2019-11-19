using UnityEngine;
using UnityEngine.UI;
public class PanelLoading : BasePanel<PanelLoading>
{
    public Text text;
    protected override void _Start()
    {
        base.Close();
        base.buttonClose.gameObject.SetActive(false);
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