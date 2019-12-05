using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelStart : BasePanel<PanelStart>
{
    protected override int StackType { get { return UIStackType.START; } }
    public Button buttonCreate, buttonLoad;
    protected override void _Start()
    {
        base.Open();
        AssetBundleUtil.Load();
        buttonCreate.onClick.AddListener(delegate
        {
            UGUITree.current.CloseStart();
            Floor.current.Reset();
            MyCamera.current.Reset();
            BuildingRoom.current.Reset();
        });
        buttonLoad.onClick.AddListener(delegate
        {
            PanelLoad.current.Open();
        });
    }
}