using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[UIType(UIStackType.START)]
public class PanelStart : BasePanel<PanelStart>
{
    public Button buttonCreate, buttonLoad;
    public string platform;
    protected override void _Start()
    {
        base.Open();
        AssetBundleUtil.Load();
        buttonCreate.onClick.AddListener(delegate
        {
            UGUITree.current.CloseStart();
            Floor.current.Reset();
            MyCamera.current.Reset();
            PanelList.current.Reset();
        });
        buttonLoad.onClick.AddListener(delegate
        {
            PanelLoad.current.Open();
        });

#if UNITY_EDITOR
        platform = "hi,大家好,我是在unity编辑模式下";
#elif UNITY_XBOX360
       platform="hi，大家好,我在XBOX360平台";
#elif UNITY_IPHONE
       platform="hi，大家好,我是IPHONE平台";
#elif UNITY_ANDROID
       platform="hi，大家好,我是ANDROID平台";
#elif UNITY_STANDALONE_OSX
       platform="hi，大家好,我是OSX平台";
#elif UNITY_STANDALONE_WIN
       platform="hi，大家好,我是Windows平台";
#endif
        Debug.Log("Current Platform:" + platform);
    }
}