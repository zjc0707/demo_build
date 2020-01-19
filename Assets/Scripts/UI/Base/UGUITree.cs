using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UGUITree : BaseUniqueObject<UGUITree>
{
    public Canvas canvasInit;
    public Canvas canvasOperate;
    public Canvas canvasStart;

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
}