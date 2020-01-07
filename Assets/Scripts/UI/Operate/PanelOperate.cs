using UnityEngine.UI;
[UIType(UIStackType.OPERATE)]
public class PanelOperate : BasePanel<PanelOperate>
{
    public Button buttonBack, buttonEditFloorTile, buttonCameraRecovery;
    private Text buttonEditFloorTileText;
    protected override void _Start()
    {
        buttonCameraRecovery.onClick.AddListener(delegate
        {
            MyCamera.current.Reset();
        });
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