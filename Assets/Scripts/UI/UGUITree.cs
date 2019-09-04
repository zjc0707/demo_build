using UnityEngine;
using UnityEngine.UI;

public class UGUITree : BaseUniqueObject<UGUITree>
{
    public Canvas canvasInit;
    public Canvas canvasOperate;
    public Canvas canvasStatic;

    private void Awake()
    {
        foreach (Image image in this.transform.GetComponentsInChildren<Image>())
        {
            if (image.name == "ButtonClose")
            {
                image.color = new Color(253 / 255f, 97 / 255f, 97 / 255f, 255 / 255f);
            }
        }
        foreach (Text text in this.transform.GetComponentsInChildren<Text>())
        {
            text.font = ResourceStatic.FONT;
        }
    }
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