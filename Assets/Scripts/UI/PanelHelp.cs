using UnityEngine;
using UnityEngine.UI;
public class PanelHelp : BasePanel<PanelHelp>
{
    public Text helpText;
    private string textInit = "1.物品界面点击选择物品，再单击放置物品\n" +
                            "2.单击放置物品后打开属性界面\n" +
                            "3.双击放置物品后拾取物体\n" +
                            "4.可操作物体属性界面（如吊车）会出现操作按钮，点击进入操作模式";
    private string textOperate = "1.wasd移动物体\n" +
                            "2.(吊车专用)j-装置上升，k-装置伸长，l-吊绳伸长。按住shift则相反";
    protected override void _Start()
    {
        TurnToInit();
    }
    public void ChangeText(bool isOperate)
    {
        if (isOperate)
        {
            TurnToOperate();
        }
        else
        {
            TurnToInit();
        }
    }
    private void TurnToInit()
    {
        helpText.text = textInit;
    }
    private void TurnToOperate()
    {
        helpText.text = textOperate;
    }

}