using System;
using UnityEngine;
using UnityEngine.UI;
public class CanvasInit : BaseUniqueObject<CanvasInit>
{
    private Action<float> hideAction;
    private const float amountTime = 1f;
    private void Start()
    {
        RectTransform buttonsRT = this.transform.Find("Buttons").GetComponent<RectTransform>();
        Vector3 buttonsRTBegin = buttonsRT.localPosition;
        Vector3 buttonsRTEnd = buttonsRT.localPosition + new Vector3(0, buttonsRT.sizeDelta.y, 0);

        RectTransform panelListRT = this.transform.Find("Panels/PanelList").GetComponent<RectTransform>();
        Vector3 panelListRTBegin = panelListRT.localPosition;
        Vector3 panelListRTEnd = panelListRT.localPosition - new Vector3(panelListRT.sizeDelta.x, 0, 0);

        RectTransform panelModelRT = this.transform.Find("Panels/PanelModel").GetComponent<RectTransform>();
        Vector3 panelModelRTBegin = panelModelRT.localPosition;
        Vector3 panelModelRTEnd = panelModelRT.localPosition - new Vector3(0, panelModelRT.sizeDelta.y, 0);

        RectTransform panelControlRT = this.transform.Find("Panels/PanelControl").GetComponent<RectTransform>();
        Vector3 panelControlRTBegin = panelControlRT.localPosition;
        Vector3 panelControlRTEnd = panelControlRT.localPosition + new Vector3(panelControlRT.sizeDelta.x, 0, 0);

        hideAction = f =>
        {
            buttonsRT.localPosition = Vector3.Lerp(buttonsRTBegin, buttonsRTEnd, f);
            panelListRT.localPosition = Vector3.Lerp(panelListRTBegin, panelListRTEnd, f);
            panelModelRT.localPosition = Vector3.Lerp(panelModelRTBegin, panelModelRTEnd, f);
            panelControlRT.localPosition = Vector3.Lerp(panelControlRTBegin, panelControlRTEnd, f);
        };
    }
    public void HideAllUI(Action rs)
    {
        PoolOfAnim.current.AddItem(amountTime, hideAction, rs);
    }
    public void ShowAllUI(Action rs)
    {
        PoolOfAnim.current.AddItem(amountTime, f => hideAction(1 - f), rs);
    }
}