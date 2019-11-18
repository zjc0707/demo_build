using UnityEngine.UI;
public class PanelOperate : BasePanel<PanelOperate>
{
    protected override int StackType { get { return UIStackType.OPERATE; } }
    public Button buttonBack;
    protected override void _Start()
    {
        buttonBack.onClick.AddListener(delegate
        {
            UGUITree.current.OpenStart();
        });
    }
}