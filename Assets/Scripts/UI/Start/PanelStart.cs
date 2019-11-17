using UnityEngine;
using UnityEngine.UI;
public class PanelStart : BasePanel<PanelStart>
{
    protected override int StackType { get { return UIStackType.START; } }
    public Button buttonCreate, buttonLoad;
    protected override void _Start()
    {
        base.Open();
        buttonCreate.onClick.AddListener(delegate
        {
            UGUITree.current.CloseStart();
        });
        buttonLoad.onClick.AddListener(delegate
        {
            PanelLoad.current.Open();
        });
    }
}