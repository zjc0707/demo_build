using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class UGUITree : BaseUniqueObject<UGUITree>
{
    public Canvas canvasInit;
    public Canvas canvasOperate;
    public Canvas canvasStart;
    public Canvas canvasViewModel;
    private Action canvasViewModelQuitAction;
    private void Awake()
    {
        //统一关闭按钮样式
        foreach (Image image in this.transform.GetComponentsInChildren<Image>())
        {
            if (image.name.Equals("ButtonClose"))
            {
                // Debug.Log(image.transform.parent.parent.name + "   " + image.name);
                image.color = new Color(253 / 255f, 97 / 255f, 97 / 255f, 255 / 255f);
            }
        }
        //统一设置字体
        foreach (Text text in this.transform.GetComponentsInChildren<Text>())
        {
            text.font = ResourceStatic.FONT;
        }
    }
    private void Start()
    {
        canvasViewModel.transform.Find("ButtonQuit").GetComponent<Button>().onClick.AddListener(() =>
        {
            canvasViewModelQuitAction();
        });
        canvasViewModel.gameObject.SetActive(false);
        OpenStart();
    }
    public void CloseStart()
    {
        canvasStart.gameObject.SetActive(false);
        canvasInit.gameObject.SetActive(true);
        PanelModel.current.Open();
        PanelList.current.Open();
    }
    public void OpenStart()
    {
        canvasInit.gameObject.SetActive(false);
        canvasStart.gameObject.SetActive(true);
    }
    public void ViewModelTurnOff()
    {
        PoolOfAnim.current.ViewModelTurnOff();
        PanelList.current.RecoveryBuildingDatas();
        canvasViewModel.gameObject.SetActive(false);
        CanvasInit.current.ShowAllUI(PanelState.current.ViewModelTurnOff);
    }
    public void ViewModelTurnOn()
    {
        PanelList.current.SaveBuildingDatas();
        PanelState.current.ViewModelTurnOn();
        Coordinate.Target.SetTarget(null);
        CanvasInit.current.HideAllUI(() =>
        {
            canvasViewModelQuitAction = ViewModelTurnOff;
            PoolOfAnim.current.ViewModelTurnOn();
            canvasViewModel.gameObject.SetActive(true);
        });
    }
    public void PlayAppearanceAnim()
    {
        PanelState.current.ViewModelTurnOn();
        Coordinate.Target.SetTarget(null);
        CanvasInit.current.HideAllUI(() =>
        {
            canvasViewModelQuitAction = StopAppearanceAnim;
            PanelList.current.SaveBuildingDatas();
            PoolOfAnim.current.PlayAppearanceAnim();
            canvasViewModel.gameObject.SetActive(true);
        });
    }
    public void StopAppearanceAnim()
    {
        PoolOfAnim.current.StopAppearanceAnim();
        PanelList.current.RecoveryBuildingDatas();
        canvasViewModel.gameObject.SetActive(false);
        CanvasInit.current.ShowAllUI(PanelState.current.ViewModelTurnOff);
    }
}