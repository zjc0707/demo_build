using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[UIType(UIStackType.START)]
public class PanelStart : BasePanel<PanelStart>
{
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