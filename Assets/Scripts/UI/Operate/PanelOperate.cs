using UnityEngine.UI;
public class PanelOperate : BasePanel<PanelOperate>
{
    protected override int StackType { get { return UIStackType.OPERATE; } }
    public Button buttonBack;
    public Button buttonEditFloorTile;
    private Text buttonEditFloorTileText;
    protected override void _Start()
    {
        buttonBack.onClick.AddListener(delegate
        {
            UGUITree.current.OpenStart();
        });
        buttonEditFloorTile.onClick.AddListener(delegate
        {
            bool flag = FloorTile.current.gameObject.activeInHierarchy;
            FloorTile.current.gameObject.SetActive(!flag);
            buttonEditFloorTileText.text = !flag ? "关闭地面方格" : "开启地面方格";
        });
        buttonEditFloorTileText = buttonEditFloorTile.transform.Find("Text").GetComponent<Text>();
        buttonEditFloorTileText.text = FloorTile.current.gameObject.activeInHierarchy ? "关闭地面方格" : "开启地面方格";
    }
}