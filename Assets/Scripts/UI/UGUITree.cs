using UnityEngine;
using UnityEngine.UI;

public class UGUITree : BaseUniqueObject<UGUITree>
{
    public Canvas canvasInit;
    public Canvas canvasOperate;

    private void Start()
    {
        canvasInit.gameObject.SetActive(true);
        canvasOperate.gameObject.SetActive(false);
        canvasOperate.transform.Find("ButtonQuit").GetComponent<Button>().onClick.AddListener(delegate
        {
            Sport.current.TurnToInit();
        });
    }
}